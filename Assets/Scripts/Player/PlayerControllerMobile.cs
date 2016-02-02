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

		public float speed = 3.0F;
		public float rotSpeed = 12.0F;

		public Transform planet;
		public Transform model;
		public Transform turret;

		public Text scoreText;
		public Text winText;
		public Text deathText;
		public Text deathTimerText;

		public Camera mainCamera;

		public int score;

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

				//#if UNITY_ANDROID
				float aimH = CrossPlatformInputManager.GetAxis ("AimH");
				float aimV = CrossPlatformInputManager.GetAxis ("AimV");
				float forwardSpeed = speed * CrossPlatformInputManager.GetAxis("MoveV");
				float strafeSpeed = speed * CrossPlatformInputManager.GetAxis("MoveH");
				// #endif
				// #if UNITY_STANDALONE
				// float aimH = ((Input.GetKey ("left")?1:0) - (Input.GetKey ("right")?1:0));
				// float aimV = ((Input.GetKey ("up")?1:0) - (Input.GetKey ("down")?1:0));
				// float forwardSpeed = speed * ((Input.GetKey ("w")?1:0) - (Input.GetKey ("s")?1:0));
				// float strafeSpeed = speed * ((Input.GetKey ("a")?1:0) - (Input.GetKey ("d")?1:0));
				// #endif
				Vector3 turretDirection = ((forward * aimV) + (right * aimH)).normalized;
				rotateObject(turret, turretDirection);

				Vector3 moveDir = forwardSpeed * forward + strafeSpeed * right;
				rotateObject(model, moveDir.normalized);
				rb.MovePosition(transform.position + moveDir * Time.deltaTime*5);
			}
		}
		void rotateObject (Transform obj, Vector3 direction) {
			if (direction.magnitude > 0.1) {
				Vector3 upDir = (model.position - planet.position).normalized;	
				Quaternion currentRotation = Quaternion.LookRotation(direction, upDir);
				obj.rotation = Quaternion.Lerp(obj.rotation, currentRotation, Time.deltaTime*rotSpeed);

				if(CrossPlatformInputManager.GetAxis ("AimH") + CrossPlatformInputManager.GetAxis ("AimV") != 0){
					if(Time.time < nextFire)
						return;

	        GetComponent<PlayerNetworkHandler>().CmdSpawnProjectile(rb.position + direction + upDir, direction);
	      	nextFire = Time.time + fireRate;
	      }
			}
		}
		
		void OnCollisionEnter(Collision col){
			if(col.gameObject.CompareTag("projectile")){
				Debug.Log("In OnCollisionEnter projectile");
				Destroy(col.gameObject);
				deathText.enabled = true;
				deathTimerText.enabled = true;
				mainCamera.enabled = true;
				ClientScene.RemovePlayer(0);
			}
			
			if(col.gameObject.CompareTag("ResourcePickUp")){
				ResourceProperties resProp = col.gameObject.GetComponent<ResourceProperties>();
				score = score + resProp.getScore();
				Destroy(col.gameObject);
				SetScoreText();

				AddScore sc = new AddScore();
				sc.team = (int) gameObject.GetComponent<TeamMember>().getTeamID();
				sc.score = (int) resProp.getScore();
				NetworkManager.singleton.client.Send(Msgs.clientTeamScore, sc);
			}
		}

			
		void SetScoreText(){
			scoreText.text = "Score: " + score.ToString();
		}
	}
}