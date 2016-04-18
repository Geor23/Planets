using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoundScoreManager {
    //Deals with scores of players. Scoring/loss of score by death (overlap with above). Potentially stores individual scores as they'll pass through anyway.
    //Deals with both Observers sending scores, and the Server passing scores to clients/recieving and storing them.
    private int pirateScore;
    private int superScore;
    //List of players/their scores potentially
    private Dictionary<int, Player> playerDict;

    public RoundScoreManager(){

    }

    public void increasePlayerScore(int playerId, int score){
        playerDict[playerId].incrementPlayerScore(score);
    }

    public void decreasePlayerScore(int playerId, int score){
            //TODO
    }

    //Request to add/remove score
    public void changeScoreServer(int playerId, int scoreChange){ //TODO

    }

    //Returns score when requested
    public int sendScoreClient(){ //TODO
        return 0; //Will return score
    }

    public void relayDataToServer(){
        //TODO
    }

    public void setPlayerDict(Dictionary<int, Player> playerDic) {
        playerDict = playerDic;
    }
}
