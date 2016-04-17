using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


[System.Serializable]
public class Team {

	public Vector3 spawnPoints;

	public List<string> players = new List<string>() ;
	public int score = 0 ;

	public void addScore( int scoreToAdd ) {
		score += scoreToAdd ;
	}

	public void removeScore( int scoreToRemove ) {
		score -= scoreToRemove ;
	}

	public int getScore() {
		return score ;
	}

	public List<string> getPlayers () {
		return players ;
	}

	public void addPlayer (string playerName) {
		players.Add(playerName) ;
	}

	public void removePlayer (string playerName) {
		players.Remove(playerName) ;
	}

	public void resetScore() {
		score = 0;
	}

	public void setSpawnPoint(Vector3 spawn) {
		spawnPoints = spawn;
	}

	public Vector3 getSpawnPoint() {
		return spawnPoints;
	}


}