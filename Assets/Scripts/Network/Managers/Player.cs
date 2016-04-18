using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Player data
public class Player {
    private int playerDictVal;
    private int connVal;
    private string playerIP;
    private string playerName;
    private int playerScore;
    private int playerTeam;
    private bool isConnected;

    public Player (int dictVal, int conn, string playIP, string playName){
        playerDictVal = dictVal;
        connVal = conn;
        playerIP = playIP;
        playerName = playName;
        playerScore = 0;
        playerTeam = TeamID.TEAM_NEUTRAL; //Unassigned team, not an observer
        isConnected = true;
    }

    public int getPlayerId() {
        return playerDictVal;
    }

    public string getPlayerIP() {
        return playerIP;
    }

    public string getPlayerName() {
        return playerName;
    }

    public int getPlayerScore() {
        return playerScore;
    }

    public int getPlayerTeam() {
        return playerTeam;
    }

    public bool getIsConnected(){
        return isConnected;
    }

    public void incrementPlayerScore(int score){
        playerScore += score;
    }

    public void decrementPlayerScore(int score){
        playerScore -= score;
    }

    public void setIsConnected(bool connBool) {
        isConnected = connBool;
    }

    public void setConnValue(int conn) {
        connVal = conn;
    }

    public void setPlayerName(string name) {
        playerName = name;
    }

}
