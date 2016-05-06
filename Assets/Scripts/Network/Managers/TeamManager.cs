using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class TeamManager {
	public List<Team> teams;

    public PlayerSpawnAreas playerSpawnAreas;


    public TeamManager() {
		teams = new List<Team>() ;
		Team teamPirates = new Team() ;
		Team teamSuperCorp = new Team() ;
		teams.Add(teamPirates) ;	
		teams.Add(teamSuperCorp) ;	
		teams[0].setSpawnPoint(new Vector3(0,0,120));
		teams[1].setSpawnPoint(new Vector3(0,-40,0));

	}

    //Change to get spawn point from PlayerSpawnArea script
	public Vector3 getSpawnP(int team){
        playerSpawnAreas = GameObject.FindGameObjectWithTag("Planet").GetComponent<PlayerSpawnAreas>();
        if ((team == 0) || (team == 1)) {
            return playerSpawnAreas.generateSpawnPoint(team);
			//return teams[team].getSpawnPoint();
		} else {
			Debug.Log("Team is observer");
			return new Vector3(0,0,100);
		}
	}

    public int getTeamWithLessPlayers() {
        int numPirates = teams[0].getPlayers().Count;
        int numSuper = teams[1].getPlayers().Count;
        if (numPirates <= numSuper) {
            return TeamID.TEAM_PIRATES;
        } else { 
           return TeamID.TEAM_SUPERCORP;
        }
    }

	public void addScore(int score, int team) {
		if ( team == 0 || team == 1 ) {
			teams[team].addScore(score);
		} else {
			Debug.LogError("ERROR[addScore]: You are trying to access a non-existant team ! ");
		}

	}

	public void removeScore(int score, int team) {
		if ( team == 0 || team == 1 ) {
			teams[team].removeScore(score);
		} else {
			Debug.LogError("ERROR[removeScore]: You are trying to access a non-existant team ! ");
		}

	}
	
	public void deletePlayer (String playerName, int team) {

		if (team == 0 || team == 1 ) {

			teams[team].removePlayer(playerName);

		} else {

			Debug.LogError("ERROR[deletePlayer]: You are trying to access a non-existant team ! ");
		}

	}
	
	public void addPlayerToTeam( string playerName, int team) {

		if (team == 0 || team == 1) {

			teams[team].addPlayer(playerName) ;

		} else {

			Debug.LogError("ERROR[addPlayerToTeam]: You are trying to access a non-existant team ! ");

		}

	}


	public List<string> getListTeam (int team) {

		if (team == 0 || team == 1) {

			return teams[team].getPlayers() ;

		} else {

			Debug.LogError("ERROR[getListTeam]: You are trying to access a non-existant team ! ");
			return null;

		}

	}


	public int getScore(int team) {

		if (team == 0 || team == 1) {

			return teams[team].getScore() ;

		} else {

			Debug.LogError("ERROR[getScore]: You are trying to access a non-existant team ! ");
			return 0;

		}

	}


	public void resetScores() {
		teams[0].resetScore();
		teams[1].resetScore();
	}

	
}
