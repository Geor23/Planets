using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DeathResourceSpawn : NetworkBehaviour {
	public GameObject ResourcePickUp;
	private GameObject objClone;
	
	void SpawnResourcePickUps(Transform trans, int score){
		//spawn a resource in the position the player died
		GameObject objClone = (GameObject)Instantiate(ResourcePickUp, trans.position, trans.rotation);
		
		objClone.GetComponent<DeathResourceProperties>().setScore(score);
		Debug.Log("Setting the spawned resource's score to " + score);
		NetworkServer.Spawn(objClone);
	}

}
