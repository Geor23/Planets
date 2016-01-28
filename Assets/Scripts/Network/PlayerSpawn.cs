using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

class PlayerSpawn : MonoBehaviour {
  public void Start(){
    ClientScene.AddPlayer(NetworkManager.singleton.client.connection, 0);
  }
}