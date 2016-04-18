using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Deals with player details.
public class PlayerManager : MonoBehaviour
{
    //Int is connection ID, player contains name, game ID, team, etc.
    private Dictionary<int, Player> playerDict;
    public PlayerManager()
    {
        DontDestroyOnLoad(this);
        playerDict = new Dictionary<int, Player>();
    }

    public void addPlayer(int id, Player player)
    {
        playerDict.Add(id, player);
    }

    public void removePlayer(int id)
    {
        playerDict.Remove(id);
    }

    public Player getPlayer(int id)
    {
        if (playerDict.ContainsKey(id))
        {
            return playerDict[id];
        }
        else
        {
            return null;
        }
    }
    public Dictionary<int, Player> getPlayerDict()
    {
        return playerDict;
    }

    //Informs calling function if a player IP has already previously connected. Returns True if so, False if no.
    public bool checkIfExists(int id){
        if (playerDict.ContainsKey(id)){
            return true;
        } else {
            return false;
        }
    }

    public void setConnected(int id) {
        playerDict[id].setIsConnected(true);
    }

    public void setDisconnected(int id){
        playerDict[id].setIsConnected(false);
    }
}
