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

    public string returnRoundWinner(int roundNumber) {

        int pirateScore = getRoundScores(roundNumber).getPirateScore();
        int superCorpScore = getRoundScores(roundNumber).getSuperCorpScore();

        int pirateDeaths = getRoundPlayerData(roundNumber).pirateDeaths;
        int pirateKills = getRoundPlayerData(roundNumber).pirateKills;

        int superCorpDeaths = getRoundPlayerData(roundNumber).superCorpDeaths;
        int superCorpKills = getRoundPlayerData(roundNumber).superCorpKills;

        if (pirateScore > superCorpScore) {
            return "PIRATES";
        }
        else if (superCorpScore > pirateScore) {
            return "SUPER CORP";
        }
        else {
            if (pirateKills > superCorpKills) {
                return "PIRATES";
            }
            else if (superCorpKills > pirateKills) {
                return "SUPER CORP";
            }
            else {
                if (pirateDeaths > superCorpDeaths) {
                    return "SUPER CORP";
                }
                else {
                    return "PIRATES";
                }
            }
        }
    }
}