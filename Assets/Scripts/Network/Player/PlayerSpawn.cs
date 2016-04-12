using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

class PlayerSpawn : MonoBehaviour {

    public Camera viewCamera;
    public void Start(){
  	  if(!NetworkClient.active) {
  		this.enabled = false;
  		return;
  	}
  		Invoke("SpawnPlayer", 5);
        viewCamera.enabled = true;
    }

    public void SpawnPlayer(){
        viewCamera.enabled = false;
        ClientScene.AddPlayer(NetworkManager.singleton.client.connection, 0);
    }

}