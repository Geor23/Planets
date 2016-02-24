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

static class Const {
    public const int RUNNING = 1;
    public const int FINISHED = -1;
    public const int NOTSTARTED = 0;

}

public class PlanetsNetworkManager : NetworkManager {
	
	[SerializeField] GameObject player1;
	[SerializeField] GameObject player2;
  	[SerializeField] GameObject observer;

	GameObject chosenCharacter;
    public string round1Scene; //Round 1 name
    public string round2Scene;
    TeamManager teamManager = new TeamManager();
    private float timerRound = 20; //This is the time communicated to clients
	/*
    Override the virtual default functions to build on existing behaviour 
    
	public override void OnServerConnect(NetworkConnection conn) {

  	}
  	*/

  	private Dictionary<int, PlayerData> dict;
  	public bool hasPickedTeam = false; 
	public bool hasConnected = false;
  	public bool inRound = false;


	public void SceneChange() {
		//Change scene
	}


  	public void Start() {
    	dict = new Dictionary<int, PlayerData>();
  	}
	
    public void Update(){
        if (NetworkManager.networkSceneName== "Round1"){
            timerRound -= Time.deltaTime;
            if (timerRound < 0) {
            	//change Round
            }
        }
        else
        {
            timerRound = 20;
        }
    }
	// register needed handlers when server starts
  	public override void OnStartServer() {

	    base.OnStartServer();
	    NetworkServer.RegisterHandler(Msgs.clientJoinMsg, OnServerRecieveName);
	    NetworkServer.RegisterHandler(Msgs.clientTeamMsg, OnServerRecieveTeamChoice);
	    NetworkServer.RegisterHandler(Msgs.startGame, OnServerStartGame);
	    NetworkServer.RegisterHandler(Msgs.requestTeamMsg, OnServerRecieveTeamRequest);
	    NetworkServer.RegisterHandler(Msgs.clientTeamScore, OnServerReceiveScore);
	    NetworkServer.RegisterHandler(Msgs.requestTeamScores, OnServerRecieveTeamScoresRequest);
        NetworkServer.RegisterHandler(Msgs.requestCurrentTime, OnServerRecieveTimeRequest);

    }

    //This function sends the current in-game time to the client requesting time.
    private void OnServerRecieveTimeRequest(NetworkMessage netMsg){
        TimeMessage timeMessage = new TimeMessage();
        timeMessage.time = timerRound;
        NetworkServer.SendToClient(IDFromConn(netMsg.conn), Msgs.sendCurrentTime, timeMessage);
    }

    private int IDFromConn(NetworkConnection nc) {
 			return nc.connectionId;
    	//return NetworkServer.connections.IndexOf(nc);
  	}

 	// when the client requests teams lists, send
	public void OnServerRecieveTeamRequest(NetworkMessage msg) {

    	sendTeam(0);
    	sendTeam(1);

    }

    // when the client requests teams lists, send
	public void OnServerRecieveTeamScoresRequest(NetworkMessage msg) {

    	sendScore(0);
    	sendScore(1);

    }

  	public void OnServerReceiveScore(NetworkMessage msg) {

  		// read the message
	  	AddScore sc = msg.ReadMessage<AddScore>();
	  	//Debug.Log("got scoooooreeee");
	  	int id = IDFromConn(msg.conn);
	  	Debug.Log("team: " + dict[id].team);
	  	// add the score to the correct team
	  	teamManager.addScore(sc.score, dict[id].team);

	  	// send to everyone the updated team score
	  	sendScore(dict[id].team);
	}

	// send the team list of players to all clients
	public void sendScore(int team) {

		TeamScore tl = new TeamScore();
		tl.team = (int) team;
		tl.score = (int) teamManager.getScore(team);
		NetworkServer.SendToAll(Msgs.serverTeamScore, tl);

	}

	public void OnServerRecieveName(NetworkMessage msg) {
	    
	    JoinMessage joinMsg = msg.ReadMessage<JoinMessage>();
	    string name = joinMsg.name;
	    int id = IDFromConn(msg.conn);
	    dict.Add(id, new PlayerData());
	    dict[id].name = name;
		dict[id].team = -1;

	    Debug.Log("Player " + name + " joined the game");
	}


  	public void OnServerRecieveTeamChoice(NetworkMessage msg) {

	    TeamChoice teamChoice = msg.ReadMessage<TeamChoice>();
	    int choice = teamChoice.teamChoice;
	    int id = IDFromConn(msg.conn);

	    // if the player is choosing the team for the first time
		if (dict[id].team == -1) {
			// update the team and send updated list to all clients
			dict[id].team = choice;	

		} else if (dict[id].team != choice) {	// if the player has switched teams
			// delete player from old list and send updated list to all clients
			teamManager.deletePlayer(dict[id].name, dict[id].team);
			sendTeam (dict[id].team);

			// add player to new team and send updated list to clients
			dict[id].team = choice;
			teamManager.addPlayerToTeam(dict[id].name, dict[id].team);
			sendTeam (dict[id].team);
		}

	    Debug.Log(dict[id].name + " chose team " + choice.ToString());
	  }

  	// send the team list of players to all clients
	public void sendTeam(int team) {
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


	public void OnServerStartGame(NetworkMessage msg) {

    	ServerChangeScene(round1Scene);

	}


	// called when a client disconnects
	public override void OnServerDisconnect(NetworkConnection conn) {
		int id = IDFromConn(conn);
		dict.Remove(id);
		NetworkServer.DestroyPlayersForConnection(conn);

	}
	

	// called when a client is ready
	public override void OnServerReady(NetworkConnection conn) {
		NetworkServer.SetClientReady(conn);
		ClientScene.RegisterPrefab(player1);
		ClientScene.RegisterPrefab(player2);
	}
	

	// called when a new player is added for a client
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		/* This is where you can register players with teams, and spawn the player at custom points in the team space */
		//hasConnected = true;
    	int id = IDFromConn(conn);
		GameObject player = Instantiate (dict[id].team==0?player1:(dict[id].team==1?player2:observer), GetStartPosition ().position, Quaternion.identity) as GameObject;
        NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
		
	}
	

