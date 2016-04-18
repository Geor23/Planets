using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Spawn+Death manager maybe?
//Use to manage player objects on the local side, with functionality to request server to 
public class RoundPlayerObjectManager { //TODO

    private List<int> deadPlayers;
    private List<KillHistory> killHistory;

    public RoundPlayerObjectManager(){
        deadPlayers = new List<int>();
        killHistory = new List<KillHistory>();
    }

    //Spawns player with the supplied Id. Currently done on server, maybe do on Observer if we remove networked objects
    public void spawnPlayerServer(int playerId){

    }

    //Kills player, potentially starts timer for respawn too
    public void killPlayerLocal(int playerIdKilled, int playerIdKiller){ //TODO
        //Remove game object locally
        //Remove score (call below stuff)
    }

    //Kills player, potentially starts timer for respawn too
    public void killPlayerServer(int playerId, int playerIdKiller){ //TODO
        //Remove game object locally
        //Remove score (call below stuff)
    }

    public void relayDataToServer(){
        //ToDO
    }
}