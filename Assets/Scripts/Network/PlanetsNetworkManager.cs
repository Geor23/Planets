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
    public const int INITIALTIMER = 20;
    public const int ROUNDOVERTIMER = 3;
}

public class RoundScores {
	public List<int> pirateScore = new List<int>();
	public List<int> superCorpScore = new List<int>();
}

public class PlanetsNetworkManager : NetworkManager {
	
	[SerializeField] GameObject player1;
	[SerializeField] GameObject player2;
  	[SerializeField] GameObject observer;

	GameObject chosenCharacter;
    public string round1Scene; //Round 1 name
    public string round2Scene;
    TeamManager teamManager = new TeamManager();
    private float timerRound = Const.INITIALTIMER; //This is the time communicated to clients
	RoundManager roundManager = new RoundManager();
	/*
    Override the virtual default functions to build on existing behaviour 
    
	public override void OnServerConnect(NetworkConnection conn) {

  	}
  	*/

  	private Dictionary<int, PlayerData> dict;
  	public bool hasPickedTeam = false; 
	public bool hasConnected = false;
  	public bool inRound = false;
    public bool timerOn = true;
  	private List<string> roundList;


	public void SceneChange() {
		//Change scene
	}


  	public void Start() {
    	dict = new Dictionary<int, PlayerData>();
    	roundList = new List<string>();
    	roundList.Add("Round1");
    	roundList.Add("RoundOver");
    	roundList.Add("Round2");
    	roundList.Add("RoundOver");
    	roundList.Add("Round3");
    	roundList.Add("GameOver");

  	}
	
    public void Update(){

    	if ( NetworkManager.networkSceneName == "RoundOver" ) {

    		timerRound -= Time.deltaTime;
    		if (timerRound < 0) {
            	ServerChangeScene( roundList[ 2 * ( roundManager.getRoundId() - 1 ) ] );
            	timerRound = Const.INITIALTIMER;
        	}

    	} else if ((roundList.Contains(NetworkManager.networkSceneName)) &&  (roundList.IndexOf(NetworkManager.networkSceneName) != (roundList.Count - 1))) {
           
            timerRound -= Time.deltaTime;
            if ((timerRound < 0) && (timerOn)) {
            	int scoreP = teamManager.getScore(0);
            	int scoreS = teamManager.getScore(1);
            	roundManager.finishRound(scoreP, scoreS);
            	roundManager.changeRound();
            	teamManager.resetScores();
            	if (roundManager.getFinishedState() == 1) {
            		ServerChangeScene( roundList[ 2 * ( roundManager.getRoundId() - 1 ) + 1 ] );
            		timerRound = Const.INITIALTIMER;
            	} else {
            		ServerChangeScene( roundList[ 2 * ( roundManager.getRoundId() - 1 ) - 1 ] );
            		timerRound = Const.ROUNDOVERTIMER;
            	}
        	}

        } else {

        	timerRound = Const.INITIALTIMER;

        }
    }

	// register needed handlers when server starts
  	public override void OnStartServer() {

	    base.OnStartServer();
	    NetworkServer.RegisterHandler(Msgs.clientJoinMsg, OnServerRecieveName);
	    NetworkServer.RegisterHandler(Msgs.clientTeamMsg, OnServerRecieveTeamChoice);
	    NetworkServer.RegisterHandler(Msgs.startGame, OnServerStartGame);
	    NetworkServer.RegisterHandler(Msgs.requestTeamMsg, OnServerRecieveTeamRequest);
	   	NetworkServer.RegisterHandler(Msgs.requestFinalScores, OnServerRecieveFinalScoresRequest);
	    NetworkServer.RegisterHandler(Msgs.clientTeamScore, OnServerReceiveScore);
	    NetworkServer.RegisterHandler(Msgs.requestTeamScores, OnServerRecieveTeamScoresRequest);
        NetworkServer.RegisterHandler(Msgs.requestCurrentTime, OnServerRecieveTimeRequest);

    }


    public void OnServerRecieveFinalScoresRequest(NetworkMessage netMsg){
    	RoundScores sc = roundManager.getFinalScores();
		sendFinalScores(sc);	
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
        TeamScore tl = new TeamScore();
        tl.team = 0;
        tl.score = (int)teamManager.getScore(0);
        NetworkServer.SendToClient(IDFromConn(msg.conn), Msgs.serverTeamScore, tl);
        tl.team = 1;
        tl.score = (int)teamManager.getScore(1);
        NetworkServer.SendToClient(IDFromConn(msg.conn), Msgs.serverTeamScore, tl);

    }

