using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RoundScoreManager {
    //Deals with scores of players. Scoring/loss of score by death (overlap with above). Potentially stores individual scores as they'll pass through anyway.
    //Deals with both Observers sending scores, and the Server passing scores to clients/recieving and storing them.
    private int pirateScore;
    private int superCorpScore;
    private PlayerManager pm;
    private Dictionary<int, Player> playerDict;

    public RoundScoreManager(){
        pm = PlayerManager.singleton;
    }

    public void increasePlayerScore(int playerId, int score){
        playerDict[playerId].incrementPlayerScore(score);
        if(pm.getTeam(playerId) == TeamID.TEAM_PIRATES){
            pirateScore += score;
        } else {
            superCorpScore += score;
        }
    }

    public void decrementPlayerScore(int playerId, int score){
        playerDict[playerId].decrementPlayerScore(score);
        if(pm.getTeam(playerId) == TeamID.TEAM_PIRATES){
            pirateScore -= score;
        } else {
            superCorpScore -= score;
        }
    }

    public void setPlayerDict(Dictionary<int, Player> playerDic) {
        playerDict = playerDic;
    }
}
