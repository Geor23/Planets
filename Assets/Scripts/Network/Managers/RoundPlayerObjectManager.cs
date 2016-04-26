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
    private Queue<int> deadPlayers;
    private List<KillInfo> killHistory;
    private PlayerManager pm;

    public int pirateKills = 0;
    public int superCorpKills = 0;
    public int pirateDeaths = 0;
    public int superCorpDeaths = 0;


    public RoundPlayerObjectManager(){
        deadPlayers = new Queue<int>();
        killHistory = new List<KillInfo>();
        pm = PlayerManager.singleton;
    }

    //Kills player, potentially starts timer for respawn too
    public void killPlayerLocal(int playerIdKilled, int playerIdKiller){ //TODO
        KillInfo ki = new KillInfo(playerIdKiller, playerIdKilled, KillType.ELE);
        killHistory.Add(ki);
        deadPlayers.Enqueue(playerIdKilled);

        //Increment team kill and death counters
        //Got killed players team
        int team = pm.getPlayer(playerIdKilled).getPlayerTeam();
        if (team == TeamID.TEAM_SUPERCORP) {
            pirateKills += 1;
            superCorpDeaths += 1;
        }
        else {
            pirateDeaths += 1;
            superCorpKills += 1;
        }
    }

    public int mostRecentDeath(){
        if(deadPlayers.Count != 0)
            return deadPlayers.Dequeue();
        return -25;
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
            kills += getKillString(i) + '\n';
        }
        return kills;
    }

    public string getLatestKill(){
        if(killHistory.Count == 0) return "";
        return getKillString(killHistory.Count - 1);
    }

    public string getKillTypeString(int killType) {
        switch(killType) {
            case KillType.BULLET: return "blasted";
            case KillType.METEOR: return "meteored";
            case KillType.ELE:    return "eleftheriosized";
        }
        return "noobed";
    }

    public Player getPlayerWithMostKills(int team){
        int max = 0;
        Player maxP = null;
        foreach(KeyValuePair<int, Player> kv in pm.getPlayerDict()){
            int prs = kv.Value.getKills();
            if(prs > max && kv.Value.getPlayerTeam() == team){
                max = prs;
                maxP = kv.Value;
            }
        }
        return maxP;
    }

    public Player getPlayerWithLeastDeaths(int team){
        int min = 10000;
        Player minP = null;
        foreach(KeyValuePair<int, Player> kv in pm.getPlayerDict()){
            int prs = kv.Value.getDeaths();
            if(prs < min && kv.Value.getPlayerTeam() == team){
                min = prs;
                minP = kv.Value;
            }
        }
        return minP;
    }
}