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
    TeamManager teamManager = new TeamManager();
	RoundManager roundManager = new RoundManager();
  	
    private List<string> roundList;
  	public HashSet<NetworkConnection> updateListeners;
  	public HashSet<NetworkConnection> observingListeners;

    public bool allowSharedIPs = false;
  	public bool onlyUpdateObservers = false;
  	public bool usingSplitScreen = false;
    public int key = 0;
    public string round1Scene; //Round 1 name
    private float timerRound = Const.INITIALTIMER; //This is the time communicated to clients
    public bool timerOn = true;

    public PlayerManager pm;
    public PersonalPlayerInfo ppi; //Singleton

  	public void Start() {
    	updateListeners = new HashSet<NetworkConnection>();
    	observingListeners = new HashSet<NetworkConnection>();
        ppi = PersonalPlayerInfo.singleton;

        //TOCHANGE
    	roundList = new List<string>();
    	roundList.Add("Round4");
        roundList.Add("RoundOver");
    	roundList.Add("Round2");
    	roundList.Add("RoundOver");
    	roundList.Add("Round3");
    	roundList.Add("GameOver");
        pm = new PlayerManager();
    }


    public override void OnServerConnect(NetworkConnection conn){
        base.OnServerConnect(conn);
    }

    // called when a client disconnects
    public override void OnServerDisconnect(NetworkConnection conn){

        string address = pm.findPlayerWithConnID(conn.connectionId);
        int idVal = ipToId(address, conn.connectionId);
        if (networkSceneName == "LobbyScene"){
            if (pm.getTeam(idVal) != TeamID.TEAM_NEUTRAL && pm.getTeam(idVal) != TeamID.TEAM_OBSERVER){
                // delete player from old list and send updated list to all clients
                teamManager.deletePlayer(pm.getName(idVal), pm.getTeam(idVal));
                sendTeam(pm.getTeam(idVal));
            }
        }
        Debug.LogError("OnServerDisconnect: Destroying players");
        pm.setDisconnected(idVal); //Indicates player disconnected
        //KEEP ADDRESS SAME. THE IDENTIFICATION WILL CHANGE THOUGH
        NetworkServer.DestroyPlayersForConnection(conn);
        //NEW: USING PLAYER STRUCTURES + MEMORY
    }


    // called when a client is ready
    public override void OnServerReady(NetworkConnection conn){
        NetworkServer.SetClientReady(conn);
        ClientScene.RegisterPrefab(player1);
        ClientScene.RegisterPrefab(player2);
    }


    public override void OnServerError(NetworkConnection conn, int errorCode){
        Debug.LogError("OnServerError with connection " + conn + ", error code: " + errorCode);
    }


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

    public void Update(){
    	if(!NetworkServer.active) return;

        // TO CHANGE

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

  	public override void OnStartServer() {
	    base.OnStartServer();
	    NetworkServer.RegisterHandler(Msgs.clientJoinMsg, OnServerRecieveName);
	    NetworkServer.RegisterHandler(Msgs.clientTeamMsg, OnServerRecieveTeamChoice);
        NetworkServer.RegisterHandler(Msgs.requestTeamMsg, OnServerRecieveTeamRequest);
	    NetworkServer.RegisterHandler(Msgs.startGame, OnServerStartGame);
	    NetworkServer.RegisterHandler(Msgs.requestCurrentTime, OnServerRecieveTimeRequest);
    }

    public int ipToId(string address, int connId){
        if (address != "localClient") {
            int idValue;
            idValue = connId*(IPAddress.Parse(address).GetAddressBytes()[15] + 1);
            return idValue;
        } 
        return -1;
    }   
    
    // called when a new player is added for a client
    // next two functions are important
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        //Change so that the spawn area is from a more random general area chosen from the PlayerSpawnArea script

        /* This is where you can register players with teams, and spawn the player at custom points in the team space */
        int idVal = ipToId(conn.address, conn.connectionId);;
        if (pm.getTeam(idVal) != TeamID.TEAM_NEUTRAL){
            GameObject chosen = playerObjectType(idVal);
            GameObject player = Instantiate(chosen, teamManager.getSpawnP(pm.getTeam(idVal)), Quaternion.identity) as GameObject;
            if (pm.getTeam(idVal) != TeamID.TEAM_OBSERVER){
                //Player playa = pm.getPlayer(idValue);
                //chosen.GetComponent<PlayerDetails>().setPlayerDetails(idValue,playa);
                player.GetComponent<PlayerControllerMobile>().dictId = idVal;
            }
            updateListeners.Add(conn);

            //Add to observing listeners if not a player
            //if (pm.getTeam(idVal) == TeamID.TEAM_OBSERVER) observingListeners.Add(conn);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }

    public GameObject playerObjectType(int idVal){
        return pm.getTeam(idVal) == TeamID.TEAM_PIRATES ?
                    player1
                :
                    (pm.getTeam(idVal) == TeamID.TEAM_SUPERCORP ?
                        player2
                    :
                        (!usingSplitScreen ?
                            observerSingleScreen
                        :
                            observerSplitScreen));
    }

    public void OnServerRecieveName(NetworkMessage msg) {  
        JoinMessage joinMsg = msg.ReadMessage<JoinMessage>();
        string name = joinMsg.name;
        int teamChoice = joinMsg.team;
        string address = msg.conn.address;
        int idValue = ipToId(address, msg.conn.connectionId);
        //If the player has already connected, set connected to true and update conn value
        int idValOld = pm.findPlayerWithIP(address); //Finds a player with the IP. Will return the latest player made and their identity if multiple exist
        if (((idValOld != -10)&&(!allowSharedIPs)) || ((pm.checkIfExists(idValue))&&(allowSharedIPs))){ //-10 means not found. If it was found, adjust old player
            Debug.LogError("Player " + idValOld + " exists, setting connected once more");
            Debug.LogError("connected state is " + pm.isConnected(idValOld));
            pm.setNewID(idValOld, idValue); //Makes new id connection based off new conn id
            pm.setConnected(idValue); //Indicate player is again connected
            pm.setConnValue(idValue, msg.conn.connectionId); //Updates old conn value
            pm.setNetworkConnection(idValue, msg.conn);

            PlayerValues pv = new PlayerValues();
            pv.dictId = idValue;
            pv.oldId = idValOld;
            pv.connVal = msg.conn.connectionId;
            pv.playerIP = address;
            pv.playerName = name;
            pv.playerTeam = teamChoice;
            NetworkServer.SendToClient(msg.conn.connectionId, Msgs.addNewPlayer, pv);
            if (teamChoice == TeamID.TEAM_OBSERVER)
            { //Checks if player is indicated as an observer. If so adds to observers list
                observingListeners.Add(msg.conn);
            }
            else
            {//If it's not an observer, inform observers
                foreach (NetworkConnection nc in getUpdateListeners())
                {
                    NetworkServer.SendToClient(nc.connectionId, Msgs.updatePlayerToObserver, pv); //Sends player info to the client that re-connected
                }
            }

        } else { //If is entirely new player
            Debug.Log("New player "+ address + ", given id" + idValue + ", offering team " + teamChoice);
            Player newPlayer = new Player(idValue, msg.conn.connectionId, address, name, teamChoice);
            
            pm.addPlayer(idValue, newPlayer);
            pm.setConnected(idValue); //Indicate player is again connected
            pm.setConnValue(idValue, msg.conn.connectionId); //Updates old conn value
            pm.setNetworkConnection(idValue, msg.conn);


            PlayerValues pv = new PlayerValues();
            pv.dictId = idValue;
            pv.connVal = msg.conn.connectionId;
            pv.playerIP = address;
            pv.playerName = pm.getName(idValue);
            pv.playerTeam = pm.getTeam(idValue);
            NetworkServer.SendToClient(msg.conn.connectionId, Msgs.addNewPlayer, pv); //Sends player info to the client that re-connected
            if (teamChoice == TeamID.TEAM_OBSERVER)
            { //Checks if player is indicated as an observer. If so adds to observers list
                observingListeners.Add(msg.conn);
            }
            else
            {//If it's not an observer, inform observers
                foreach (NetworkConnection nc in getUpdateListeners())
                {
                    NetworkServer.SendToClient(nc.connectionId, Msgs.addNewPlayerToObserver, pv); //Sends player info to the client that re-connected
                }
            }
            //NEED TO UPDATE OBSERVER'S INFORMATION AS WELL, AS THIS MAY OCCUR MID-GAME
        }
    }

    //called when a player is removed for a client
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController) {
        GameObject player = playerController.gameObject;
        if (player != null){
            int id = IDFromConn(conn);
           // sendScore(pm.getTeam(idVal));
            if (playerController.unetView != null){
                NetworkServer.Destroy(player);
                Debug.LogError("OnServerRemovePlayer: Destroying player"); // We shall finish our business on slack plz :)
            }
            updateListeners.Remove(conn);
        }
    }


    //REPLACE
    //This function sends the current in-game time to the client requesting time.
    private void OnServerRecieveTimeRequest(NetworkMessage netMsg){
        TimeMessage timeMessage = new TimeMessage();
        timeMessage.time = timerRound;
        NetworkServer.SendToClient(IDFromConn(netMsg.conn), Msgs.sendCurrentTime, timeMessage);
    }

    private int IDFromConn(NetworkConnection nc) {
 		return nc.connectionId;
  	}

	public void OnServerRecieveTeamRequest(NetworkMessage msg) {

 //TODO : MAKE ONLY SEND TO OBSERVERS
    	sendTeam(TeamID.TEAM_PIRATES);
    	sendTeam(TeamID.TEAM_SUPERCORP);
    }

  	public void OnServerRecieveTeamChoice(NetworkMessage msg) {
	    TeamChoice teamChoice = msg.ReadMessage<TeamChoice>();
	    int choice = teamChoice.teamChoice;
     int idVal = ipToId(msg.conn.address, msg.conn.connectionId);
        if (!pm.checkIfExists(idVal)) {
            Debug.LogError("ID DOES NOT EXISTS, CONN IS "+msg.conn.connectionId);
            return;
        }
     int team = pm.getTeam(idVal);

	    // if the player is choosing the team for the first time
		if (team == TeamID.TEAM_NEUTRAL){
			// update the team and send updated list to all clients
			
			pm.setTeam(idVal, choice);	
			teamManager.addPlayerToTeam(pm.getName(idVal), choice);

			sendTeam (choice);

		} else if (pm.getTeam(idVal) != choice) {	// if the player has switched teams
			// delete player from old list and send updated list to all clients
			teamManager.deletePlayer(pm.getName(idVal), pm.getTeam(idVal));
			sendTeam (pm.getTeam(idVal));

			// add player to new team and send updated list to clients
			pm.setTeam(idVal, choice);
			teamManager.addPlayerToTeam(pm.getName(idVal), choice);
			sendTeam (choice);
		}

        //Send changes to the observers
        PlayerValues pv = new PlayerValues();
        pv.dictId = idVal;
        pv.oldId = idVal;
        pv.connVal = msg.conn.connectionId;
        pv.playerIP = msg.conn.address;
        pv.playerName = name;
        pv.playerTeam = choice;
        foreach (NetworkConnection nc in getUpdateListeners()){
            NetworkServer.SendToClient(nc.connectionId, Msgs.updatePlayerToObserver, pv); //Sends player info to the client that re-connected
        }
	  }
    // send the team list of players to all clients
    public void sendTeam(int team) {
		string display = "";
		TeamList tl = new TeamList();

		foreach(string player in teamManager.getListTeam(team) ) {
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

	
	/* ------------------  Client functions ---------------- */
	
    // called when connected to a server
	public override void OnClientConnect(NetworkConnection conn) {
		Debug.Log("Client connected!");
	}

    public override void OnStartClient(NetworkClient client){
        base.OnStartClient(client);
        ppi = PersonalPlayerInfo.singleton;
        client.RegisterHandler(Msgs.updatePlayer, OnPlayerUpdate);
        client.RegisterHandler(Msgs.addNewPlayer, OnNewPlayer);
        client.RegisterHandler(Msgs.addNewPlayerToObserver, OnNewPlayerObserver);
        client.RegisterHandler(Msgs.updatePlayerToObserver, OnPlayerUpdateObserver);
    }

    //TODO: USE THIS WHEN PLAYER CHANGES TEAM AND SUCH
    public void OnPlayerUpdate(NetworkMessage msg) {
      PlayerValues pv = msg.ReadMessage<PlayerValues>();
      //  pm.updatePlayer(pv.dictId, pv.player);
      Debug.Log("attempting to update player");
    }

    //Updates Observer player
    public void OnPlayerUpdateObserver(NetworkMessage msg){
        PlayerValues pv = msg.ReadMessage<PlayerValues>();
        Player updatedPlayer = new Player(pv.dictId, pv.connVal, pv.playerIP, pv.playerName, pv.playerTeam);
        pm.updatePlayerIncludingID(pv.oldId, updatedPlayer);
        Debug.Log("attempting to update player");
    }

    /*
    //TODO: THIS IS CALLED WHEN PLAYER DISCONNECTS, SETS CONNECTED INDICATOR TO FALSE ON OBSERVERS
    public void OnPlayerDisconnected(NetworkMessage msg){

    }
    */
    public void OnNewPlayer(NetworkMessage msg) {
        PlayerValues pv = msg.ReadMessage<PlayerValues>();
        Player newPlayer = new Player(pv.dictId, pv.connVal, pv.playerIP, pv.playerName, pv.playerTeam);
        ppi.setPlayerDetails(pv.dictId, newPlayer); //Sets details of player to non-destroyable space
    }

    public void OnNewPlayerObserver(NetworkMessage msg){
        PlayerValues pv = msg.ReadMessage<PlayerValues>();
        Player newPlayer = new Player(pv.dictId, pv.connVal, pv.playerIP, pv.playerName, pv.playerTeam);
            pm.addPlayer(pv.dictId, newPlayer);
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