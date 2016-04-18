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

    void Start(){
        pm = PlayerManager.singleton;
        pom = new RoundPlayerObjectManager();
        Dictionary<int, Player> playerDict = pm.getPlayerDict();
        sm = new RoundScoreManager(playerDict);
        //Handle messages from server such as end of round signal etc. act upon them

        if(NetworkClient.active){
            //Enable client callbacks
        }

        if(NetworkServer.active){
            //Enable server callbacks
        }
    }

    //These can be called inside the playercontroller mobile so that we can tap int othe object manager functionality
    public RoundPlayerObjectManager getRoundPlayerObjectManager(){
        return pom;
    }

    //Same as above, except for scores
    public RoundScoreManager getRoundScoreManager(){
        return sm;
    }

    public void playerDeath(int playerKilledId, int playerKillerId){
        //TODO
    }



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