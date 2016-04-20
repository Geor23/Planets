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

    public RoundScoreManager(Dictionary<int, Player> pd){
        pm = PlayerManager.singleton;
        playerDict = pd; //Reference to other dictionary, means changes here change there too

    }

    public void increasePlayerScore(int playerId, int score){
        playerDict[playerId].incrementPlayerScore(score);
        if(pm.getTeam(playerId) == TeamID.TEAM_PIRATES){
            pirateScore += score;
        } else {
            superCorpScore += score;
        }
    }

    public void decreasePlayerScore(int playerId, int score){
        playerDict[playerId].decrementPlayerScore(score);
        if(pm.getTeam(playerId) == TeamID.TEAM_PIRATES){
            pirateScore -= score;
        } else {
            superCorpScore -= score;
        }
    }


    public int getPirateScore(){
        return pirateScore;
    }

    public int getSuperCorpScore(){
        return superCorpScore;
    }

    public int getPlayerScore(int playerId){
        Player p = pm.getPlayer(playerId);
        if(p == null) return 0;
        else return p.getPlayerScore();
    }
}
