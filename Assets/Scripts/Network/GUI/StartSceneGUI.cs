using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class StartSceneGUI : MonoBehaviour {

  public NetworkManager nm;
  public Text networkAddr;
  public Text nameT;

  void Start(){
    nm = NetworkManager.singleton;
    DontDestroyOnLoad(transform.gameObject);
  }

  public void StartClient(){
    nm.networkAddress = networkAddr.text;
    nm.StartClient().RegisterHandler(MsgType.Connect, SendJoinMessageCallback);
  }

  public void SendJoinMessageCallback(NetworkMessage m){
    SendJoinMessage();
  }

  public void SendJoinMessage(){
    JoinMessage jm = new JoinMessage();
    jm.name = nameT.text;
    nm.client.Send(Msgs.clientJoinMsg, jm);
  }

  public void StartHost(){
    nm = NetworkManager.singleton;
    nm.StartHost();
    SendJoinMessage();
  }

  public void StartDedicatedHost(){
    nm = NetworkManager.singleton;
    nm.StartServer();
	}

}