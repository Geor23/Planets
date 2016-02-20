using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

class PlayerConfig : MonoBehaviour {

  private bool isObserver = true; //Initialised as true
  public static PlayerConfig singleton;

  private void Start(){
    DontDestroyOnLoad(transform.gameObject);
    singleton = this;
  }
  
  public void SetObserver(bool observe) {
        isObserver = observe;
    }

  public bool GetObserver(){
        return isObserver;
    }
}