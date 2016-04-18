using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

/*
  The full API documentation for NetworkManager can be found here:
    http://docs.unity3d.com/ScriptReference/Networking.NetworkManager.html
*/

static class TeamID {
  public const int TEAM_NEUTRAL = -2;
  public const int TEAM_OBSERVER = -1; 
  public const int TEAM_PIRATES = 0;
  public const int TEAM_SUPERCORP = 1;
};


class PlayerData {

  	public int team;
  	public string name;
  	public int uniqueId;

}

static class Const {
    public const int RUNNING = 1;
    public const int FINISHED = -1;
    public const int NOTSTARTED = 0;
    public const int INITIALTIMER = 50;
    public const int ROUNDOVERTIMER = 15;
}

public class RoundScores {
	public List<int> pirateScore = new List<int>();
	public List<int> superCorpScore = new List<int>();
}

public class PlanetsNetworkManager : NetworkManager {
	
	[SerializeField] GameObject player1;
	[SerializeField] GameObject player2;
  	[SerializeField] GameObject observerSingleScreen;
  	[SerializeField] GameObject observerSplitScreen;

  	public int key = 0;
	GameObject chosenCharacter;
    public string round1Scene; //Round 1 name
    TeamManager teamManager = new TeamManager();
    private float timerRound = Const.INITIALTIMER; //This is the time communicated to clients
	RoundManager roundManager = new RoundManager();

  	/* Client Data */
  	public bool hasPickedTeam = false; 
	public bool hasConnected = false;
  	public bool inRound = false;
    public bool timerOn = true;
  	private List<string> roundList;

  	/* Server Data */
  	private Dictionary<int, PlayerData> dict;
  	public HashSet<NetworkConnection> updateListeners;
  	public HashSet<NetworkConnection> observingListeners;
  	public bool onlyUpdateObservers = false;
  	public bool usingSplitScreen = false;


  	String killFeed;

    //public PlayerSpawnAreas playerSpawnAreas;


    public void SceneChange() {
		//Change scene
	}

	public HashSet<NetworkConnection> getUpdateListeners() {
		if(onlyUpdateObservers) return observingListeners;
		return updateListeners;
	}

	public HashSet<NetworkConnection> getUpdateListeners(bool all) {
		if(onlyUpdateObservers && !all) return observingListeners;
		return updateListeners;
	}

	public bool observerCollisionsOnly(){
		return onlyUpdateObservers;
	}

	public bool isSplitScreen(){
		return usingSplitScreen;
	}


  	public void Start() {
    	dict = new Dictionary<int, PlayerData>();
    	updateListeners = new HashSet<NetworkConnection>();
    	observingListeners = new HashSet<NetworkConnection>();
    	roundList = new List<string>();
    	roundList.Add("Round1");
        roundList.Add("RoundOver");
    	roundList.Add("Round2");
    	roundList.Add("RoundOver");
    	roundList.Add("Round3");
    	roundList.Add("GameOver");
    }

    public void Update(){
    	if(!NetworkServer.active) return;

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
	    NetworkServer.RegisterHandler(Msgs.clientTeamScore, OnServerRecieveScore);
	    NetworkServer.RegisterHandler(Msgs.requestTeamScores, OnServerRecieveTeamScoresRequest);
	    NetworkServer.RegisterHandler(Msgs.requestCurrentTime, OnServerRecieveTimeRequest);
	    NetworkServer.RegisterHandler(Msgs.clientKillFeed, OnServerRecieveKill);
	    NetworkServer.RegisterHandler(Msgs.deathResourceCollision, OnServerRecieveDeathResourceCollision);
	    NetworkServer.RegisterHandler(Msgs.requestName, OnServerSendName);
	    NetworkServer.RegisterHandler(Msgs.killPlayer, OnKillPlayer);
     NetworkServer.RegisterHandler(Msgs.updatePlayer, OnPlayerUpdate);
    }

