using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.NetworkSystem;

namespace UnityStandardAssets.CrossPlatformInput {
    [NetworkSettings(channel=1)]
    public class PlayerControllerMobile : NetworkBehaviour {
        private NetworkIdentity nIdentity;
        private PlanetsNetworkManager nm;
        private const float fireRate = 0.3F;
        private float currentFireRate = fireRate;
        private float nextFire = 0.0F;

        private bool hasCollide = false;

        //Becomes true upon coming into contact with a double score object. Gives double score for 5 seconds
        public bool doubleScore = false;
        private const float doubleScoreTimeInit = 5;
        private float doubleScoreTime = doubleScoreTimeInit;


        private bool fasterFire = false;
        private float fasterFireSpeed = 0.1F;
        private const float fasterFireTimeInit = 5;
        private float fasterFireTime = fasterFireTimeInit;

        private bool shielded = false;
        private const float shieldedTimeInit = 120;
        private float shieldedTime = shieldedTimeInit;

        public float speed = 3.0F;
        public float rotSpeed = 20.0F;

        public Transform planet;
        public Transform model;
        public Transform turret;

        public Text scoreText;
        public Text winText;
        public Text id;
        // public GUIText idForObsScreen;
        public Text deathText;
        public Text deathTimerText;
        public GameObject ResourcePickUp;

        public Camera mainCamera;

        public int score;
        public int scoreToRemove;
        public GameObject projectileModel;

        public GameObject shield;
        // NetworkView networkView;

        Rigidbody rb;

        void Start()
        {
            nIdentity = GetComponent<NetworkIdentity>();
            nm = (PlanetsNetworkManager)NetworkManager.singleton;
            rb = GetComponent<Rigidbody>();
            if(!nIdentity.isLocalPlayer) return;
            nm.client.RegisterHandler(Msgs.updateLocalScore, OnClientPickupDeath);
            score = 0;
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            winText = GameObject.Find("WinText").GetComponent<Text>();
            deathText = GameObject.Find("DeathText").GetComponent<Text>();
            deathTimerText = GameObject.Find("DeathTimerText").GetComponent<Text>();
            id = GameObject.Find("ID").GetComponent<Text>();
            mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            nm.client.RegisterHandler(Msgs.serverName, OnClientReceiveName);
            nm.client.Send(Msgs.requestName, new EmptyMessage());
        }

        public void OnClientReceiveName(NetworkMessage msg)
        {
            return;
            if(!nIdentity.isLocalPlayer) return;
            Name tl = msg.ReadMessage<Name>();
            Debug.LogError("Text: " + gameObject.GetComponent<Text>());
            Text name = gameObject.GetComponent<Text>();
            name.text = tl.name;
            id.text = tl.id.ToString();
            // networkView.RPC ("SetId", RPCMode.All, id.text);
            GetComponent<PlayerNetworkHandler>().CmdSetId(id.text);
        }


        void Update(){

            if (doubleScore == true)
            {
                doubleScoreTime -= Time.deltaTime;
                Debug.Log(doubleScoreTime);
                if (doubleScoreTime <= 0)
                {
                    doubleScore = false;
                    doubleScoreTime = doubleScoreTimeInit;
                    Debug.Log("End of double Score");
                }
            }

            if (fasterFire == true){
                //Debug.LogError("Starting faster fire for " + nIdentity);
                fasterFireTime -= Time.deltaTime;
                if (fasterFireTime <= 0)
                {
                    fasterFire = false;
                    fasterFireTime = fasterFireTimeInit; //Resets timer
                    currentFireRate = fireRate; //Resets fire rate to classic fire rate. Only needed for local-viewing
                }
            }

            if (shielded == true){
                shieldedTime -= Time.deltaTime;
                if (shieldedTime <= 0){
                    shielded = false;
                    shield.SetActive(false);
                    Destroy(shield);
                    shieldedTime = fasterFireTimeInit; //Resets timer
                }
            }
            if (!nIdentity.isLocalPlayer) return;

            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

#if UNITY_ANDROID
			float aimH = CrossPlatformInputManager.GetAxis ("AimH");
			float aimV = CrossPlatformInputManager.GetAxis ("AimV");
			float forwardSpeed = speed * CrossPlatformInputManager.GetAxis("MoveV");
			float strafeSpeed = speed * CrossPlatformInputManager.GetAxis("MoveH");
#endif

#if UNITY_STANDALONE
            float aimH = (-(Input.GetKey("left") ? 1 : 0) + (Input.GetKey("right") ? 1 : 0));
            float aimV = ((Input.GetKey("up") ? 1 : 0) - (Input.GetKey("down") ? 1 : 0));
            float forwardSpeed = speed * ((Input.GetKey("w") ? 1 : 0) - (Input.GetKey("s") ? 1 : 0));
            float strafeSpeed = speed * (-(Input.GetKey("a") ? 1 : 0) + (Input.GetKey("d") ? 1 : 0));
#endif

            Vector3 moveDir = forwardSpeed * forward + strafeSpeed * right;
            rotateObject(model, moveDir.normalized);

            Vector3 turretDirection = ((forward * aimV) + (right * aimH)).normalized;
            rotateObject(turret, turretDirection);

            rb.MovePosition(transform.position + moveDir * Time.deltaTime * 5);
        }

