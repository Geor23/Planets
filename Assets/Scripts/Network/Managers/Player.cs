using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Player data
public class Player : MonoBehaviour {
    private int playerId;
    private string playerIP;
    private string playerName;
    private int playerScore;
    private int playerTeam;

    public int getPlayerId() {
        return playerId;
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

    public void incrementPlayerScore(int score){
        playerScore += score;
    }
}
