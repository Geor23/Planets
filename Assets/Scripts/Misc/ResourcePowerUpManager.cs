using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class ResourcePowerUpManager : MonoBehaviour {

    //used to normalize objects to surface
    public float planetSize = 50;

    public GameObject resourceObject;
    public GameObject shieldObject;
    public GameObject fasterFireObject;
    public GameObject doubleScoreObject;
    public GameObject meteorObject;
    public Vector3 planetCenter = new Vector3(0, 0, 0);
    private int minResourceScore = 1;
    private int maxResourceScore = 11;
    private int initialScore = 1;
    private NetworkManager nm = NetworkManager.singleton;

    private List<GameObject> resources;
    private List<GameObject> shields;
    private List<GameObject> doubleScore;
    private List<GameObject> fasterFire;
    private List<GameObject> meteor;

    public GameObject[] resourceSpawnPoints;
    public GameObject[] shieldSpawnPoints;
    public GameObject[] doubleScoreSpawnPoints;
    public GameObject[] fasterFireSpawnPoints;
    public GameObject[] meteorSpawnPoints;




    public int maxResourceOnPlanet = 5;
    public int minResourceOnPlanet = 1;

    public int maxShieldOnPlanet = 2;
    public int minShieldOnPlanet = 1;

    public int maxDoubleScoreOnPlanet = 2;
    public int minDoubleScoreOnPlanet = 1;

    public int maxFasterFireOnPlanet = 2;
    public int minFasterFireOnPlanet = 1;

    public int maxMeteorOnPlanet = 1;
    public int minMeteorFireOnPlanet = 1;

    public float resourceSpawnTime = 0.1f;

    public float powerUpSpawnTime = 10.5f;

    public float meteorSpawnTime = 5.5f;

    void Start() {
        resources = new List<GameObject>();
        fasterFire = new List<GameObject>();
        doubleScore = new List<GameObject>();
        shields = new List<GameObject>();
        meteor = new List<GameObject>();

        normalizeGameObjectsToPlanet(resourceSpawnPoints);
        normalizeGameObjectsToPlanet(shieldSpawnPoints);
        normalizeGameObjectsToPlanet(doubleScoreSpawnPoints);
        normalizeGameObjectsToPlanet(fasterFireSpawnPoints);

        //Spawn Resources, Power Ups and Meteors and initialize lists 

        InvokeRepeating("spawnResource", 0.0f, resourceSpawnTime);
        InvokeRepeating("UpdateRandomResourceScoreValue", 1, 0.5f);

        InvokeRepeating("spawnFasterFire", 0.0f, powerUpSpawnTime);
        InvokeRepeating("spawnDoubleScore", 0.0f, powerUpSpawnTime);
        InvokeRepeating("spawnShield", 0.0f, powerUpSpawnTime);

        InvokeRepeating("spawnMeteor", 0.0f, meteorSpawnTime);
    }

    void normalizeGameObjectsToPlanet(GameObject[] list) {
        foreach (GameObject g in list) {
            g.transform.position = g.transform.position.normalized * planetSize;
        }
    }

    void UpdateRandomResourceScoreValue() {
        int resourceID = Random.Range(0, resources.Count);
        resources[resourceID].GetComponent<CurrentResourceScore>().resourceScore += 1;
        if (resources[resourceID].GetComponent<CurrentResourceScore>().resourceScore > 2 * maxResourceScore) { initialScore = 2 * maxResourceScore; }
        else
        {
            ResourceRescale(resources[resourceID]);
        }
    }

    void ResourceRescale(GameObject resourceGameObject) {
        float tmp = (maxResourceScore - minResourceScore) / 5;
        int currentResourceScore = resourceGameObject.GetComponent<CurrentResourceScore>().resourceScore;
        Vector3 scale = (resourceGameObject.transform.localScale*1.1f);
        resourceGameObject.transform.localScale = scale;
    }

    //Change function to run with the new Resource Class
    public int resourcePickUpCollision(GameObject collidedResourcePickUp) {
        //Pass score, destroy object and remove it from list 
        int resourceScore = collidedResourcePickUp.GetComponent<CurrentResourceScore>().resourceScore;
        Destroy(collidedResourcePickUp);
        resources.Remove(collidedResourcePickUp);
        return resourceScore;
    }

    public void powerUpCollision(GameObject collidedPowerUpgameObject) {
        switch (collidedPowerUpgameObject.tag) {
            case "Shield":
                Destroy(collidedPowerUpgameObject);
                shields.Remove(collidedPowerUpgameObject);
                break;
            case "DoubleScore":
                Destroy(collidedPowerUpgameObject);
                doubleScore.Remove(collidedPowerUpgameObject);
                break;
            case "FasterFire":
                Destroy(collidedPowerUpgameObject);
                fasterFire.Remove(collidedPowerUpgameObject);
                break;
        }
    }

    public void meteorCollision(GameObject collidedMeteorObject) {
        collidedMeteorObject.GetComponent<Exploder>().expl();
        Destroy(collidedMeteorObject);
        meteor.Remove(collidedMeteorObject);
    }

    //Review way it updates resource score value
    void spawnResource() {
        for (int i = resources.Count; i < maxResourceOnPlanet; i++) {
            int spawnIndex = Random.Range(0, resourceSpawnPoints.Length);
            resources.Add((GameObject)Instantiate(resourceObject, resourceSpawnPoints[spawnIndex].transform.position, resourceSpawnPoints[spawnIndex].transform.rotation));
            resources[i].GetComponent<CurrentResourceScore>().resourceScore = initialScore;
        }
    }

    void spawnFasterFire() {
        for (int i = fasterFire.Count; i < maxFasterFireOnPlanet; i++) {
            int spawnIndex = Random.Range(0, fasterFireSpawnPoints.Length);
            fasterFire.Add((GameObject)Instantiate(fasterFireObject, fasterFireSpawnPoints[spawnIndex].transform.position, fasterFireSpawnPoints[spawnIndex].transform.rotation));
        }
    }

    void spawnDoubleScore() {
        for (int i = doubleScore.Count; i < maxDoubleScoreOnPlanet; i++) {
            int spawnIndex = Random.Range(0, doubleScoreSpawnPoints.Length);
            doubleScore.Add((GameObject)Instantiate(doubleScoreObject, doubleScoreSpawnPoints[spawnIndex].transform.position, doubleScoreSpawnPoints[spawnIndex].transform.rotation));
        }
    }

    void spawnShield() {
        for (int i = shields.Count; i < maxShieldOnPlanet; i++) {
            int spawnIndex = Random.Range(0, shieldSpawnPoints.Length);
            shields.Add((GameObject)Instantiate(shieldObject, shieldSpawnPoints[spawnIndex].transform.position, shieldSpawnPoints[spawnIndex].transform.rotation));
        }
    }
    //needs fixing
    void spawnMeteor() {
        Transform obsCam = GameObject.FindGameObjectsWithTag("Observer")[0].transform.GetChild(0);
       // for (int i = meteor.Count; i < maxMeteorOnPlanet; i++) {
        int spawnIndex = Random.Range(0, meteorSpawnPoints.Length);
            Vector3 dirFromObsToPlanet = planetCenter - obsCam.position;
            Vector3 spawnPos = (obsCam.position - (dirFromObsToPlanet));
            float rand = (Random.value * 400) - 200;
        Vector3 dir = obsCam.transform.right.normalized * (rand);
            spawnPos += dir;
            meteor.Add((GameObject)Instantiate(meteorObject, spawnPos, meteorSpawnPoints[spawnIndex].transform.rotation));
 //   }
}
}