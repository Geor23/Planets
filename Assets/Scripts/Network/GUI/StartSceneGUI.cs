using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class StartSceneGUI : MonoBehaviour {

  public NetworkManager nm;
  public Text networkAddr;
  public Text nameT;
  public int playerChoice = TeamID.TEAM_NEUTRAL;
  public bool keyPressed = false;
  void Start(){
    nm = NetworkManager.singleton;
    DontDestroyOnLoad(transform.gameObject);
  }

    void Update(){ //TODO : REMOVE THIS
        if (Input.GetKeyDown("o") && Input.GetKeyDown("p")){
            playerChoice = TeamID.TEAM_OBSERVER;
            Debug.LogError("Now an observer");
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