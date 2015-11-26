using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class StartSceneGUI : MonoBehaviour {

  public NetworkManager nm;
  public Text networkAddr;

  void Start(){
    //Something
  }

  public void StartClient(){
    nm.networkAddress = networkAddr.text;
    nm.StartClient();
  }

  public void StartHost(){
    nm.StartHost();
  }

  void StartDedicatedHost(){
    //Something
  }

}