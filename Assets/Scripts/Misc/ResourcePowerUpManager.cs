﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class ResourcePowerUpManager : MonoBehaviour
{
    public GameObject resourceObject;
    public GameObject shieldObject;
    public GameObject fasterFireObject;
    public GameObject doubleScoreObject;

    private int minScore = 1;
    private int maxScore = 11;
    private int score = 1;
    private NetworkManager nm = NetworkManager.singleton;

    //public GameObject[] resources;
    public List<GameObject> resources;
    //public GameObject[] shields;
    public List<GameObject> shields;
    //public GameObject[] doubleScore;
    public List<GameObject> doubleScore;
    //public GameObject[] fasterFire;
    public List<GameObject> fasterFire;

    public GameObject[] resourceSpawnPoints;
    public GameObject[] shieldSpawnPoints;
    public GameObject[] doubleScoreSpawnPoints;
    public GameObject[] fasterFireSpawnPoints;


    public int maxResource = 5;
    public int minResource = 1;
    //public int currentResource = 0;
    public int maxShield = 2;
    public int minShield = 1;
    //public int currentShield = 0;
    public int maxDoubleScore = 2;
    public int minDoubleScore = 1;
    //public int currentDoubleScore = 0;
    public int maxFasterFire = 2;
    public int minFasterFire = 1;
    //public int currentFasterFire = 0;

    public float resourceSpawnTime = 1.5f;
    public float powerUpSpawnTime = 10.5f;


    // Use this for initialization
    void Start() {

        //Spawn Resources and Power Ups and initialize lists 
        InvokeRepeating("spawnPlanetResources",0.0f, resourceSpawnTime);
        InvokeRepeating("spawnFasterFire", 0.0f, powerUpSpawnTime);
        InvokeRepeating("spawnDoubleScore", 0.0f, powerUpSpawnTime);
        InvokeRepeating("spawnShield", 0.0f, powerUpSpawnTime);

        //InvokeRepeating("SpawnResource", resourceSpawnTime, resourceSpawnTime);
        InvokeRepeating("UpdateValue", 1, 1);
        ResourceRescale();
    }

    public int getScore()
    {
        return score;
    }

    public void setScore(int scoreToSet)
    {
        score = scoreToSet;
        ResourceRescale();
    }

    void UpdateValue()
    {
        score += 1;
        if (score > 2 * maxScore) { score = 2 * maxScore; }
        ResourceRescale();
    }

    void ResourceRescale()
    {
        float scale;
        float tmp = (maxScore - minScore) / 5;
        if (score == 1)
        {
            scale = 0.2f;
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
        else if (score - minScore < tmp * 2)
        {
            scale = 0.4f;
            gameObject.GetComponent<Renderer>().enabled = true;
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }
        else if (score - minScore < tmp * 3) { scale = 0.6f; }
        else if (score - minScore < tmp * 4) { scale = 0.8f; }
        else if (score - minScore < tmp * 5) { scale = 1.0f; }
        else if (score - minScore < tmp * 6) { scale = 1.2f; }
        else if (score - minScore < tmp * 7) { scale = 1.4f; }
        else if (score - minScore < tmp * 8) { scale = 1.6f; }
        else if (score - minScore < tmp * 9) { scale = 1.8f; }
        else { scale = 1.8f; }
        transform.localScale = new Vector3(scale, scale, scale);
    }

    //Modify to recognize collisions for all kinds of pick ups
    void OnTriggerEnter(Collider col)
    {
        if (gameObject.CompareTag("StaticResource"))
        {
            if (col.gameObject.CompareTag("PlayerPirate") || col.gameObject.CompareTag("PlayerSuperCorp"))
            {
                NetworkIdentity nIdentity = col.gameObject.GetComponent<NetworkIdentity>();
                if (PlayerConfig.singleton.GetObserver())
                {
                    Debug.Log("Collided with a player");
                    //DoubleScore Powerup
                    if (col.gameObject.GetComponent<PlayerControllerMobile>().doubleScore == true)
                    {
                        Debug.Log("DOUBLED SCORE");
                        score *= 2;
                    }
                    //col.gameObject.GetComponent<PlayerControllerMobile>().SetScoreTextNew(score); //This needs to be done
                    //int id = col.gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId;
                    AddScore sc = new AddScore();
                    sc.team = 0;
                    sc.score = score;
                    sc.obj = col.gameObject;
                    nm.client.Send(Msgs.clientTeamScore, sc);
                }

                setScore(1);
            }
        }
      /*  
      else if (gameObject.CompareTag("Shield")) { }

        else if (gameObject.CompareTag("FasterFire")) { }

        else if (gameObject.CompareTag("DoubleScore")) { }
        */
    }


    // Update is called once per frame
    void Update() {}

    public void resourceCollision(GameObject gameObject) {
        switch (gameObject.tag) {
            case "StaticResource":
                Destroy(gameObject);
                resources.Remove(gameObject);
                break;
            case "Shield":
                Destroy(gameObject);
                shields.Remove(gameObject);
                break;
            case "DoubleScore":
                Destroy(gameObject);
                doubleScore.Remove(gameObject);
                break;
            case "FasterFire":
                Destroy(gameObject);
                fasterFire.Remove(gameObject);
                break;
            }
        }

    void spawnResource() {
        for (int i = resources.Count; i < maxResource; i++) {
            int spawnIndex = Random.Range(0, maxResource);
            resources.Add((GameObject) Instantiate(resourceObject, resourceSpawnPoints[spawnIndex].transform.position, resourceSpawnPoints[spawnIndex].transform.rotation));
        }
    }

    void spawnFasterFire()
    {
        for (int i = fasterFire.Count; i < maxFasterFire; i++)
        {
            int spawnIndex = Random.Range(0, maxFasterFire);
            fasterFire.Add((GameObject)Instantiate(fasterFireObject, fasterFireSpawnPoints[spawnIndex].transform.position, fasterFireSpawnPoints[spawnIndex].transform.rotation));
        }
    }

    void spawnDoubleScore ()
    {
        for (int i = doubleScore.Count; i < maxDoubleScore; i++)
        {
            int spawnIndex = Random.Range(0, maxDoubleScore);
            doubleScore.Add((GameObject)Instantiate(doubleScoreObject, doubleScoreSpawnPoints[spawnIndex].transform.position, doubleScoreSpawnPoints[spawnIndex].transform.rotation));
        }
    }

    void spawnShield ()
    {
        for (int i = shields.Count; i < maxShield; i++)
        {
            int spawnIndex = Random.Range(0, maxShield);
            shields.Add((GameObject)Instantiate(shieldObject, shieldSpawnPoints[spawnIndex].transform.position, shieldSpawnPoints[spawnIndex].transform.rotation));
        }
    }
}