        void rotateObject(Transform obj, Vector3 direction)
        {
            if (direction.magnitude == 0)
            {
                direction = model.forward;
            }
            Vector3 upDir = (model.position - planet.position).normalized;
            Quaternion currentRotation = Quaternion.LookRotation(direction, upDir);
            obj.rotation = Quaternion.Lerp(obj.rotation, currentRotation, Time.deltaTime * rotSpeed);

#if UNITY_ANDROID
			if(CrossPlatformInputManager.GetAxis ("AimH") + CrossPlatformInputManager.GetAxis ("AimV") != 0){
#endif
#if UNITY_STANDALONE
            if (Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("up") || Input.GetKey("down"))
            {
#endif
                if (Time.time < nextFire)
                    return;

                string name = GetComponent<Text>().text;
                if(!nm.observerCollisionsOnly()){
                    if (gameObject.tag == "PlayerPirate")
                    {
                        GetComponent<PlayerNetworkHandler>().CmdSpawnProjectile(rb.position + turret.forward, turret.forward, "ProjectilePirate", name);
                    }
                    else
                    {
                        GetComponent<PlayerNetworkHandler>().CmdSpawnProjectile(rb.position + turret.forward, turret.forward, "ProjectileSuperCorp", name);
                    }
                } else {
                    CmdFireProjectile();
                }

                nextFire = Time.time + currentFireRate;
            }
        }

        void OnCollisionEnter(Collision col){
            Debug.Log("Observer collisions only? " + nm.observerCollisionsOnly());
            if(nm.observerCollisionsOnly()){
                Debug.Log("Is observer?" + PlayerConfig.singleton.GetObserver());
                if(!PlayerConfig.singleton.GetObserver()) {
                    return;
                } else {
                    Debug.Log("I am an observer");
                }
            } else {
                if (!nIdentity.isLocalPlayer) return;
            }

            if (col.gameObject.CompareTag("DoubleScore")){
                doubleScore = true;
                Destroy(col.gameObject);
            }else if (col.gameObject.CompareTag("Shield")){ //Also need to potentially create an animation here?
                shielded = true;
                shield = Instantiate(shield);
                shield.transform.parent = this.transform;
                shield.transform.position = this.transform.position;
                shield.SetActive(true);
                Destroy(col.gameObject);

            }else if (col.gameObject.CompareTag("FasterFire")){ //Turn on faster fire rate. Still needs graphical additions
                Debug.LogError("Starting faster fire for ");
                fasterFire = true;
                Destroy(col.gameObject);
                currentFireRate = fasterFireSpeed;
            }

            else if (col.gameObject.CompareTag("ProjectilePirate") && gameObject.CompareTag("PlayerSuperCorp")){
                if ((hasCollide == false)&&(shielded==false)){
                    hasCollide = true;
                    Destroy(col.gameObject);
                    deathText.enabled = true; //Causes timer for next spawn to occur
                    deathTimerText.enabled = true;
                    mainCamera.enabled = true;

                    Text shooter = col.gameObject.GetComponent<Text>();
                    Text victim = gameObject.GetComponent<Text>();
                    Kill tc = new Kill();
                    tc.msg = shooter.text + " killed " + victim.text;
                    nm.client.Send(Msgs.clientKillFeed, tc);
                    GetComponent<PlayerNetworkHandler>().CmdSpawnResource(gameObject.transform.position, score);
                    scoreToRemove = score;

                    AddScore sc = new AddScore();
                    sc.team = 0;
                    sc.score = -score;
                    sc.obj = col.gameObject;
                    nm.client.Send(Msgs.clientTeamScore, sc);

                    score = 0;
                    SetScoreText();
                    GetComponent<FixDestroyBug>().dead = true;
                    ClientScene.RemovePlayer(0);
                }
            }else if (col.gameObject.CompareTag("ProjectileSuperCorp") && gameObject.CompareTag("PlayerPirate")){
                if ((hasCollide == false) && (shielded == false)){
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

                    AddScore sc = new AddScore();
                    sc.team = 0;
                    sc.score = -score;
                    sc.obj = col.gameObject;
                    nm.client.Send(Msgs.clientTeamScore, sc);

                    GetComponent<PlayerNetworkHandler>().CmdSpawnResource(gameObject.transform.position, score);
                    scoreToRemove = score;
                    score = 0;
                    SetScoreText();
                    GetComponent<FixDestroyBug>().dead = true;
                    ClientScene.RemovePlayer(0);
                }
            }
           
            if (col.gameObject.CompareTag("StaticResource")){ //Dealt with on the resource currently
                /*
                ResourceController resProp = col.gameObject.GetComponent<ResourceController>();
                int resourceScore = resProp.getScore();
                if (doubleScore){ //If points are to count for double, double score
                    resourceScore *= 2;
                }
                score += resourceScore;
                Debug.Log("SCORE IS" + score);
                Debug.Log("RESOURCE IS" + resourceScore);
                SetScoreText();
                AddScore sc = new AddScore();
                sc.team = 0;
                sc.score = resourceScore;
                sc.obj = col.gameObject;
                nm.client.Send(Msgs.clientTeamScore, sc);
                */
            }

            if (col.gameObject.CompareTag("ResourcePickUpDeath"))
            {

                DeathResourceProperties resProp = col.gameObject.GetComponent<DeathResourceProperties>();
                //score = score + resProp.getScore();
                //score = score + int.Parse(col.gameObject.GetComponent<Text>().text);
                Debug.Log("picked up death with score " + col.gameObject.GetComponent<Text>().text);
                //GetComponent<PlayerNetworkHandler>().CmdDestroyDeathResource(col.gameObject);
                SetScoreText();

                DeathResource dr = new DeathResource();
                dr.team = 0;
                dr.score = (int)resProp.getScore();
                dr.drID = col.gameObject;
                nm.client.Send(Msgs.deathResourceCollision, dr);
            }
        }

        //If the player dies before the server updates score, score will not be counted. Potential error?
        public void OnClientPickupDeath(NetworkMessage msg)
        {
            UpdateLocalScore uls = msg.ReadMessage<UpdateLocalScore>(); //Recieve death resource score from server
            int scoreAdd = uls.score;
            score += scoreAdd; //Add to existing score
            SetScoreText();
        }

        // function only called after the player dies to get the score that the team manager has to substract
        // hence the score has to be reset to 0 after that
        public int getScore()
        {
            return scoreToRemove;
        }   

        public void SetScoreText()
        {
            scoreText.text = score.ToString();
        }

        public void SetScoreTextNew(int scoreVal)
        {
            score += scoreVal;
            scoreText.text = score.ToString();
        }

        [Command]
        void CmdFireProjectile(){
            foreach (NetworkConnection nc in ((PlanetsNetworkManager)PlanetsNetworkManager.singleton).getUpdateListeners()){
                #if UNITY_5_4_OR_NEWER
                TargetFireProjectile(nc);
                #else
                UniqueObjectMessage uom = new UniqueObjectMessage();
                uom.netId = nIdentity.netId;
                NetworkServer.SendToClient(nc.connectionId, Msgs.fireProjectile, uom);
                #endif
            }
        }

        #if UNITY_5_4_OR_NEWER
        [TargetRpc]
        void TargetFireProjectile(NetworkConnection nc){
        #else
        public void TargetFireProjectile(UniqueObjectMessage msg){
        #endif
            SpawnProjectile();
            if (fasterFire){
                Invoke("SpawnProjectile", fireRate / 2);
            }
        }

        void SpawnProjectile(){
            GameObject projectile = Instantiate(projectileModel) as GameObject;
            projectile.GetComponent<Transform>().position = rb.position + turret.forward;
            projectile.GetComponent<ProjectileMovement>().setDirection(turret.forward);
            Destroy(projectile, 2);
        }

        void OnDestroy(){
            if(nIdentity.isLocalPlayer)
            Debug.LogError("Why was I destroyed?");
        }
    }
}
