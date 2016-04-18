using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//All classes are per round
//We can make other classes for data that needs to persist across rounds like full game stats etc...



public class RoundEvents : MonoBehaviour {

    public RoundPlayerObjectManager pom;
    public RoundScoreManager sm;

    void Start(){
        pom = new RoundPlayerObjectManager();
        sm = new RoundScoreManager();


        //Handle messages from server such as end of round signal etc. act upon them
    }

    //These can be called inside the playercontroller mobile so that we can tap int othe object manager functionality

    public RoundPlayerObjectManager getRoundPlayerObjectManager(){
        return pom;
    }


    //Same as above, except for scores
    public RoundScoreManager getRoundScoreManager(){
        return sm;
    }

    public void relayDataToServer(){
        //Call functons inside ObjectManager/ScoreManager respectively to sync them together
        //Possibly called at the end of the round
        pom.relayDataToServer();
        sm.relayDataToServer();
    }
    
}

/// <summary>
//All code in here should have either Server/Client suffixed to it. Client indicates the code is called on a Client.
//Server indicates the code is called on a server.
//WIP means the function still needs doing.
//CHANGE means adjustements are made even if the function works as implemented
/// </summary>

public class KillHistory {
    public int playerKiller;
    public int playerKilled;
    public int time;
}


//Spawn+Death manager maybe?
//Use to manage player objects on the local side, with functionality to request server to 
public class RoundPlayerObjectManager
{ //WIP

    private List<int> deadPlayers;
    private List<KillHistory> killHistory;

    public RoundPlayerObjectManager(){
        deadPlayers = new List<int>();
        killHistory = new List<KillHistory>();
    }

    //Spawns player with the supplied Id. Currently done on server, maybe do on Observer if we remove networked objects
    public void spawnPlayerServer(int playerId)
    {

    }

    //Kills player, potentially starts timer for respawn too
    public void killPlayerLocal(int playerIdKilled, int playerIdKiller)
    { //WIP
        //Remove game object locally
        //Remove score (call below stuff)
    }

    //Kills player, potentially starts timer for respawn too
    public void killPlayerServer(int playerId, int playerIdKiller)
    { //WIP
        //Remove game object locally
        //Remove score (call below stuff)
    }

    public void relayDataToServer(){
            //ToDO
    }
}

//Deals with scores of players. Scoring/loss of score by death (overlap with above). Potentially stores individual scores as they'll pass through anyway.
//Deals with both Observers sending scores, and the Server passing scores to clients/recieving and storing them.
public class RoundScoreManager
{ //WIP
    int pirateScore;
    int superScore;
    //List of players/their scores potentially
    private Dictionary<int, int> playerScores;

    public RoundScoreManager(){
        playerScores = new Dictionary<int, int>();
    }


    public void increasePlayerScore(int playerId, int score) {
        //ToDO
    }

    public void decreasePlayerScore(int playerId, int score){
        //ToDO
    }

    //Request to add/remove score
    public void ChangeScoreServer(int playerId, int scoreChange)
    { //WIP

    }

    //Returns score when requested
    public int sendScoreClient()
    { //WIP
        return 0; //Will return score
    }

    public void relayDataToServer(){
        //ToDO
    }
}

/*
[System.Serializable]
public class TeamManager { //CHANGE
    //Needs removal of spawn details preferably, and placement of some of its content into other classes
    public List<Team> teams;

    public PlayerSpawnAreas playerSpawnAreas;


    public TeamManager(){
        teams = new List<Team>();
        Team teamPirates = new Team();
        Team teamSuperCorp = new Team();
        teams.Add(teamPirates);
        teams.Add(teamSuperCorp);
        teams[0].setSpawnPoint(new Vector3(0, 0, 120));
        teams[1].setSpawnPoint(new Vector3(0, -40, 0));
    }

    //Change to get spawn point from PlayerSpawnArea script
    public Vector3 getSpawnP(int team){
        playerSpawnAreas = GameObject.FindGameObjectWithTag("Planet").GetComponent<PlayerSpawnAreas>();

        if ((team == 0) || (team == 1)){
            return playerSpawnAreas.generateSpawnPoint(team);
            //return teams[team].getSpawnPoint();
        }else{
            Debug.Log("Team is observer");
            return new Vector3(0, 0, 100);
        }
    }

    public void addScore(int score, int team){
        if (team == 0 || team == 1){
            teams[team].addScore(score);
        }else{
            Debug.LogError("ERROR[addScore]: You are trying to access a non-existant team ! ");
        }
    }

    public void removeScore(int score, int team){
        if (team == 0 || team == 1){
            teams[team].removeScore(score);
        }else {
            Debug.LogError("ERROR[removeScore]: You are trying to access a non-existant team ! ");
        }
    }

    public void deletePlayer(String playerName, int team){
        if (team == 0 || team == 1){
            teams[team].removePlayer(playerName);
        }else{
            Debug.LogError("ERROR[deletePlayer]: You are trying to access a non-existant team ! ");
        }
    }

    public void addPlayerToTeam(string playerName, int team){
        if (team == 0 || team == 1){
            teams[team].addPlayer(playerName);
        }else{
            Debug.LogError("ERROR[addPlayerToTeam]: You are trying to access a non-existant team ! ");
        }
    }

    public List<string> getListTeam(int team){
        if (team == 0 || team == 1){
            return teams[team].getPlayers();
        }else{
            Debug.LogError("ERROR[getListTeam]: You are trying to access a non-existant team ! ");
            return null;
        }
    }

    public int getScore(int team){

        if (team == 0 || team == 1){
            return teams[team].getScore();
        }else{
            Debug.LogError("ERROR[getScore]: You are trying to access a non-existant team ! ");
            return 0;
        }
    }

    public void resetScores(){
        teams[0].resetScore();
        teams[1].resetScore();
    }

}
*/