    public void OnServerSendName(NetworkMessage msg){
    	int id = IDFromConn(msg.conn);
    	Name tl = new Name();
        tl.name = dict[id].name;
        tl.id = dict[id].uniqueId;
        NetworkServer.SendToClient(id, Msgs.serverName, tl);
    }

    public void OnServerRecieveKill(NetworkMessage netMsg){
    	Kill sc = netMsg.ReadMessage<Kill>();
    	addToKillFeed(sc.msg);
    }

    public void addToKillFeed(string killToAdd){
    	killFeed += "\n" + killToAdd;
    	sendKillFeed();
    }

    public void sendKillFeed() {
    	Kill tl = new Kill();
		tl.msg = killFeed;
		foreach (NetworkConnection conn in NetworkServer.connections) {
			int id = IDFromConn(conn);
			if (dict[id].team == TeamID.TEAM_OBSERVER) {
				NetworkServer.SendToClient(id, Msgs.serverKillFeed, tl);
			}
		}
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

    	sendTeam(TeamID.TEAM_PIRATES);
    	sendTeam(TeamID.TEAM_SUPERCORP);

    }

    // when the client requests teams lists, send
	public void OnServerRecieveTeamScoresRequest(NetworkMessage msg) {
        TeamScore tl = new TeamScore();
        tl.team = TeamID.TEAM_PIRATES;
        tl.score = (int)teamManager.getScore(0);
        NetworkServer.SendToClient(IDFromConn(msg.conn), Msgs.serverTeamScore, tl);
        tl.team = 1;
        tl.score = (int)teamManager.getScore(1);
        NetworkServer.SendToClient(IDFromConn(msg.conn), Msgs.serverTeamScore, tl);

    }

  	public void OnServerRecieveScore(NetworkMessage msg) {
  		// read the message
	  	AddScore sc = msg.ReadMessage<AddScore>();
    Debug.LogError("IM DOING SCOREZ HAHAHA");
    int id = sc.obj.GetComponent<NetworkIdentity>().connectionToClient.connectionId;
    Debug.Log("team: " + dict[id].team);
	  	teamManager.addScore(sc.score, dict[id].team);

	  	// send to everyone the updated team score
	  	sendScore(dict[id].team);
        if (sc.score > 0){ //If scoring not dying...
            UpdateLocalScore ls = new UpdateLocalScore();
            ls.score = sc.score;
            NetworkServer.SendToClient(id, Msgs.updateLocalScore, ls);
        }
    }


    public void OnServerRecieveDeathResourceCollision(NetworkMessage msg){
        // read the message
        DeathResource dr = msg.ReadMessage<DeathResource>();
        int id = IDFromConn(msg.conn);
        GameObject resource = dr.drID;
        Debug.Log("team: " + dict[id].team);
        // add the score to the correct team
        int score = int.Parse(resource.GetComponent<Text>().text);
        teamManager.addScore(score, dict[id].team);
        Destroy(resource);
        // send to everyone the updated team score
        sendScore(dict[id].team);
        UpdateLocalScore sc = new UpdateLocalScore();
        sc.score = score;
        NetworkServer.SendToClient(IDFromConn(msg.conn), Msgs.updateLocalScore, sc);
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
		dict[id].team = TeamID.TEAM_OBSERVER;
		dict[id].uniqueId = key;
		key++ ;

	    Debug.Log("Player " + name + " joined the game");
	}


