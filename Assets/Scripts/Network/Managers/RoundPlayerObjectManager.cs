using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

static class KillType {
    public const int BULLET = 0;
    public const int METEOR = 1;
    public const int ELE = 2;
};

public class KillInfo {
    public int killerId;
    public int killedId;
    public int killType;
    public int time;

    public KillInfo(int killerId, int killedId, int killType){
        this.killerId = killerId;
        this.killedId = killedId;
        this.killType = killType;
        time = (int) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }
}

//Spawn+Death manager maybe?
//Use to manage player objects on the local side, with functionality to request server to 
public class RoundPlayerObjectManager { //TODO

    private int roundNum;
    private List<int> deadPlayers;
    private List<KillInfo> killHistory;
    private PlayerManager pm;

    public RoundPlayerObjectManager(){
        deadPlayers = new List<int>();
        killHistory = new List<KillInfo>();
        pm = PlayerManager.singleton;
    }

    //Kills player, potentially starts timer for respawn too
    public void killPlayerLocal(int playerIdKilled, int playerIdKiller){ //TODO
        KillInfo ki = new KillInfo(playerIdKiller, playerIdKilled, KillType.ELE);
        killHistory.Add(ki);
    }

    public string getKillString(int i){
        KillInfo ki = killHistory[i];
        if(i < killHistory.Count)
            return pm.getName(ki.killerId) + " " + getKillTypeString(ki.killType) + " "  + pm.getName(ki.killedId);
        return "";
    }

    public string getKillsAsList(int howMany){
        string kills = "";
        for (int i = killHistory.Count - 1; i >= 0 ; i--) {
            kills += getKillString(i);
        }
        return kills;
    }

    public string getLatestKill(){
        return getKillString(killHistory.Count - 1);
    }

    public string getKillTypeString(int killType){
        switch(killType){
            case KillType.BULLET: return "blasted";
            case KillType.METEOR: return "meteored";
            case KillType.ELE:    return "eleftheriosized";
        }
        return "noobed";
    }

}