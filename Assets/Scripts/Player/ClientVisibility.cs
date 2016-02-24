using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


class ClientVisibility : MonoBehaviour{

  public bool observerVisible;
  //public bool playerVisible;

  public void Start(){
    bool res = true;
    if(!observerVisible && PlayerConfig.singleton.GetObserver()) {
      res = false;
    }
        Debug.Log(PlayerConfig.singleton.GetObserver());
    gameObject.SetActive(res);
  }
}