  	public void OnServerRecieveTeamChoice(NetworkMessage msg) {

	    TeamChoice teamChoice = msg.ReadMessage<TeamChoice>();
	    int choice = teamChoice.teamChoice;
	    int id = IDFromConn(msg.conn);

	    // if the player is choosing the team for the first time
		if (dict[id].team == TeamID.TEAM_OBSERVER) {
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

 //Send message to the Player to request the player object to be deleted
 public void OnKillPlayer(NetworkMessage msg) {
        KillPlayer kp = msg.ReadMessage<KillPlayer>();
        int id = kp.obj.GetComponent<NetworkIdentity>().connectionToClient.connectionId;
        NetworkServer.SendToClient(id, Msgs.killPlayerRequestClient, kp);
    }


	// called when a client disconnects
	public override void OnServerDisconnect(NetworkConnection conn) {
		int id = IDFromConn(conn);
		dict.Remove(id);
		Debug.LogError("OnServerDisconnect: Destroying players");
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

        //Change so that the spawn area is from a more random general area chosen from the PlayerSpawnArea script

		/* This is where you can register players with teams, and spawn the player at custom points in the team space */
		//hasConnected = true;
    	int id = IDFromConn(conn);
    	GameObject chosen = dict[id].team==TeamID.TEAM_PIRATES?
    							player1
    						:
    							(dict[id].team==TeamID.TEAM_SUPERCORP?
    								player2
    							:
									(!usingSplitScreen?
										observerSingleScreen
									:
										observerSplitScreen));
		GameObject player = Instantiate (chosen, teamManager.getSpawnP(dict[id].team), Quaternion.identity) as GameObject;
        player.GetComponent<Text>().text = dict[id].name;
        updateListeners.Add(conn);

        //Add to observing listeners if not a player
        if(dict[id].team!=TeamID.TEAM_PIRATES && dict[id].team!=TeamID.TEAM_SUPERCORP) observingListeners.Add(conn);
        NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
	}
	

	//called when a player is removed for a client
	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController) {
		GameObject player = playerController.gameObject;
		if (player != null){
			int id = IDFromConn(conn);
			sendScore(dict[id].team);
			if (playerController.unetView != null){
				NetworkServer.Destroy(player);
				Debug.LogError("OnServerRemovePlayer: Destroying player");
			}
			updateListeners.Remove(conn);
		}
	}

	public override void OnServerError(NetworkConnection conn, int errorCode){
		Debug.LogError("OnServerError with connection " + conn + ", error code: " + errorCode);
	}
	
	/*
    Client functions */
	// called when connected to a server
	public override void OnClientConnect(NetworkConnection conn) {
		hasConnected = true;
		Debug.Log("Client connected!");
	}

    public override void OnServerConnect(NetworkConnection conn){
        base.OnServerConnect(conn);
        string address = conn.address;
        int idValue = BitConverter.ToInt32(IPAddress.Parse(address).GetAddressBytes(), 0);
        //If the player has already connected, set connected to true and update conn value
        if(GameObject.Find("PlayerManager").GetComponent<PlayerManager>().checkIfExists(idValue)) {
            GameObject.Find("PlayerManager").GetComponent<PlayerManager>().setConnected(idValue);
            GameObject.Find("PlayerManager").GetComponent<PlayerManager>().setConnValue(idValue, conn.connectionId);

            PlayerValues pv = new PlayerValues();
            pv.dictId = idValue;
            pv.player = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().getPlayer(idValue);
            foreach (NetworkConnection nc in getUpdateListeners()){
                NetworkServer.SendToClient(nc.connectionId, Msgs.updatePlayer, pv);
            }
                //TODO
        } else {
            Player newPlayer = new Player();

        }
        //string ipAddress = new IPAddress(BitConverter.GetBytes(intAddress)).ToString();
    }

    //Updates Observer player
    public void OnPlayerUpdate(NetworkMessage msg) {
        PlayerValues pv = msg.ReadMessage<PlayerValues>();
        GameObject.Find("PlayerManager").GetComponent<PlayerManager>().updatePlayer(pv.dictId, pv.player);
    }


    // called when disconnected from a server
    public override void OnClientDisconnect(NetworkConnection conn){
		StopClient();
	}
	
	public override void OnClientSceneChanged(NetworkConnection conn){
		//ClientScene.Ready(conn);
	}

	
	// called when a network error occurs
	public override void OnClientError(NetworkConnection conn, int errorCode){
		Debug.LogError("OnClientError with connection " + conn + ", error code: " + errorCode);
	}
	
	// called when told to be not-ready by a server
	//public override void OnClientNotReady(NetworkConnection conn);
}