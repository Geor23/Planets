using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ResourceSpawn : NetworkBehaviour {
	public Transform[] SpawnPoints;
	public float spawnTime = 1.5f;
	public GameObject ResourcePickUp;
	private GameObject objClone;
	//public GameObject[] ResourcePickUp; in case we want to change between different resource pick up types

	void Start () {
		if(isServer){
			InvokeRepeating("SpawnResourcePickUps", spawnTime, spawnTime);
		} else{
			ClientScene.RegisterPrefab((GameObject)Instantiate(ResourcePickUp, new Vector3(0,0,0), Quaternion.identity));
		}
	}
	
	void SpawnResourcePickUps(){
		//set next number of the array randomly
		int spawnIndex = Random.Range(0, SpawnPoints.Length);
		objClone = (GameObject)Instantiate(ResourcePickUp, SpawnPoints[spawnIndex].position, SpawnPoints[spawnIndex].rotation);
		objClone.GetComponent<Rigidbody>().velocity = new Vector3(3,3,0);
		NetworkServer.Spawn(objClone);
	}
}
