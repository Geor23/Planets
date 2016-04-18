using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Player data
public class Player : MonoBehaviour {
    private int playerDictVal;
    private int connVal;
    private string playerIP;
    private string playerName;
    private int playerScore;
    private int playerTeam;
    private bool isConnected;

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

    public void setIsConnected(bool connBool) {
        isConnected = connBool;
    }


}
