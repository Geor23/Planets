using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

class PlayerConfig : MonoBehaviour {

  public bool isObserver = true; //Initialised as true
  public static PlayerConfig singleton;

  public void Start(){
    DontDestroyOnLoad(transform.gameObject);
    singleton = this;
  }
}