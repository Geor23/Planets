using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DeathResourceSpawn : NetworkBehaviour {
	public GameObject ResourcePickUp;
	private GameObject objClone;
	//public GameObject[] ResourcePickUp; in case we want to change between different resource pick up types

	void Start () {
		if(isServer){
			//InvokeRepeating("SpawnResourcePickUps", spawnTime, spawnTime);
		} else{
			ClientScene.RegisterPrefab((GameObject)Instantiate(ResourcePickUp, new Vector3(0,0,0), Quaternion.identity));
		}
	}
	
	void SpawnResourcePickUps(Transform trans, int score){
		//set next number of the array randomly
		//int spawnIndex = Random.Range(0, SpawnPoints.Length);
		//objClone = (GameObject)Instantiate(ResourcePickUp, SpawnPoints[spawnIndex].position, SpawnPoints[spawnIndex].rotation);
		//objClone.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(0,randomVelocityMax),Random.Range(0,randomVelocityMax),Random.Range(0,randomVelocityMax));
		//Here we should get the direction to the planet, so we can reduce the explosive launches some get when they roll high in vertical velocity
		//NetworkServer.Spawn(objClone);

		//spawn a resource in the position the player died
		GameObject objClone = (GameObject)Instantiate(ResourcePickUp, trans.position, trans.rotation);
		
		objClone.GetComponent<DeathResourceProperties>().setScore(score);
		Debug.Log("Setting the spawned resource's score to " + score);
		NetworkServer.Spawn(objClone);

	}

}