  	public void OnServerReceiveScore(NetworkMessage msg) {

  		// read the message
	  	AddScore sc = msg.ReadMessage<AddScore>();
	  	//Debug.Log("got scoooooreeee");
	  	int id = IDFromConn(msg.conn);
	  	Debug.Log("team: " + dict[id].team);
        // add the score to the correct team
        GameObject obj = sc.obj; //Object interacted with for score
        //obj.GetComponent<ResourceController>().setScore(1);
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

	public void sendFinalScores(RoundScores sc) {

		FinalScores tl = new FinalScores();

		tl.round1P = sc.pirateScore[0];
		tl.round1S = sc.superCorpScore[0];

		if (roundManager.getRoundId() == 3) {
			tl.round2P = sc.pirateScore[1];
			tl.round2S = sc.superCorpScore[1];
		} else {
			tl.round2P = -1;
			tl.round2S = -1;
		}
		
		if (roundManager.getFinishedState() == 1) {
			tl.round3P = sc.pirateScore[2];
			tl.round3S = sc.superCorpScore[2];
		} else {
			tl.round3P = -1;
			tl.round3S = -1;
		}
		
		NetworkServer.SendToAll(Msgs.serverFinalScores, tl);

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
			teamManager.addPlayerToTeam(dict[id].name, dict[id].team);

			sendTeam (dict[id].team);

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
    	roundManager.changeRound();

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
		GameObject player = Instantiate (dict[id].team==0?player1:(dict[id].team==1?player2:observer), teamManager.getSpawnP(dict[id].team), Quaternion.identity) as GameObject;
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

	public Vector3 spawnPoints;

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

	public void resetScore() {
		score = 0;
	}

	public void setSpawnPoint(Vector3 spawn) {
		spawnPoints = spawn;
	}

	public Vector3 getSpawnPoint() {
		return spawnPoints;
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
		teams[0].setSpawnPoint(new Vector3(0,20,0));
		teams[1].setSpawnPoint(new Vector3(0,-20,0));

	}

	public Vector3 getSpawnP(int team){
		if  ((team == 0) || (team == 1)) {
			return teams[team].getSpawnPoint();
		} else {
			Debug.Log("Team is observer");
			return new Vector3(0,20,0);
		}
	}

	public void addScore(int score, int team) {
		if ( team == 0 || team == 1 ) {
			teams[team].addScore(score);
		} else {
			Debug.Log("ERROR[addScore]: You are trying to access a non-existant team ! ");
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


	public void resetScores() {
		teams[0].resetScore();
		teams[1].resetScore();
	}

	
}


[System.Serializable]
public class RoundManager {
	private int roundId = 0;
	private int maxRounds = 3;
	List<Round> rounds = new List<Round>();

	private int hasFinishedState = 0;

	public RoundManager() {

		Round round1 = new Round();
		Round round2 = new Round();
		Round round3 = new Round();

		rounds.Add(round1);
		rounds.Add(round2);
		rounds.Add(round3);


		rounds[0].changeState(Const.NOTSTARTED); // update state of all rounds to not started
		rounds[1].changeState(Const.NOTSTARTED); // update state of all rounds to not started
		rounds[2].changeState(Const.NOTSTARTED); // update state of all rounds to not started

	}

	public RoundScores getFinalScores() {
	
		RoundScores sc = new RoundScores();

		sc.pirateScore.Add(rounds[0].getPiratesFinalScore());
		sc.pirateScore.Add(rounds[1].getPiratesFinalScore());
		sc.pirateScore.Add(rounds[2].getPiratesFinalScore());

		sc.superCorpScore.Add(rounds[0].getSuperCorpFinalScore());
		sc.superCorpScore.Add(rounds[1].getSuperCorpFinalScore());
		sc.superCorpScore.Add(rounds[2].getSuperCorpFinalScore());

		return sc;
	}

	public void changeRound() {

		if (roundId == 0) {
			// game starts now

			Debug.Log("[RoundManager] : Starting game...");
			roundId = 1 ;

			if (rounds[roundId-1].getState() != Const.NOTSTARTED) {
				Debug.LogError("ERROR[RoundManager-ChangeRound]: Cannot start round " + roundId);
			} else {

				rounds[roundId-1].changeState(Const.RUNNING); // update state of new round to running

			}

		}  else if (roundId != maxRounds) {
			// as long as the game is not finishing
			Debug.Log("[RoundManager] : Changing Round...");

			if (rounds[roundId-1].getState() != Const.RUNNING) {

				Debug.LogError("ERROR[RoundManager-ChangeRound]: The round " + roundId + " is not running so cannot be finished");

			} else {

				rounds[roundId-1].changeState(Const.FINISHED); // update state of current round to finished
				roundId ++;

				if (rounds[roundId-1].getState() != Const.NOTSTARTED) {

					Debug.LogError("ERROR[RoundManager-ChangeRound]: Cannot start round " + roundId);

				} else {
					rounds[roundId-1].changeState(Const.RUNNING); // update state of new round to running
				}
			}
		} else {
			// when the game finishes
			Debug.Log("[RoundManager] : Finishing game...");

			if (rounds[roundId-1].getState() != Const.RUNNING) {

				Debug.LogError("ERROR[RoundManager-ChangeRound]: The round " + roundId + " is not running so cannot be finished");

			} else {	
				rounds[roundId-1].changeState(Const.FINISHED); // update state of current round to finished
				hasFinishedState = 1;
				}
			}

	}

	public int getRoundId() {

		return roundId;

	}

	public int getFinishedState() {
		return hasFinishedState;
	}

	public void finishRound(int scoreP, int scoreS) {
		if (rounds[roundId-1].getState() != Const.RUNNING) {
			Debug.LogError("ERROR[RoundManager-ChangeRound]: The round " + roundId + " is not running so cannot be finished");
		}
		else {
			rounds[roundId-1].finishRound(scoreP,scoreS);
		}
	}
}

[System.Serializable]
public class Round {

	private int state ; // 0 = not started, 1 = running, -1 = finished
	private int finalScoreTeamPirates = 0;
	private int finalScoreTeamSuperCorp = 0;

	public void changeState (int newState) {

		state = newState;

	}

	public void finishRound(int scoreP, int scoreS) {
		Debug.Log("Finishing Scores");
		finalScoreTeamPirates = scoreP ;
		finalScoreTeamSuperCorp = scoreS ;
		
	}

	public int getPiratesFinalScore() {

		return finalScoreTeamPirates;

	} 

	public int getSuperCorpFinalScore() {

		return finalScoreTeamSuperCorp;

	} 

	public int getState() {

		return state;

	}



}
