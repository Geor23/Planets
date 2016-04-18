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

      //  public Text scoreText;
        public Text winText;
        public Text id;
        // public GUIText idForObsScreen;
        public Text deathText;
        public Text deathTimerText;
        public GameObject ResourcePickUp;

        public Camera mainCamera;
        public Transform obsCamera ;
        public Vector3 planetCenter = new Vector3(0,0,0);

      //  public int score;
     //   public int scoreToRemove;
        public GameObject projectileModel;

        public GameObject shield;

        /* Variables belonging to split screen controls */
        private bool invertControls = false;
        private Matrix4x4 reflectionMatrix;
        private bool needsReflection = false;
        private float lastMoveV;
        private float lastMoveH;

        public ResourcePowerUpManager resourcePowerUpManager;

        Rigidbody rb;

        private GameObject roundManager; //Contains reference to RoundEventsManager object

        void Start(){
            nIdentity = GetComponent<NetworkIdentity>();
            nm = (PlanetsNetworkManager)NetworkManager.singleton;
            roundManager = GameObject.Find("RoundEventsManager"); //Sets reference to RoundEvents object
            rb = GetComponent<Rigidbody>();
            resourcePowerUpManager = GameObject.FindGameObjectWithTag("Planet").GetComponent<ResourcePowerUpManager>();
            if(!nIdentity.isLocalPlayer) return;
            invertControls = nm.isSplitScreen();
            reflectionMatrix = genRefMatrix(90*Mathf.Deg2Rad);
            needsReflection = gameObject.CompareTag("PlayerSuperCorp");
            nm.client.RegisterHandler(Msgs.updateLocalScore, OnClientPickupDeath);
       //     score = 0;
      //      scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            winText = GameObject.Find("WinText").GetComponent<Text>();
            deathText = GameObject.Find("DeathText").GetComponent<Text>();
            deathTimerText = GameObject.Find("DeathTimerText").GetComponent<Text>();
            id = GameObject.Find("ID").GetComponent<Text>();
            mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            nm.client.RegisterHandler(Msgs.serverName, OnClientReceiveName);
            nm.client.Send(Msgs.requestName, new EmptyMessage());
            nm.client.RegisterHandler(Msgs.killPlayerRequestClient, OnClientKillPlayer);
        }

        public void OnClientReceiveName(NetworkMessage msg){
            return;
            if(!nIdentity.isLocalPlayer) return;
            Name tl = msg.ReadMessage<Name>();
            //Debug.LogError("Text: " + gameObject.GetComponent<Text>());
            Text name = gameObject.GetComponent<Text>();
            name.text = tl.name;
            id.text = tl.id.ToString();
            GetComponent<PlayerNetworkHandler>().CmdSetId(id.text);
        }


        void Update(){

            if (doubleScore == true){
                doubleScoreTime -= Time.deltaTime;
                //Debug.Log(doubleScoreTime);
                if (doubleScoreTime <= 0)
                {
                    doubleScore = false;
                    doubleScoreTime = doubleScoreTimeInit;
                    //Debug.Log("End of double Score");
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

            rb = GetComponent<Rigidbody>();
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            #if UNITY_ANDROID
            float aimH = CrossPlatformInputManager.GetAxis ("AimH");
            float aimV = CrossPlatformInputManager.GetAxis ("AimV");
            float moveV = CrossPlatformInputManager.GetAxis("MoveV");
            float moveH = CrossPlatformInputManager.GetAxis("MoveH");
            #endif

            #if UNITY_STANDALONE
            float aimH = (-(Input.GetKey ("left")?1:0) + (Input.GetKey ("right")?1:0));
            float aimV = ((Input.GetKey ("up")?1:0) - (Input.GetKey ("down")?1:0));
            float moveV = ((Input.GetKey ("w")?1:0) - (Input.GetKey ("s")?1:0));
            float moveH = (-(Input.GetKey ("a")?1:0) + (Input.GetKey ("d")?1:0));
            #endif

            lastMoveH = moveH;
            lastMoveV = moveV;
            
            if(needsReflection && invertControls){
                Vector3 refMove = reflectPoint(moveH, moveV);
                Vector3 refAim = reflectPoint(aimH, aimV);

                moveH = refMove[0];
                moveV = refMove[1];
                aimH = refAim[0];
                aimV = refAim[1];
            }

            float forwardSpeed = speed * moveV;
            float strafeSpeed = speed * moveH;

            Vector3 moveDir = forwardSpeed * forward + strafeSpeed * right;
            //rotateObject(model, moveDir.normalized);

            Vector3 turretDirection = ((forward * aimV) + (right * aimH)).normalized;
            rotateObject(turret, turretDirection);

            //rb.MovePosition(transform.position + moveDir * Time.deltaTime * 5);


            Vector3 worldPos = obsCamera.position + obsCamera.parent.transform.position;

            Vector3 newLocation = transform.position + moveDir * Time.deltaTime*5;
            float distPlanetToCam = Vector3.Distance(planetCenter, worldPos);
            float distPlayerToCam = Vector3.Distance(newLocation, worldPos);
            rotateObject(model, moveDir.normalized);
            rb.MovePosition(newLocation);
            
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
                        GetComponent<PlayerNetworkHandler>().CmdSpawnProjectile(rb.position + turret.forward, turret.forward, projectileModel, name);
                } else {
                    CmdFireProjectile();
                }

                nextFire = Time.time + currentFireRate;
            }
        }

        void OnCollisionEnter(Collision col){
            if(nm.observerCollisionsOnly()){
                if(!PlayerConfig.singleton.GetObserver()) return;
            } else {
                if (!nIdentity.isLocalPlayer) return;
            }

            if (col.gameObject.CompareTag("DoubleScore")) {
                doubleScore = true;
                //Call planet manager
                resourcePowerUpManager.resourceCollision(col.gameObject);
            }else if (col.gameObject.CompareTag("Shield")) { //Also need to potentially create an animation here?
                shielded = true;
                shield = Instantiate(shield);
                shield.transform.parent = this.transform;
                shield.transform.position = this.transform.position;
                shield.SetActive(true);
                //Call planet manager
                resourcePowerUpManager.resourceCollision(col.gameObject);

            }else if (col.gameObject.CompareTag("FasterFire")) { //Turn on faster fire rate. Still needs graphical additions
                fasterFire = true;
                //Destroy(col.gameObject);
                //Call planet manager
                resourcePowerUpManager.resourceCollision(col.gameObject);
                currentFireRate = fasterFireSpeed;
            }else if (col.gameObject.CompareTag("Meteor")) {
                /*
                    if ((hasCollide == false) && (shielded == false))
                    {
                        hasCollide = true;
                        resourcePowerUpManager.resourceCollision(col.gameObject);

                        //Update kill feed
                        Text shooter = col.gameObject.GetComponent<Text>();
                        Text victim = gameObject.GetComponent<Text>();
                        Kill tc = new Kill();
                        tc.msg = shooter.text + " killed " + victim.text;
                        nm.client.Send(Msgs.clientKillFeed, tc);

                        //Remove score from team
                        GetComponent<PlayerNetworkHandler>().CmdSpawnResource(gameObject.transform.position, score);
                        scoreToRemove = score;
                        AddScore sc = new AddScore();
                        sc.team = 0;
                        sc.score = -score;
                        sc.obj = this.gameObject;
                        nm.client.Send(Msgs.clientTeamScore, sc);

                        //Send death request to server, to send to Player
                        KillPlayer kp = new KillPlayer();
                        kp.netId = this.netId;
                        kp.obj = this.gameObject;
                        nm.client.Send(Msgs.killPlayer, kp);

                    }
    */
            }else if (col.gameObject.CompareTag("ProjectilePirate") && gameObject.CompareTag("PlayerSuperCorp")) {
                if ((hasCollide == false) && (shielded == false)) {
                    hasCollide = true;
                    Destroy(col.gameObject);

                    //Update kill feed
                    Text shooter = col.gameObject.GetComponent<Text>();
                    Text victim = gameObject.GetComponent<Text>();
                    Kill tc = new Kill();
                    tc.msg = shooter.text + " killed " + victim.text;
                    nm.client.Send(Msgs.clientKillFeed, tc);

                    /*
                    //Remove score from team
                    GetComponent<PlayerNetworkHandler>().CmdSpawnResource(gameObject.transform.position, score);
                    scoreToRemove = score;
                    AddScore sc = new AddScore();
                    sc.team = 0;
                    sc.score = -score;
                    sc.obj = this.gameObject;
                    nm.client.Send(Msgs.clientTeamScore, sc);

          */

                    //GetComponent<FixDestroyBug>().dead = true;
                    //if (PlayerConfig.singleton.isKillPlayerObserver){
                    //Send death request to server, to send to Player
                    KillPlayer kp = new KillPlayer();
                    kp.netId = this.netId;
                    kp.obj = this.gameObject;
                    nm.client.Send(Msgs.killPlayer, kp);
                    //}
                }
            } else if (col.gameObject.CompareTag("ProjectileSuperCorp") && gameObject.CompareTag("PlayerPirate")) {
                if ((hasCollide == false) && (shielded == false)) {
                    hasCollide = true;
                    Destroy(col.gameObject);

                    //Update kill feed
                    Text shooter = col.gameObject.GetComponent<Text>();
                    Text victim = gameObject.GetComponent<Text>();
                    Kill tc = new Kill();
                    tc.msg = shooter.text + " killed " + victim.text;
                    nm.client.Send(Msgs.clientKillFeed, tc);
                    /*
                                        //Remove score from team
                                        GetComponent<PlayerNetworkHandler>().CmdSpawnResource(gameObject.transform.position, score);
                                        scoreToRemove = score;
                                        AddScore sc = new AddScore();
                                        sc.team = 0;
                                        sc.score = -score;
                                        sc.obj = this.gameObject;
                                        nm.client.Send(Msgs.clientTeamScore, sc);

                                        //GetComponent<FixDestroyBug>().dead = true;
                                        //if (PlayerConfig.singleton.isKillPlayerObserver){
                                            //Send death request to server, to send to Player
                                            KillPlayer kp = new KillPlayer();
                                            kp.netId = this.netId;
                                            kp.obj = this.gameObject;
                                            nm.client.Send(Msgs.killPlayer, kp);
                                      //  }
                                      */
                                    }
                                    
            }else if (col.gameObject.CompareTag("ResourcePickUp")) { //Dealt with on the resource currently
                //int resourceScore = resourcePowerUpManager.collided(col.gameObject);
                int resourceScore = 1; //TODO: Make resourcePowerManager work
                if (doubleScore) { //If points are to count for double, double score
                    resourceScore *= 2;
                }
                int dictId = GetComponent<PlayerDetails>().getDictId();
                roundManager.GetComponent<RoundEvents>().getRoundScoreManager().increasePlayerScore(dictId, resourceScore);
            }else if (col.gameObject.CompareTag("ResourcePickUpDeath")) {
                //int resourceScore = resourcePowerUpManager.collided(col.gameObject);
                int resourceScore = 1; //TODO: Make resourcePowerManager work
                int dictId = GetComponent<PlayerDetails>().getDictId();
                roundManager.GetComponent<RoundEvents>().getRoundScoreManager().increasePlayerScore(dictId, resourceScore);
            }
        }

        void OnTriggerEnter(Collider col){
            if(!col.gameObject.CompareTag("InversionPlane")) return;
            if(needsReflection){
                reflectionMatrix = reflectionMatrix * genRefMatrix(Mathf.Atan2(lastMoveV, lastMoveH));
            } else {
                needsReflection = true;
                reflectionMatrix = genRefMatrix(Mathf.Atan2(lastMoveV, lastMoveH));
            }
        }

        Matrix4x4 genRefMatrix(float theta){
            Matrix4x4 rm = new Matrix4x4();
            for(int i = 0; i < 4; i++) for(int j = 0; j < 4; j++) rm[i,j] = 0.0f;
            rm[0,0] = Mathf.Cos(2 * theta);
            rm[0,1] = Mathf.Sin(2 * theta);
            rm[1,0] = Mathf.Sin(2 * theta);
            rm[1,1] = -Mathf.Cos(2 * theta);
            return rm;
        }

        Vector3 reflectPoint(float x, float y){
            Vector3 rp = new Vector3(reflectionMatrix[0,0] * x + reflectionMatrix[0,1] * y, reflectionMatrix[1,0] * x + reflectionMatrix[1,1] * y, 0);
            return rp;
        }

        //If the player dies before the server updates score, score will not be counted. Potential error?
        public void OnClientPickupDeath(NetworkMessage msg)
        {
            UpdateLocalScore uls = msg.ReadMessage<UpdateLocalScore>(); //Recieve death resource score from server
            int scoreAdd = uls.score;
     //       score += scoreAdd; //Add to existing score
            SetScoreText();
        }

        // function only called after the player dies to get the score that the team manager has to substract
        // hence the score has to be reset to 0 after that
   //     public int getScore(){
  //          return scoreToRemove;
  //      }   

        public void SetScoreText(){
    //        scoreText.text = score.ToString();
        }

        public void SetScoreTextNew(int scoreVal){
    //        score += scoreVal;
    //        scoreText.text = score.ToString();
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

        void OnClientKillPlayer(NetworkMessage msg) { //Gets called on the Player Client for death
            deathText.enabled = true; //Causes timer for next spawn to occur
            deathTimerText.enabled = true;
            mainCamera.enabled = true;
        //    score = 0;
            SetScoreText();
            ClientScene.RemovePlayer(0);
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
