using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


class ClientVisibility : MonoBehaviour{

  public bool observerVisible;

  public void Start(){
  	if(!NetworkClient.active) {
  		this.enabled = false;
  		return;
  	}
    bool res = true;
    if(!observerVisible && PlayerConfig.singleton.GetObserver()) {
      res = false;
    }
    gameObject.SetActive(res);
  }
}