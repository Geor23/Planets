using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

class PlayerConfig : MonoBehaviour {

  public int team = -1; //Initialised as observer - -1: obs, 0: pirate, 1:supercorp, -2 = neutral (unpicked)
  public static PlayerConfig singleton;

  void OnGUI(){
  }

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