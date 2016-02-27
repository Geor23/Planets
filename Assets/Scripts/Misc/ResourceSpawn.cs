using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ResourceSpawn : NetworkBehaviour {
	public Transform[] SpawnPoints;
	public float spawnTime = 1.5f;
	public GameObject ResourcePickUp;
	private GameObject objClone;
	public int randomVelocityMax;
	public int randomLocationMax;
    public int minScore;
    public int maxScore;
	//public GameObject[] ResourcePickUp; in case we want to change between different resource pick up types

	void Start () {
		if(isServer){
			InvokeRepeating("SpawnResourcePickUps", spawnTime, spawnTime);
		}
	}
	
	void SpawnResourcePickUps(){
		//set next number of the array randomly
		int spawnIndex = Random.Range(0, SpawnPoints.Length);
		objClone = (GameObject)Instantiate(ResourcePickUp, SpawnPoints[spawnIndex].position, SpawnPoints[spawnIndex].rotation);
        //objClone.GetComponent<ResourceProperties>().setScore(Random.Range(minScore, maxScore));
		//objClone.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(0,randomVelocityMax),Random.Range(0,randomVelocityMax),Random.Range(0,randomVelocityMax));
		//Here we should get the direction to the planet, so we can reduce the explosive launches some get when they roll high in vertical velocity
		NetworkServer.Spawn(objClone);
	}

}
