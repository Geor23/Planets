using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Player data
public class Player {
    private int playerDictVal;
    private int connVal;
    private string playerIP;
    private string playerName;
    private int playerTeam;
    private bool isConnected;

    private int playerScoreRound; //Contains final personal score of a round
    private int playerScoreAccRound;  //MAYBE?
    private int playerScoreTotal; //Contains final personal score of all rounds
    private int playerKills; //Contains number of times you killed in a round
    private int playerDeaths; //Contains number of times you died in a round

    public Player (int dictVal, int conn, string playIP, string playName, int teamChoice, int playerD, int playerK, int playerScoreAR, int playerTotal){
        playerDictVal = dictVal;
        connVal = conn;
        playerIP = playIP;
        playerName = playName;
        playerScoreRound = 0;
        playerTeam = teamChoice; //Unassigned team, not an observer
        isConnected = true;
        playerDeaths = playerK; //TODO: Ensure these values pass on if you dc and reconnect
        playerKills = playerD;
        playerScoreAccRound = playerScoreAR;
        playerScoreTotal = playerTotal;
    }

    public int getTotalScore() {
        return playerScoreTotal;
    }
    
    public int getRoundScore() {
        return playerScoreRound;
    }

    public int getRoundScoreAcc() {
        return playerScoreAccRound;
    }

    public int getKills() {
        return playerKills;
    }

    public int getDeaths() {
        return playerDeaths;
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

    public int getPlayerScoreRound() {
        return playerScoreRound;
    }

    public int getPlayerTeam() {
        return playerTeam;
    }

    public void addScore(int score) {
        playerScoreRound    += score;
        playerScoreTotal    += score;
        playerScoreAccRound += score;
    }

    public void playerDie() {
        playerScoreTotal -= playerScoreRound;
        playerScoreRound = 0;
        playerDeaths += 1;
    }

    public bool getIsConnected(){
        return isConnected;
    }


    public void setIsConnected(bool connBool) {
        isConnected = connBool;
    }

    public void setConnValue(int conn) {
        connVal = conn;
    }

    public int getConnValue(){
        return connVal;
    }

    public void setPlayerName(string name) {
        playerName = name;
    }

    public void setPlayerTeam(int pTeam) {
        playerTeam = pTeam;
    }

   public void setPlayerDictVal(int id){
        playerDictVal = id;
    }
}
