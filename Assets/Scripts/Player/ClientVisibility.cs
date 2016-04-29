using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


class ClientVisibility : MonoBehaviour{

  public bool observerVisible = false;
  public bool playerVisible = false;

  public void Start(){
  	if(!NetworkClient.active) {
  		this.enabled = false;
  		return;
  	}
    bool res = false;
    if(observerVisible && PlayerConfig.singleton.GetObserver()) {
      res = true;
    }
    if(playerVisible && !PlayerConfig.singleton.GetObserver()){
      res = true;
    }
    gameObject.SetActive(res);
  }
}