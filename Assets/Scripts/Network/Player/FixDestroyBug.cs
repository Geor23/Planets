using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class FixDestroyBug : MonoBehaviour {
	public bool dead = false;
	
	void OnDestroy(){
		//Remove this line to reenable the functionality
		dead = true;
		if(!GetComponent<NetworkIdentity>().isLocalPlayer) return;
		if(!dead){
		    ClientScene.RemovePlayer(0);
		    ClientScene.AddPlayer(NetworkManager.singleton.client.connection, 0);
		}
	}
	void OnApplicationQuit(){
		dead = true;
	}
}