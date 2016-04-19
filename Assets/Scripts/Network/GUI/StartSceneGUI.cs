using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class StartSceneGUI : MonoBehaviour {

  public NetworkManager nm;
  public Text networkAddr;
  public Text nameT;
  public int playerChoice;
  void Start(){
    nm = NetworkManager.singleton;
    playerChoice = TeamID.TEAM_NEUTRAL; //Initialises as a neutral team
    DontDestroyOnLoad(transform.gameObject);
  }

  void Update() { //TODO : REMOVE THIS
    if(Input.GetKey("o")) {
        playerChoice = TeamID.TEAM_OBSERVER;
    }
        if (Input.GetKey("s")){
            StartHost();
        }

        if(Input.GetKey("c")) {
            StartClient();
        }
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
    jm.team = playerChoice;
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