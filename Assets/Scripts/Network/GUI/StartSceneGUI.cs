using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class StartSceneGUI : MonoBehaviour {

  public NetworkManager nm;
  public Text networkAddr;
  public Text name;

  void Start(){
    nm = NetworkManager.singleton;
  }

  public void StartClient(){
    nm.networkAddress = networkAddr.text;
    nm.StartClient();
    SendJoinMessage();
  }

  public void SendJoinMessage(){
    JoinMessage jm = new JoinMessage();
    jm.name = name.text;
    nm.client.Send(Msgs.clientJoinMsg, jm);
  }

  public void StartHost(){
    nm.StartHost();
    SendJoinMessage();
  }

  void StartDedicatedHost(){
    //Something
  }

}