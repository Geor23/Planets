﻿using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


//All classes are per round
//We can make other classes for data that needs to persist across rounds like full game stats etc...



public class RoundEvents : MonoBehaviour {
    public RoundPlayerObjectManager pom;
    public RoundScoreManager sm;
    public PlayerManager pm;
    public PlanetsNetworkManager nm;

    void Start(){
        pm = PlayerManager.singleton;
        pom = new RoundPlayerObjectManager();
        Dictionary<int, Player> playerDict = pm.getPlayerDict();
        sm = new RoundScoreManager(playerDict);
        nm = (PlanetsNetworkManager)PlanetsNetworkManager.singleton;
        //Handle messages from server such as end of round signal etc. act upon them

        if(NetworkClient.active) enableClientCallbacks();
        if(NetworkServer.active) enableServerCallbacks();
    }

    void enableClientCallbacks(){
        nm.client.RegisterHandler(Msgs.killPlayer, OnClientDeath);

        // If observer, invoke repeating respawn players
        if(PlayerConfig.singleton.GetObserver()){
            InvokeRepeating("respawnPlayer", 0, 1); // Respawn 1 player per second
        }
    }

    void enableServerCallbacks(){
        NetworkServer.RegisterHandler(Msgs.killPlayer, OnServerRegisterDeath);
        NetworkServer.RegisterHandler(Msgs.spawnPlayer, OnServerSpawnPlayer);
    }

    //These can be called inside the playercontroller mobile so that we can tap int othe object manager functionality
    public RoundPlayerObjectManager getRoundPlayerObjectManager(){
        return pom;
    }

    //Same as above, except for scores
    public RoundScoreManager getRoundScoreManager(){
        return sm;
    }

    /* CLIENT FUNCS PLZ */

    public void respawnPlayer(){
        int playerId = pom.mostRecentDeath();
        if(playerId == -25) return;
        PlayerSpawnMsg ps = new PlayerSpawnMsg();
        ps.playerId = playerId;
        ps.pos = new Vector3(60,60,60); //Observer cam;
        nm.client.Send(Msgs.spawnPlayer, ps);
    }

    public void registerKill(NetworkInstanceId netId, int playerKilledId, int playerKillerId){
        KillPlayer kp = new KillPlayer();
        kp.netId = netId;
        nm.client.Send(Msgs.killPlayer, kp);
        pom.killPlayerLocal(playerKilledId, playerKillerId);
        Debug.Log("Player " + playerKilledId + " died");
    }

    public void OnClientDeath(NetworkMessage msg){
        //Find player object, and call function
        // deathText.enabled = true; //Causes timer for next spawn to occur
        // deathTimerText.enabled = true;
        // mainCamera.enabled = true;
        ClientScene.RemovePlayer(0);
    }

    /* SERVER FUNCS PLZ */

    public void OnServerRegisterDeath(NetworkMessage msg){
        KillPlayer kp = msg.ReadMessage<KillPlayer>();
        GameObject obj = NetworkServer.FindLocalObject(kp.netId); // Relay msg to player
        if(obj == null) return;
        NetworkServer.SendToClientOfPlayer(obj, Msgs.killPlayer, kp);
    }

    public void OnServerSpawnPlayer(NetworkMessage msg){
        PlayerSpawnMsg ps = msg.ReadMessage<PlayerSpawnMsg>();
        GameObject player = Instantiate(nm.playerObjectType(ps.playerId), ps.pos, Quaternion.identity) as GameObject;
        player.GetComponent<UnityStandardAssets.CrossPlatformInput.PlayerControllerMobile>().dictId = ps.playerId;
        Debug.Log("Player " + ps.playerId + "being respawned");
        NetworkServer.AddPlayerForConnection(pm.getNetworkConnection(ps.playerId), player, 0);
    }


    //Extras junk

    public void relayDataToServer(){
        //Call functons inside ObjectManager/ScoreManager respectively to sync them together
        //Possibly called at the end of the round
    }
//Called by Observer upon collecting a resource. Calls score addition in RoundScoreManager
    
}

/// <summary>
//All code in here should have either Server/Client suffixed to it. Client indicates the code is called on a Client.
//Server indicates the code is called on a server.
//TODO means the function still needs doing.
//CHANGE means adjustements are made even if the function works as implemented
/// </summary>