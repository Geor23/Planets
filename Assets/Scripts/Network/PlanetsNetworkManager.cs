using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

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
	
	TeamManager teamManager = new TeamManager();
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
    NetworkServer.RegisterHandler(Msgs.requestTeamMsg, OnServerRecieveTeamRequest);
  }


  private int IDFromConn(NetworkConnection nc){
    return NetworkServer.connections.IndexOf(nc);
  }

  public void OnServerRecieveTeamRequest(NetworkMessage msg){
    sendTeam(0);
        Debug.Log("I'm a potato");
    sendTeam(1);
    }

  public void OnServerRecieveName(NetworkMessage msg){
    Debug.Log("join!");
    JoinMessage joinMsg = msg.ReadMessage<JoinMessage>();
    string name = joinMsg.name;
    int id = IDFromConn(msg.conn);
    dict.Add(id, new PlayerData());
    dict[id].name = name;
	dict[id].team = -1;

    Debug.Log("Player " + name + " joined the game");
  }


  public void OnServerRecieveTeamChoice(NetworkMessage msg){
    Debug.Log("Team!");
    TeamChoice teamChoice = msg.ReadMessage<TeamChoice>();
    int choice = teamChoice.teamChoice;
    int id = IDFromConn(msg.conn);

	if (dict [id].team == -1) {

			dict[id].team = choice;
			
			teamManager.addPlayerToTeam(dict[id].name, dict[id].team);
			
			sendTeam (dict[id].team);

	} else if (dict [id].team != choice) {
			teamManager.deletePlayer(dict[id].name, dict[id].team);
			sendTeam (dict[id].team);
			dict[id].team = choice;
			teamManager.addPlayerToTeam(dict[id].name, dict[id].team);
			
			sendTeam (dict[id].team);


	}

    Debug.Log(dict[id].name + " chose team " + choice.ToString());
  }

	public void sendTeam(int team){
		string display = "";
		TeamList tl = new TeamList();

		foreach(string player in teamManager.getListTeam(team) ) {
            Debug.Log(player.ToString());
			display = display.ToString () + player.ToString() + "\n";
		}
		
		tl.team = (int) team;
		tl.teamList = (string) display;
		NetworkServer.SendToAll(Msgs.serverTeamMsg, tl);
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


[System.Serializable]
public class TeamManager {
	public List<string> playersTeamA = new List<string>() ;
	public List<string> playersTeamB = new List<string>() ;
	public int scoreTeamA = 0;
	public int scoreTeamB = 0;
	
	public void addScoreToTeamA(int score){
		scoreTeamA += score;
	}
	
	public void addScoreToTeamB(int score){
		scoreTeamB += score;
	}
	
	public void deletePlayer (String playerName, int team) {
		if (team == 0) {
			playersTeamA.Remove(playerName);
		} else if (team == 1) {
			playersTeamB.Remove(playerName);
		} else {
			//error
		}
	}
	
	public void addPlayerToTeam( string playerName, int team) {
		if (team == 0) {
			playersTeamA.Add(playerName);
		} else if (team == 1) {
			playersTeamB.Add(playerName);
		} else {
			//error
		}
	}

	public List<string> getListTeam (int team) {
		if (team == 0) {
			return playersTeamA;
		} else if (team == 1) {
			return playersTeamB;
		} else {
			return null;
		}
	}

	public int getScoreTeamA() {
		return scoreTeamA;
	}
	
	public int getScoreTeamB() {
		return scoreTeamB;
	}
}