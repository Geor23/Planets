using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

/*
  The full API documentation for NetworkManager can be found here:
    http://docs.unity3d.com/ScriptReference/Networking.NetworkManager.html
*/

class PlayerData {
  public int team;
  public string name;
}

public class PlanetsNetworkManager : NetworkManager {
	
	[SerializeField] GameObject player1;
	[SerializeField] GameObject player2;
  [SerializeField] GameObject observer;

	GameObject chosenCharacter; 
	
	/*
    Override the virtual default functions to build on existing behaviour 
    
	public override void OnServerConnect(NetworkConnection conn){

  }
  */

  private Dictionary<int, PlayerData> dict;
  public bool hasPickedTeam = false; 
	public bool hasConnected = false;
  public bool inRound = false;
  int timerRound = 180;



 // void Update(){
 //  if(inRound){
 //     timerRound -= Time.deltaTime;
 //     if ( timerRound < 0){
 //         SceneChange();
 //     }
 //   }
 // }

public void SceneChange(){
//Change scene
}


  public void Start(){
    dict = new Dictionary<int, PlayerData>();
  }
	
  public override void OnStartServer(){
    base.OnStartServer();
    NetworkServer.RegisterHandler(Msgs.clientJoinMsg, OnServerRecieveName);
    NetworkServer.RegisterHandler(Msgs.clientTeamMsg, OnServerRecieveTeamChoice);
    NetworkServer.RegisterHandler(Msgs.startGame, OnServerStartGame);
  }

  private int IDFromConn(NetworkConnection nc){
    return NetworkServer.connections.IndexOf(nc);
  }

  public void OnServerRecieveName(NetworkMessage msg){
    JoinMessage joinMsg = msg.ReadMessage<JoinMessage>();
    string name = joinMsg.name;
    int id = IDFromConn(msg.conn);
    dict.Add(id, new PlayerData());
    dict[id].name = name;

    Debug.Log("Player " + name + " joined the game");
  }

  public void OnServerRecieveTeamChoice(NetworkMessage msg){
    TeamChoice teamChoice = msg.ReadMessage<TeamChoice>();
    int choice = teamChoice.teamChoice;
    int id = IDFromConn(msg.conn);
    dict[id].team = choice;

    Debug.Log(dict[id].name + " chose team " + choice.ToString());
  }

  public void OnServerStartGame(NetworkMessage msg){
    ServerChangeScene("RunningScene");
  }

	// called when a client disconnects
	public override void OnServerDisconnect(NetworkConnection conn)
	{
		NetworkServer.DestroyPlayersForConnection(conn);
	}
	
	// called when a client is ready
	public override void OnServerReady(NetworkConnection conn)
	{
		NetworkServer.SetClientReady(conn);
		ClientScene.RegisterPrefab(player1);
		ClientScene.RegisterPrefab(player2);

		
	}
	
	// called when a new player is added for a client
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		/* This is where you can register players with teams, and spawn the player at custom points in the team space */
		//hasConnected = true;
    int id = IDFromConn(conn);
		GameObject player = Instantiate (dict[id].team==0?player1:(dict[id].team==1?player2:observer), GetStartPosition ().position, Quaternion.identity) as GameObject;
		//		GameObject player = Instantiate (playerPrefab, GetStartPosition ().position, Quaternion.identity) as GameObject;
		NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
		
	}
	
	//called when a player is removed for a client
	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController)
	{
		GameObject player = playerController.gameObject;
		if (player != null)
		{
			if (playerController.unetView != null)
				NetworkServer.Destroy(player);
		}
	}


	// called when a network error occurs
	// public override void OnServerError(NetworkConnection conn, int errorCode);
	
	/*
    Client functions */
	// called when connected to a server
	public override void OnClientConnect(NetworkConnection conn)
	{
		hasConnected = true;
		Debug.Log("Client connected!");
	}
	
	// called when disconnected from a server
	public override void OnClientDisconnect(NetworkConnection conn)
	{
		StopClient();
	}
	
	public override void OnClientSceneChanged(NetworkConnection conn){
		//ClientScene.Ready(conn);
	}
	
	// called when a network error occurs
	//public override void OnClientError(NetworkConnection conn, int errorCode);
	
	// called when told to be not-ready by a server
	//public override void OnClientNotReady(NetworkConnection conn);
}