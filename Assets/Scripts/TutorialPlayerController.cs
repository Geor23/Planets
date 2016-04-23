using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialPlayerController : MonoBehaviour
{

    private const float fireRate = 0.3F;
    private float currentFireRate = fireRate;
    private float nextFire = 0.0F;
    private bool hasCollide = false;



    // ------- POWER UP VARS---------------- //
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

    // Stuffs to do with models
    public GameObject projectileModel;
    public GameObject shield;
    public GameObject ResourcePickUp;
    public Transform planet;
    public Transform model;
    public Transform turret;
    public TextMesh tearDropId;
    private Rigidbody rb;

    //Tutorial Text
    public Text tutorialMessage;




    private ResourcePowerUpManager resourcePowerUpManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        resourcePowerUpManager = GameObject.FindGameObjectWithTag("Planet").GetComponent<ResourcePowerUpManager>();

    }


    void Update () {

        float aimH = (-(Input.GetKey("left") ? 1 : 0) + (Input.GetKey("right") ? 1 : 0));
        float aimV = ((Input.GetKey("up") ? 1 : 0) - (Input.GetKey("down") ? 1 : 0));
        float moveV = ((Input.GetKey("w") ? 1 : 0) - (Input.GetKey("s") ? 1 : 0));
        float moveH = (-(Input.GetKey("a") ? 1 : 0) + (Input.GetKey("d") ? 1 : 0));

        if (doubleScore == true)
        {
            doubleScoreTime -= Time.deltaTime;
            if (doubleScoreTime <= 0)
            {
                doubleScore = false;
                doubleScoreTime = doubleScoreTimeInit;
            }
        }

        if (fasterFire == true)
        {
            fasterFireTime -= Time.deltaTime;
            if (fasterFireTime <= 0)
            {
                fasterFire = false;
                fasterFireTime = fasterFireTimeInit; //Resets timer
                currentFireRate = fireRate; //Resets fire rate to classic fire rate. Only needed for local-viewing
            }
        }

        if (shielded == true)
        {
            shieldedTime -= Time.deltaTime;
            if (shieldedTime <= 0)
            {
                shielded = false;
                shield.SetActive(false);
                Destroy(shield);
                shieldedTime = fasterFireTimeInit; //Resets timer
            }
        }
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("DoubleScore"))
        {
            doubleScore = true;

            //Call planet manager
            resourcePowerUpManager.powerUpCollision(col.gameObject);

            tutorialMessage.text = "Get the DOUBLE SCORE Power Up to gain double the resource points!!!";
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
            tutorialMessage.text = "Get the SHIELD Power Up and became INDESTRUCTIBLE for a few seconds!!!";

        }
        else if (col.gameObject.CompareTag("FasterFire"))
        { //Turn on faster fire rate. Still needs graphical additions
            fasterFire = true;

            //Call planet manager
            resourcePowerUpManager.powerUpCollision(col.gameObject);
            currentFireRate = fasterFireSpeed;

            tutorialMessage.text = "Get the FASTER FIRE Power Up and DOUBLE your FIREPOWER!!!";

        }
        else if (col.gameObject.CompareTag("Meteor"))
        {
            //TODO
        }
        //TOFIX
        else if (col.gameObject.CompareTag("ProjectilePirate") && gameObject.CompareTag("PlayerSuperCorp"))
        {
            if ((hasCollide == false) && (shielded == false))
            {
                //hasCollide = true;
                int killerId = col.gameObject.GetComponent<ProjectileData>().ownerId;
                Destroy(col.gameObject);
                //roundEvents.registerKill(netId, playerDetails.getDictId(), killerId);
                tutorialMessage.text = "Getting DESTROYED causes you to lose ALL gathered RESOURCES!!!";

            }
        }
        else if (col.gameObject.CompareTag("ProjectileSuperCorp") && gameObject.CompareTag("PlayerPirate"))
        {
            if ((hasCollide == false) && (shielded == false))
            {
                //hasCollide = true;
                int killerId = col.gameObject.GetComponent<ProjectileData>().ownerId;
                Destroy(col.gameObject);
                //  roundEvents.registerKill(netId, playerDetails.getDictId(), killerId);
                tutorialMessage.text = "Getting DESTROYED causes you to lose ALL gathered RESOURCES!!!";

            }
        }
        else if (col.gameObject.CompareTag("ResourcePickUp"))
        { //Dealt with on the resource currently
            int resourceScore = resourcePowerUpManager.resourcePickUpCollision(col.gameObject);
            if (doubleScore)
            { //If points are to count for double, double score
                resourceScore *= 2;
            }
            //int dictId = playerDetails.getDictId();
            //roundEvents.getRoundScoreManager().increasePlayerScore(dictId, resourceScore);
            tutorialMessage.text = "RESOURCES are the most important pick up, gather more thatn the enemy team to WIN!!!";

        }

        else if (col.gameObject.CompareTag("ResourcePickUpDeath"))
        {
            //int resourceScore = resourcePowerUpManager.collided(col.gameObject);
            int resourceScore = 1; //TODO: Make resourcePowerManager work
           // int dictId = playerDetails.getDictId();
           // roundEvents.GetComponent<RoundEvents>().getRoundScoreManager().increasePlayerScore(dictId, resourceScore);
        }
    }
}