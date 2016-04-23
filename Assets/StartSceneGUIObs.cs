using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class StartSceneGUIObs : MonoBehaviour
{

    public NetworkManager nm;
    public string networkAddr;
    public string nameT;
    public int playerChoice = TeamID.TEAM_OBSERVER;
    public bool keyPressed = false;
    void Start(){
        nm = NetworkManager.singleton;
        DontDestroyOnLoad(transform.gameObject);
        StartClient();
    }

    //Gives the local Network Manager the network address. Request a start client, also adds a handler for the SendJoinMessageCallback
    public void StartClient(){
        nm.networkAddress = networkAddr;
        nm.StartClient().RegisterHandler(MsgType.Connect, SendJoinMessageCallback);
    }

    public void SendJoinMessageCallback(NetworkMessage m)
    {
        SendJoinMessage();
    }

    public void SendJoinMessage()
    {
        JoinMessage jm = new JoinMessage();
        jm.name = nameT;
        jm.team = playerChoice;
        nm.client.Send(Msgs.clientJoinMsg, jm);
    }

    public void StartHost()
    {
        nm = NetworkManager.singleton;
        nm.StartHost();
        SendJoinMessage();
    }

    public void StartDedicatedHost()
    {
        nm = NetworkManager.singleton;
        nm.StartServer();
    }

}
