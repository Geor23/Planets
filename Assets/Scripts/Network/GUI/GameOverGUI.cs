using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class GameOverGUI : MonoBehaviour {

    //NetworkManager nm;
    //public Text teamP;
    //public Text teamS;

    public Text totalScoreP;
    public Text totalScoreS;

    public Text totalKillsP;
    public Text totalKillsS;

    public Text totalDeathsP;
    public Text totalDeathsS;

    public Text winnerTeam;

    public GameObject tumorPirate;
    public GameObject tumorSuperCorp;
    public GameObject mazePirate;
    public GameObject mazeSuperCorp;
    public GameObject cityPirate;
    public GameObject citySuperCorp;

    public GameStatsManager gsm;

    public void Start() {
        gsm = GameStatsManager.singleton;
        displayData();
    }

    public void displayData() {
        int latestRound = gsm.getLatestRound();
        calculateGameOverData(latestRound);
    }

    public void calculateGameOverData(int latestRound) {
        int pirateFinalScore = 0;
        int pirateKills = 0;
        int pirateDeaths = 0;

        int superCorpFinalScore = 0;
        int superCorpKills = 0;
        int superCorpDeaths = 0;

        int pirateWins = 0;
        int superCorpWins = 0;

        string roundWinner;

        tumorPirate.SetActive(false);
        tumorSuperCorp.SetActive(false);

        mazePirate.SetActive(false);
        mazeSuperCorp.SetActive(false);

        cityPirate.SetActive(false);
        citySuperCorp.SetActive(false); 

        for (int i = latestRound; i>0; i-- ) {

            pirateFinalScore += gsm.getRoundScores(i).getPirateScore();
            Debug.Log("Pirate Final Score Round " + i  + pirateFinalScore);
            pirateKills += gsm.getRoundPlayerData(i).pirateKills;
            Debug.Log("Pirate Kills Round " + i + pirateKills);
            pirateDeaths += gsm.getRoundPlayerData(i).pirateDeaths;
            Debug.Log("Pirate Deaths Round " + i + pirateDeaths);

            superCorpFinalScore += gsm.getRoundScores(i).getSuperCorpScore();
            Debug.Log("Super Corp Final Score Round " + i + superCorpFinalScore);
            superCorpKills += gsm.getRoundPlayerData(i).superCorpKills;
            Debug.Log("Super Corp Kills " + i + superCorpKills);
            superCorpDeaths += gsm.getRoundPlayerData(i).superCorpDeaths;
            Debug.Log("Super Corp Deaths " + i + superCorpDeaths);

            roundWinner = gsm.returnRoundWinner(i);
            Debug.Log(roundWinner);
            if (roundWinner == "PIRATES") {
                pirateWins += 1;
                switch (i) {
                    case 1:
                        tumorPirate.SetActive(true);
                        tumorSuperCorp.SetActive(false);
                        break;
                    case 2:
                        mazePirate.SetActive(true);
                        mazeSuperCorp.SetActive(false);
                        break;
                    case 3:
                        cityPirate.SetActive(true);
                        citySuperCorp.SetActive(false);
                        break;
                }
            }
            else {
                superCorpWins += 1;
                switch (i) {
                    case 1:
                        tumorPirate.SetActive(false);
                        tumorSuperCorp.SetActive(true);
                        break;
                    case 2:
                        mazePirate.SetActive(false);
                        mazeSuperCorp.SetActive(true);
                        break;
                    case 3:
                        cityPirate.SetActive(false);
                        citySuperCorp.SetActive(true);
                        break;
                }
            }
        }

        totalScoreP.text = "Total Resources: " + pirateFinalScore.ToString();
        totalKillsP.text = "Total Kills: " + pirateKills.ToString();
        totalDeathsP.text = "Total Deaths: " + pirateDeaths.ToString();

        totalScoreS.text = "Total Resources: " + superCorpFinalScore.ToString();
        totalKillsS.text = "Total Kills: " + superCorpKills.ToString();
        totalDeathsS.text = "Total Deaths: " + superCorpDeaths.ToString();

        if (pirateWins > superCorpWins) {
            winnerTeam.text = "WINNER PIRATES";
        }
        else {
            winnerTeam.text = "WINNER SUPER CORP";
        }
    }
}