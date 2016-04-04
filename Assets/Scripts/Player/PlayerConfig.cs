using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

class PlayerConfig : MonoBehaviour {

  private int team = -1; //Initialised as observer - -1: obs, 0: pirate, 1:supercorp
  public static PlayerConfig singleton;

  private void Start(){
    DontDestroyOnLoad(transform.gameObject);
    singleton = this;
  }
  
  public void SetTeam(int steam) {
        team = steam;
  }

  public bool GetObserver(){
        return (team==-1);
  }

  public int getTeam() {
    return team;
  }
}