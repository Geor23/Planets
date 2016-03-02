using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput {
	public class PlayerControllerMobile : MonoBehaviour {
		private NetworkIdentity nIdentity;
		private NetworkManager nm;
        private const float fireRate = 0.3F;
		private float nextFire = 0.0F;

		private bool hasCollide = false;

		public float speed = 3.0F;
		public float rotSpeed = 20.0F;

		public Transform planet;
		public Transform model;
		public Transform turret;

		public Text scoreText;
		public Text winText;
		public Text deathText;
		public Text deathTimerText;
		public GameObject ResourcePickUp;

		public Camera mainCamera;

		public int score;
		public int scoreToRemove;

		Rigidbody rb;

		void Start(){
			nIdentity = GetComponent<NetworkIdentity>();
			nm = NetworkManager.singleton;
			score = 0;
			scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
			winText = GameObject.Find("WinText").GetComponent<Text>();
			deathText = GameObject.Find("DeathText").GetComponent<Text>();
			deathTimerText = GameObject.Find("DeathTimerText").GetComponent<Text>();
			mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
		}

		void Update () {
			if(nIdentity.isLocalPlayer){

				rb = GetComponent<Rigidbody>();
				Vector3 forward = transform.forward;
				Vector3 right = transform.right;

#if UNITY_ANDROID
				float aimH = CrossPlatformInputManager.GetAxis ("AimH");
				float aimV = CrossPlatformInputManager.GetAxis ("AimV");
				float forwardSpeed = speed * CrossPlatformInputManager.GetAxis("MoveV");
				float strafeSpeed = speed * CrossPlatformInputManager.GetAxis("MoveH");
#endif

#if UNITY_STANDALONE
				float aimH = (-(Input.GetKey ("left")?1:0) + (Input.GetKey ("right")?1:0));
				float aimV = ((Input.GetKey ("up")?1:0) - (Input.GetKey ("down")?1:0));
				float forwardSpeed = speed * ((Input.GetKey ("w")?1:0) - (Input.GetKey ("s")?1:0));
				float strafeSpeed = speed * (-(Input.GetKey ("a")?1:0) + (Input.GetKey ("d")?1:0));
#endif
				Vector3 moveDir = forwardSpeed * forward + strafeSpeed * right;
				rotateObject(model, moveDir.normalized);

				Vector3 turretDirection = ((forward * aimV) + (right * aimH)).normalized;
				rotateObject(turret, turretDirection);

				rb.MovePosition(transform.position + moveDir * Time.deltaTime*5);
			}
		}
		void rotateObject (Transform obj, Vector3 direction) {
			if (direction.magnitude == 0) {
				direction = model.forward;
			}
				Vector3 upDir = (model.position - planet.position).normalized;	
				Quaternion currentRotation = Quaternion.LookRotation(direction, upDir);
				obj.rotation = Quaternion.Lerp(obj.rotation, currentRotation, Time.deltaTime*rotSpeed);

#if UNITY_ANDROID
				if(CrossPlatformInputManager.GetAxis ("AimH") + CrossPlatformInputManager.GetAxis ("AimV") != 0){
#endif 
#if UNITY_STANDALONE
				if(Input.GetKey ("left") || Input.GetKey ("right") || Input.GetKey ("up") || Input.GetKey ("down")){
#endif
					if(Time.time < nextFire)
						return;

			string name = GetComponent<Text>().text;	        
	        Debug.Log("Got name " + name);

                if (gameObject.tag == "PlayerPirate"){
                    GetComponent<PlayerNetworkHandler>().CmdSpawnProjectile(rb.position + turret.forward, turret.forward, "ProjectilePirate", name);
                }
                else{
                    GetComponent<PlayerNetworkHandler>().CmdSpawnProjectile(rb.position + turret.forward, turret.forward, "ProjectileSuperCorp", name);
                }

	      	nextFire = Time.time + fireRate;
	      }
		}
		
		void OnCollisionEnter(Collision col){

			if (!nIdentity.isLocalPlayer) return;
            if (col.gameObject.CompareTag("ProjectilePirate") && gameObject.CompareTag("PlayerSuperCorp")){
                if (hasCollide == false) {
                    hasCollide = true;
                    Destroy(col.gameObject);
                    deathText.enabled = true;
                    deathTimerText.enabled = true;
                    mainCamera.enabled = true;

                    Text shooter = col.gameObject.GetComponent<Text>();
                    Text victim = gameObject.GetComponent<Text>();
                    Kill tc = new Kill();
    				tc.msg = shooter.text + " killed " + victim.text;
    				nm.client.Send(Msgs.clientKillFeed, tc);

                    GetComponent<PlayerNetworkHandler>().CmdSpawnResource(gameObject.transform.position, score);
                    scoreToRemove = score;
                    score = 0;
                    SetScoreText();
                    ClientScene.RemovePlayer(0);
                }
            } else if (col.gameObject.CompareTag("ProjectileSuperCorp") && gameObject.CompareTag("PlayerPirate")){
                if (hasCollide == false){
                    hasCollide = true;
                    Destroy(col.gameObject);
                    deathText.enabled = true;
                    deathTimerText.enabled = true;
                    mainCamera.enabled = true;

                    Text shooter = col.gameObject.GetComponent<Text>();
                    Text victim = gameObject.GetComponent<Text>();
                    Kill tc = new Kill();
    				tc.msg = shooter.text + " killed " + victim.text;
    				nm.client.Send(Msgs.clientKillFeed, tc);

                    GetComponent<PlayerNetworkHandler>().CmdSpawnResource(gameObject.transform.position, score);
                    scoreToRemove = score;
                    score = 0;
                    SetScoreText();
                    ClientScene.RemovePlayer(0);
                }
            }

                if (col.gameObject.CompareTag("ResourcePickUp")){
				ResourceProperties resProp = col.gameObject.GetComponent<ResourceProperties>();
				score = score + resProp.getScore();
                //Network.Destroy(col.gameObject);
				SetScoreText();
				AddScore sc = new AddScore();
				sc.team = 0;
				sc.score = (int) resProp.getScore();
                sc.obj = col.gameObject;
				nm.client.Send(Msgs.clientTeamScore, sc);
			}

            if (col.gameObject.CompareTag("StaticResource")){
                ResourceController resProp = col.gameObject.GetComponent<ResourceController>();
                score = score + resProp.getScore();
                SetScoreText();
                AddScore sc = new AddScore();
                sc.team = 0;
                sc.score = (int)resProp.getScore();
                sc.obj = col.gameObject;
                nm.client.Send(Msgs.clientTeamScore, sc);
            }

            if (col.gameObject.CompareTag("ResourcePickUpDeath")){

				DeathResourceProperties resProp = col.gameObject.GetComponent<DeathResourceProperties>();
				score = score + resProp.getScore();
				Debug.Log("picked up death with score "+ resProp.getScore());
                GetComponent<PlayerNetworkHandler>().CmdDestroyDeathResource(col.gameObject);
				SetScoreText();

				AddScore sc = new AddScore();
				sc.team = 0;
				sc.score = (int) resProp.getScore();
				NetworkManager.singleton.client.Send(Msgs.clientTeamScore, sc);
			}
		}


		// function only called after the player dies to get the score that the team manager has to substract
		// hence the score has to be reset to 0 after that
		public int getScore(){
			return scoreToRemove;
		}

		public void SetScoreText(){
            scoreText.text = "Score: " + score.ToString();
        }	

		public void SetScoreTextNew(int scoreVal){
            score += scoreVal;
			scoreText.text = "Score: " + score.ToString();
		}
	}
}