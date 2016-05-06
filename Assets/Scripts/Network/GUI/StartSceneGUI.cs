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

    void Update(){ //TODO : REMOVE THIS, REPLACE WITH OBSERVER SCENE
        if (Input.GetKeyDown("o") && Input.GetKeyDown("p")){
            playerChoice = TeamID.TEAM_OBSERVER;
            PlayerConfig.singleton.SetTeam(TeamID.TEAM_OBSERVER);
            Debug.LogError("Now an observer");
        }

        if ((Input.GetKeyDown("g") && Input.GetKeyDown("h"))){
          GameObject.Find("FadeTexture").GetComponent<SceneFadeInOut>().EndScene();
          nm = NetworkManager.singleton;
          nm.StartHost();
          SendJoinMessage();
        }
    }

    //Gives the local Network Manager the network address. Request a start client, also adds a handler for the SendJoinMessageCallback
  public void StartClient(){
    GameObject.Find("FadeTexture").GetComponent<SceneFadeInOut>().EndScene();
    nm.networkAddress = networkAddr.text;
    nm.StartClient().RegisterHandler(MsgType.Connect, SendJoinMessageCallback);
  }

  public void SendJoinMessageCallback(NetworkMessage m){
    SendJoinMessage();
  }

  public void SendJoinMessage(){
    JoinMessage jm = new JoinMessage();
        /*if (nameT.text.Length < 1)
        {
            //request new name input
        }*/
    jm.name = nameT.text; // Add check so that invalid names are avoided
    jm.team = playerChoice;
    nm.client.Send(Msgs.clientJoinMsg, jm);
  }

  public void BecomeObserver() {
    playerChoice = TeamID.TEAM_OBSERVER;
    PlayerConfig.singleton.SetTeam(TeamID.TEAM_OBSERVER);
    Debug.LogError("Now an observer");
  }

  // public void StartHost(){
  //   GameObject.Find("FadeTexture").GetComponent<SceneFadeInOut>().EndScene();
  //   nm = NetworkManager.singleton;
  //   nm.StartHost();
  //   SendJoinMessage();
  // }

  public void StartDedicatedHost(){
    nm = NetworkManager.singleton;
    nm.StartServer();
	}

}