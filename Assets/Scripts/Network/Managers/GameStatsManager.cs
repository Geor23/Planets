using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


//Class to churn all the of the data from all of the rounds
public class GameStatsManager {

	public int numRounds;
	public List<RoundPlayerObjectManager> roundPlayerData;
	public List<RoundScoreManager> roundScoreData;

	public GameStatsManager(){
		//Do Something
		roundPlayerData = new List<RoundPlayerObjectManager>();
		roundScoreData = new List<RoundScoreManager>();
	}

	public void churnDataIntoStats(){


	}

	public void makeGraph(){

	}
}