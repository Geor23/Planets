using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.NetworkSystem;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

[NetworkSettings(channel = 1)]
public class PlayerControllerMobile : NetworkBehaviour {


    private NetworkIdentity nIdentity;
    private PlanetsNetworkManager nm;
    public PersonalPlayerInfo ppi;
    public PlayerManager pm;

    public  float fireRate;
    public  float fasterFireTimeInit;
    public  float doubleScoreTimeInit;
    public  float shieldedTimeInit;


    private float currentFireRate;
    private float nextFire = 0.0F;    

    // ------- POWER UP VARS---------------- //
    public bool doubleScore = true;
    private float doubleScoreTime;


    private bool fasterFire = false;
    private float fasterFireTime;
    private float fasterFireSpeed = 0.1f;

    private bool shielded = false;
    private float shieldedTime;

    // -------------------------------------- //

    public float speed = 3.0F;
    public float rotSpeed = 20.0F;

    // Stuffs to do with models
    public GameObject projectileModel;
    public GameObject shield;
    public GameObject ResourcePickUp;
    public GameObject ResourcePickUpDeath;
    public Transform planet;
    public Transform model;
    public Transform turret;
    public GameObject tearDrop;
    public TextMesh tearDropText;
    private Rigidbody rb;
    public GameObject pin;


    public Camera mainCamera;
    public Vector3 planetCenter = new Vector3(0, 0, 0);

    /* Variables belonging to split screen controls */
    private bool invertControls = false;
    private Matrix4x4 reflectionMatrix;
    private bool needsReflection = false;
    private float lastMoveV;
    private float lastMoveH;

    public PlayerDetails playerDetails;

    private ResourcePowerUpManager resourcePowerUpManager;
    private RoundPlayerObjectManager roundPlayerObjectManager;
    private RoundEvents roundEvents; //Contains reference to RoundEventsManager object

    private bool dead = true;
    private bool pinScaling = false;

    [SyncVar]
    public int dictId;

    public override void OnStartClient(){
        base.OnStartClient();
        Debug.Log("DictID is " + dictId);
        nIdentity = GetComponent<NetworkIdentity>();
        nm = (PlanetsNetworkManager)NetworkManager.singleton;
        ppi = PersonalPlayerInfo.singleton;
        pm = PlayerManager.singleton;
        rb = GetComponent<Rigidbody>();
        invertControls = nm.isSplitScreen();
        reflectionMatrix = genRefMatrix(90 * Mathf.Deg2Rad);
        roundEvents = GameObject.Find("RoundEvents").GetComponent<RoundEvents>(); //Sets reference to RoundEvents object
        resourcePowerUpManager = GameObject.FindGameObjectWithTag("Planet").GetComponent<ResourcePowerUpManager>();
        needsReflection = gameObject.CompareTag("PlayerSuperCorp");
        //Debug.Log("Player + " + PlayerManager.singleton.getPlayer(dictId));

        if (pm.checkIfExists(dictId)) {
            playerDetails.setPlayerDetails(dictId, pm.getPlayer(dictId));
        } else {
            playerDetails.setPlayerDetails(dictId, ppi.getPlayer());
        }
        Debug.Log("Setting teardrop id to " + playerDetails.getObsId().ToString()); //BUG
        tearDropText.text = playerDetails.getObsId().ToString();
        if(nIdentity.isLocalPlayer) gameObject.transform.localScale = new Vector3(3,3,3);
        dead = false;

        currentFireRate = fireRate;
        doubleScoreTime = doubleScoreTimeInit;
        fasterFireTime = fasterFireTimeInit;
        shieldedTime = shieldedTimeInit;

    }

    public override void OnStartLocalPlayer(){
        PersonalPlayerInfo.singleton.getPlayer().setPlayerObject(gameObject);
    }

