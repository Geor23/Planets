using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
//Deals with player details.
public class PlayerManager {
    //Int is connection ID, player contains name, game ID, team, etc.
    public static PlayerManager singleton;
    private Dictionary<int, Player> playerDict;
    private Dictionary<int, NetworkConnection> connectionDict;
    public PlayerManager(){
        //DontDestroyOnLoad(this);
        singleton = this;
        playerDict = new Dictionary<int, Player>();
        connectionDict = new Dictionary<int, NetworkConnection>();
    }

    //Replaces old version of player with new one
    public void setNewID(int oldID, int newID){
        Player player = playerDict[oldID];
        playerDict.Remove(oldID);
        player.setPlayerDictVal(newID);
        playerDict.Add(newID, player);
    }

    //Removes old version of player and adds again
    public void updatePlayerIncludingID(int oldID, Player player){
        playerDict.Remove(oldID);
        playerDict.Add(player.getPlayerId(), player);
    }

    public int findPlayerWithIP(string ip){
        foreach (var i in playerDict) {
            string val = playerDict[i.Key].getPlayerIP();
            if ((ip == val)&&(!playerDict[i.Key].getIsConnected())){ //If the IPS match up and the person was disconnected...
                return playerDict[i.Key].getPlayerId();
            }
        }
        return -10; //Indicates failure to find IP specified
    }

    public bool isConnected(int id){
        return playerDict[id].getIsConnected();
    }

    public string findPlayerWithConnID(int id){
        foreach (var i in playerDict){
            int val = playerDict[i.Key].getConnValue();
            if(id == val){
                return playerDict[i.Key].getPlayerIP();
            }
        }

        return null; //Indicates nothing found

    }
    public void addPlayer(int id, Player player){
        playerDict.Add(id, player);
    }

    public void updatePlayer(int id, Player player){
        playerDict[id] = player; //Updates player to new values
    }

    public void removePlayer(int id){
        playerDict.Remove(id);
        connectionDict.Remove(id);
    }

    public Player getPlayer(int id){
        if (playerDict.ContainsKey(id)){
            return playerDict[id];
        } else {
            return null;
        }
    }

    public Dictionary<int, Player> getPlayerDict(){
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
    
    public void setConnValue(int id, int conn) {
        playerDict[id].setConnValue(conn);
    }

    public void setNetworkConnection(int id, NetworkConnection conn){
        connectionDict[id] = conn;
    }



    public string getName(int id) {
        return playerDict[id].getPlayerName();
    }

    public void setName(int id, string name){
        playerDict[id].setPlayerName(name);
    }

    public int getTeam(int id) {
       return playerDict[id].getPlayerTeam();
    }

    public void setTeam(int id, int teamChoice) {
        playerDict[id].setPlayerTeam(teamChoice);
    }

    public NetworkConnection getNetworkConnection(int id){
        if (connectionDict.ContainsKey(id)){
            return connectionDict[id];
        } else {
            return null;
        }
    }
}