	//called when a player is removed for a client
	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController) {
		GameObject player = playerController.gameObject;
		if (player != null){
			int id = IDFromConn(conn);
			int sc = player.GetComponent<UnityStandardAssets.CrossPlatformInput.PlayerControllerMobile>().getScore();
			teamManager.removeScore(sc, dict[id].team);
			Debug.Log("Player died. Removing "+sc+ " points from team "+ dict[id].team);
			sendScore(dict[id].team);
			if (playerController.unetView != null)
				NetworkServer.Destroy(player);
		}
	}


	// called when a network error occurs
	// public override void OnServerError(NetworkConnection conn, int errorCode);
	
	/*
    Client functions */
	// called when connected to a server
	public override void OnClientConnect(NetworkConnection conn) {
		hasConnected = true;
		Debug.Log("Client connected!");
	}
	
	// called when disconnected from a server
	public override void OnClientDisconnect(NetworkConnection conn){
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
public class Team {

	public List<string> players = new List<string>() ;
	public int score = 0 ;

	public void addScore( int scoreToAdd ) {
		score += scoreToAdd ;
	}

	public void removeScore( int scoreToRemove ) {
		score -= scoreToRemove ;
	}

	public int getScore() {
		return score ;
	}

	public List<string> getPlayers () {
		return players ;
	}

	public void addPlayer (string playerName) {
		players.Add(playerName) ;
	}

	public void removePlayer (string playerName) {
		players.Remove(playerName) ;
	}

}



[System.Serializable]
public class TeamManager {
	public List<Team> teams;


	public TeamManager() {
		teams = new List<Team>() ;
		Team teamPirates = new Team() ;
		Team teamSuperCorp = new Team() ;
		teams.Add(teamPirates) ;	
		teams.Add(teamSuperCorp) ;	

	}

	public void addScore(int score, int team) {
		if ( team == 0 || team == 1 ) {
			teams[team].addScore(score);
		} else {
			Debug.LogError("ERROR[addScore]: You are trying to access a non-existant team ! ");
		}

	}

	public void removeScore(int score, int team) {
		if ( team == 0 || team == 1 ) {
			teams[team].removeScore(score);
		} else {
			Debug.LogError("ERROR[removeScore]: You are trying to access a non-existant team ! ");
		}

	}
	
	public void deletePlayer (String playerName, int team) {

		if (team == 0 || team == 1 ) {

			teams[team].removePlayer(playerName);

		} else {

			Debug.LogError("ERROR[deletePlayer]: You are trying to access a non-existant team ! ");
		}

	}
	
	public void addPlayerToTeam( string playerName, int team) {

		if (team == 0 || team == 1) {

			teams[team].addPlayer(playerName) ;

		} else {

			Debug.LogError("ERROR[addPlayerToTeam]: You are trying to access a non-existant team ! ");

		}

	}


	public List<string> getListTeam (int team) {

		if (team == 0 || team == 1) {

			return teams[team].getPlayers() ;

		} else {

			Debug.LogError("ERROR[getListTeam]: You are trying to access a non-existant team ! ");
			return null;

		}

	}


	public int getScore(int team) {

		if (team == 0 || team == 1) {

			return teams[team].getScore() ;

		} else {

			Debug.LogError("ERROR[getScore]: You are trying to access a non-existant team ! ");
			return 0;

		}

	}

	
}


[System.Serializable]
public class RoundManager {
	private int roundId;
	private int maxRounds = 2;
	List<Round> rounds ;

	public RoundManager() {

		Round round1 = new Round();
		Round round2 = new Round();

		roundId = 0;
		rounds.Add(round1);
		rounds.Add(round2);

		rounds[0].changeState(Const.NOTSTARTED); // update state of all rounds to not started
		rounds[1].changeState(Const.NOTSTARTED); // update state of all rounds to not started

	}

	public void changeRound() {

		// reset timer

		if (roundId == 0) {
			// game starts now
			Debug.Log("[RoundManager] : Starting game...");
			roundId = 1 ;
			rounds[roundId-1].changeState(Const.RUNNING); // update state of new round to running

		}  else if (roundId != maxRounds) {
			// as long as the game is not finishing
			rounds[roundId-1].changeState(Const.FINISHED); // update state of current round to finished
			roundId ++;
			rounds[roundId-1].changeState(Const.RUNNING); // update state of new round to running

		} else {
			// when the game finishes
			rounds[roundId-1].changeState(Const.FINISHED); // update state of current round to finished
			// game over
			// call game manager to get final scores

		}
	}

	public int getRoundId() {

		return roundId;

	}

}

[System.Serializable]
public class Round {

	private int state ; // 0 = not started, 1 = running, -1 = finished
	private int finalScoreTeamPirates = 0;
	private int finalScoreTeamSuperCorp = 0;

	public void changeState (int newState) {
		state = newstate;
	}

	public void finishRound(int scoreP, int scoreS) {

		finalScoreTeamPirates = scoreP ;
		finalScoreTeamSuperCorp = scoreS ;
		
	}

	public int getPiratesFinalScore() {
		return finalScoreTeamPirates;
	} 

	public int getSuperCorpFinalScore() {
		return finalScoreTeamSuperCorp;
	} 

}
