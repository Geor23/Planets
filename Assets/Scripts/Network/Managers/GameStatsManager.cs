using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


//Class to churn all the of the data from all of the rounds
public class GameStatsManager : MonoBehaviour {
 	public static GameStatsManager singleton;
 	public int numRounds;
	public List<RoundPlayerObjectManager> roundPlayerData;
	public List<RoundScoreManager> roundScoreData;

	public void Start(){
    //Do Something
	    singleton = this;
	    DontDestroyOnLoad(this);
	    roundPlayerData = new List<RoundPlayerObjectManager>();
		roundScoreData = new List<RoundScoreManager>();
	}

	public void addNewRoundDatas(RoundPlayerObjectManager rpom, RoundScoreManager rsm){
		roundPlayerData.Add(rpom);
		roundScoreData.Add(rsm);
	}

	public void churnDataIntoStats(){}

	public void makeGraph(){}

	public RoundScoreManager getRoundScores(int round){
		if(round > roundPlayerData.Count) return null;
		return roundScoreData[round-1];
	}

	public RoundPlayerObjectManager getRoundPlayerData(int round){
		if(round > roundPlayerData.Count) return null;
		return roundPlayerData[round-1];
	}

	//Assumes that the data has been populated correctly by Round Events
	public int getLatestRound(){
		return roundPlayerData.Count;
	}

    public void returnRoundScores()
    {
        int latestRound = getLatestRound();
        this.getRoundScores(latestRound);
    }

    public void returnRoundPlayerData()
    {
        int latestRound = getLatestRound();
        this.getRoundPlayerData(latestRound);
    }

    public String returnRoundWinner(int latestRound) {

        int pirateScore = getRoundScores(latestRound).getPirateScore();
        int superCorpScore = getRoundScores(latestRound).getSuperCorpScore();

        int pirateDeaths = getRoundPlayerData(latestRound).pirateDeaths;
        int pirateKills = getRoundPlayerData(latestRound).pirateKills;

        int superCorpDeaths = getRoundPlayerData(latestRound).superCorpDeaths;
        int superCorpKills = getRoundPlayerData(latestRound).superCorpKills;

        if (pirateScore > superCorpScore)
        {
            Debug.Log("WINNER - PIRATES");
            return "PIRATES";
        }
        else if (superCorpScore > pirateScore)
        {
            Debug.Log("WINNER - Super Corp");
            return "SUPER CORP";
        }
        else {
            if (pirateKills > superCorpKills)
            {
                Debug.Log("WINNER - PIRATES");
                return "PIRATES";
            }
            else if (superCorpKills > pirateKills)
            {
                Debug.Log("WINNER - Super Corp");
                return "SUPER CORP";
            }
            else {
                if (pirateDeaths > superCorpDeaths)
                {
                    Debug.Log("WINNER - Super Corp");
                    return "SUPER CORP";
                }
                else
                {
                    Debug.Log("WINNER - PIRATES");
                    return "PIRATES";
                }
            }
        }
    }
}