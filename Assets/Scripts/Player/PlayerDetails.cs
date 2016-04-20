using UnityEngine;
using System.Collections;

public class PlayerDetails : MonoBehaviour {
    private int dictId;
    private Player playerDetails;

    //Called by the function which creates players, assigning details to it
    public void setPlayerDetails(int id, Player playerDet) {
        dictId = id;
        playerDetails = playerDet;
    }

    public int getDictId() {
        return dictId;
    }

    public int getObsId(){
        return playerDetails.getConnValue();
    }

    public string getPlayerName() {
        return playerDetails.getPlayerName();
    }

    public string getPlayerIP(){
        return playerDetails.getPlayerIP();
    }

    public int getPlayerScore(){
        return playerDetails.getPlayerScore();
    }

    public int getPlayerTeam(){
        return playerDetails.getPlayerTeam();
    }
}