    void Update() {
/*
    	Debug.Log ("AimH Crossplatform:" + CrossPlatformInputManager.GetAxis ("AimH"));
        Debug.Log ("AimV Crossplatform:" + CrossPlatformInputManager.GetAxis ("AimV"));
        Debug.Log ("MoveH Crossplatform:" + CrossPlatformInputManager.GetAxis ("MoveH"));
        Debug.Log ("MoveV Crossplatform:" + CrossPlatformInputManager.GetAxis ("MoveV"));
        Debug.Log ("AimH Input:" + Input.GetAxis ("AimH"));
        Debug.Log ("AimV Input:" + Input.GetAxis ("AimV"));
        Debug.Log ("MoveH Input:" + Input.GetAxis ("MoveH"));
        Debug.Log ("MoveV Input:" + Input.GetAxis ("MoveV"));
*/
        if(nIdentity == null) return;
        if(nIdentity.isLocalPlayer) gameObject.transform.localScale = new Vector3(3,3,3);

        if (doubleScore == true) {
            doubleScoreTime -= Time.deltaTime;
            if (doubleScoreTime <= 0)
            {
                doubleScore = false;
                doubleScoreTime = doubleScoreTimeInit;
            }
        }

        if (fasterFire == true) {
            fasterFireTime -= Time.deltaTime;
            if (fasterFireTime <= 0)
            {
                fasterFire = false;
                fasterFireTime = fasterFireTimeInit; //Resets timer
                currentFireRate = fireRate; //Resets fire rate to classic fire rate. Only needed for local-viewing
            }
        }

        if (shielded == true) {
            shieldedTime -= Time.deltaTime;
            if (shieldedTime <= 0) {
                shielded = false;
                shield.SetActive(false);
                Destroy(shield);
                shieldedTime = fasterFireTimeInit; //Resets timer
            }
        }
        Transform obsCam = GameObject.FindGameObjectsWithTag("Observer")[0].transform.GetChild(0);
        tearDrop.transform.LookAt(obsCam.position, -obsCam.up);
        if (!nIdentity.isLocalPlayer) return;

        rb = GetComponent<Rigidbody>();
        // Vector3 forward = transform.forward;
        // Vector3 right = transform.right;
        Vector3 forward = obsCam.up;
        Vector3 right = obsCam.right;
        #if UNITY_ANDROID
        float aimH = CrossPlatformInputManager.GetAxis ("AimH");
        float aimV = CrossPlatformInputManager.GetAxis ("AimV");
        float moveV = CrossPlatformInputManager.GetAxis("MoveV");
        float moveH = CrossPlatformInputManager.GetAxis("MoveH");
        #endif

        #if UNITY_STANDALONE
        float aimH = (-(Input.GetKey("left") ? 1 : 0) + (Input.GetKey("right") ? 1 : 0));
        float aimV = ((Input.GetKey("up") ? 1 : 0) - (Input.GetKey("down") ? 1 : 0));
        float moveV = ((Input.GetKey("w") ? 1 : 0) - (Input.GetKey("s") ? 1 : 0));
        float moveH = (-(Input.GetKey("a") ? 1 : 0) + (Input.GetKey("d") ? 1 : 0));
        if (aimH == 0) {aimH = Input.GetAxis("AimH");}
        if (aimV == 0) {aimV = Input.GetAxis("AimV");}
        if (moveH == 0) {moveH = Input.GetAxis("MoveH");}
        if (moveV == 0) {moveV = Input.GetAxis("MoveV");}
        #endif


        lastMoveH = moveH;
        lastMoveV = moveV;

        if (needsReflection && invertControls) {
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
        Vector3 turretDirection = ((forward * aimV) + (right * aimH)).normalized;
        rotateObject(turret, turretDirection);
        Vector3 worldPos = Vector3.zero, 
                newLocation = Vector3.zero;
        if(obsCam == null){
            Debug.Log("Can't find observer!");
        } else{
            worldPos = obsCam.position + obsCam.parent.transform.position;
            newLocation = transform.position + moveDir * Time.deltaTime * 5;
        }


        float distPlanetToCam = Vector3.Distance(planetCenter, worldPos);
        float distPlayerToCam = Vector3.Distance(newLocation, worldPos);

        if (distPlanetToCam - 30 > distPlayerToCam) {
            rotateObject(model, moveDir.normalized);
            rb.MovePosition(newLocation);
        } else {
            Debug.Log("can't move!");
        }
       

    }

    void rotateObject(Transform obj, Vector3 direction) {
        if (direction.magnitude == 0) {
            direction = model.forward;
        }
        Vector3 upDir = (model.position - planet.position).normalized;
        Quaternion currentRotation = Quaternion.LookRotation(direction, upDir);
        obj.rotation = Quaternion.Lerp(obj.rotation, currentRotation, Time.deltaTime * rotSpeed);

        #if UNITY_ANDROID
		if(CrossPlatformInputManager.GetAxis ("AimH") + CrossPlatformInputManager.GetAxis ("AimV") != 0){
        #endif
        #if UNITY_STANDALONE
        if (Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("up") || Input.GetKey("down") || (Input.GetAxis ("AimH") + Input.GetAxis ("AimV")) != 0) 
        {
        #endif
            if (Time.time < nextFire)
                return;
            CmdFireProjectile();
            nextFire = Time.time + currentFireRate;
        }
    }

    void OnCollisionEnter(Collision col){
        if(dead) return;
        if(nm == null) return;
        if (nm.observerCollisionsOnly()){
            if (!PlayerConfig.singleton.GetObserver()) return;
        }
        else
        {
            if (!nIdentity.isLocalPlayer) return;
        }

        if (col.gameObject.CompareTag("DoubleScore"))
        {
            doubleScore = true;

            //Call planet manager
            resourcePowerUpManager.powerUpCollision(col.gameObject);
        }
        else if (col.gameObject.CompareTag("Shield"))
        { //Also need to potentially create an animation here?
            shielded = true;
            shield = Instantiate(shield);
            shield.transform.parent = this.transform;
            shield.transform.position = this.transform.position;
            shield.SetActive(true);

            //Call planet resource  manager
            resourcePowerUpManager.powerUpCollision(col.gameObject);

        }
        else if (col.gameObject.CompareTag("FasterFire"))
        { //Turn on faster fire rate. Still needs graphical additions
            fasterFire = true;

            //Call planet manager
            resourcePowerUpManager.powerUpCollision(col.gameObject);
            currentFireRate = fasterFireSpeed;
        }
        else if (col.gameObject.CompareTag("Meteor"))
        {
            //TODO
        }
        //TOFIX
        else if (col.gameObject.CompareTag("ProjectilePirate") || col.gameObject.CompareTag("ProjectileSuperCorp")) {
            if (!dead && !shielded) {
                dead = true;
                int killerId = col.gameObject.GetComponent<ProjectileData>().ownerId;
                gameObject.GetComponent<Exploder>().expl();
                Destroy(col.gameObject);
                roundEvents.registerKill(netId, playerDetails.getDictId(), killerId);
            }
        }
        else if (col.gameObject.CompareTag("ResourcePickUp")) { //Dealt with on the resource currently
            int resourceScore = resourcePowerUpManager.resourcePickUpCollision(col.gameObject);
            if (doubleScore) { //If points are to count for double, double score
                resourceScore *= 2;
            }
            int dictId = playerDetails.getDictId();
            roundEvents.getRoundScoreManager().increasePlayerScore(dictId, resourceScore);
        }

        else if (col.gameObject.CompareTag("ResourcePickUpDeath")) {
            //int resourceScore = resourcePowerUpManager.collided(col.gameObject);
            int resourceScore = 1; //TODO: Make resourcePowerManager work
            int dictId = playerDetails.getDictId();
            roundEvents.GetComponent<RoundEvents>().getRoundScoreManager().increasePlayerScore(dictId, resourceScore);
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

    [Command]
    void CmdFireProjectile(){
        foreach (NetworkConnection nc in ((PlanetsNetworkManager)PlanetsNetworkManager.singleton).getUpdateListeners()){
            #if UNITY_5_4_OR_NEWER
            TargetFireProjectile(nc);
            #else
            UniqueObjectMessage uom = new UniqueObjectMessage();
            uom.netId = GetComponent<NetworkIdentity>().netId;
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
        projectile.GetComponent<ProjectileData>().ownerId = playerDetails.getDictId();
        Destroy(projectile, 2);
    }


    public void Ping() {
        if(pinScaling) return;
        pinScaling = true;
        Vector3 temp = pin.transform.localScale;
        pin.transform.localScale = Vector3.Lerp (pin.transform.localScale, 50*temp, Time.deltaTime);
   
        StartCoroutine(Wait(temp));
    }

 
    IEnumerator Wait(Vector3 temp) {
        yield return new WaitForSeconds(1f);      
        pin.transform.localScale = temp;
        yield return new WaitForSeconds(1f);      
        pin.transform.localScale = Vector3.Lerp (pin.transform.localScale, 50*temp, Time.deltaTime);
        yield return new WaitForSeconds(1f);      
        pin.transform.localScale = temp;
        pinScaling = false;
    }
}
