 using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class SimpleTimer : NetworkBehaviour {
  
  public float timerRound = 5;

  void Update(){
    if(isServer){
      timerRound -= Time.deltaTime;
      if ( timerRound < 0){
         NetworkManager.singleton.ServerChangeScene("LobbyScene");
      }
    }
  }

}