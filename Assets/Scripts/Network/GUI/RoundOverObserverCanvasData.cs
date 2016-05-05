using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundOverObserverCanvasData : MonoBehaviour {

    public Text totalKillsP;
    public Text totalKillsS;

    public Text totalDeathsP;
    public Text totalDeathsS;

    public Text mostResourcesP;
    public Text mostResourcesS;

    public Text leastDeathsP;
    public Text leastDeathsS;

    public Text mostKillsP;
    public Text mostKillsS;

    public Text finalResourcesP;
    public Text finalResourcesS;

    public Text winnerTeam;

    public GameStatsManager gsm;

    void Start () {
        gsm = GameStatsManager.singleton;
        displayData();
    }

    public void displayData() {
        int latestRound = gsm.getLatestRound();

        int pirateScore = gsm.getRoundScores(latestRound).getPirateScore();
        int superCorpScore = gsm.getRoundScores(latestRound).getSuperCorpScore();
        finalResourcesP.text = "Total Score:" + pirateScore.ToString();
        finalResourcesS.text = "Total Score:" + superCorpScore.ToString();

        int pirateDeaths = gsm.getRoundPlayerData(latestRound).pirateDeaths;
        int pirateKills = gsm.getRoundPlayerData(latestRound).pirateKills;
        totalDeathsP.text = "Total Deaths: " + pirateDeaths.ToString();
        totalKillsP.text = "Total Kills: " + pirateKills.ToString();

        int superCorpDeaths = gsm.getRoundPlayerData(latestRound).superCorpDeaths;
        int superCorpKills = gsm.getRoundPlayerData(latestRound).superCorpKills;
        totalDeathsS.text = "Total Deaths: " + superCorpDeaths.ToString();
        totalKillsS.text = "Total Kills: " + superCorpKills.ToString();

        string leastDeathsPlayerP = gsm.getRoundPlayerData(latestRound).getPlayerWithLeastDeaths(0).getPlayerName();
        string mostKillsPlayerP = gsm.getRoundPlayerData(latestRound).getPlayerWithMostKills(0).getPlayerName();
        leastDeathsP.text = "Least Deaths: " + leastDeathsPlayerP;
        mostKillsP.text = "Most Kills: " + mostKillsPlayerP;

        string leastDeathsPlayerS = gsm.getRoundPlayerData(latestRound).getPlayerWithLeastDeaths(1).getPlayerName();
        string mostKillsPlayerS = gsm.getRoundPlayerData(latestRound).getPlayerWithMostKills(1).getPlayerName();
        leastDeathsS.text = "Least Deaths: " + leastDeathsPlayerS;
        mostKillsS.text = "Most Kills: " + mostKillsPlayerS;

        if (pirateScore > superCorpScore) {
            winnerTeam.text = "WINNER - PIRATES";
        }
        else if (superCorpScore > pirateScore) {
            winnerTeam.text = "WINNER - SUPER CORP";
        }
        else if (pirateScore == superCorpScore) {
            if (pirateKills > superCorpKills) {
                winnerTeam.text = "WINNER - PIRATES";
            }
            else if (superCorpKills > pirateKills) {
                winnerTeam.text = "WINNER - SUPER CORP";
            }
            else if (superCorpKills == pirateKills) {
                if (pirateDeaths > superCorpDeaths) {
                    winnerTeam.text = "WINNER - SUPER CORP";
                }
                else {
                    winnerTeam.text = "WINNER - PIRATES";
                }
            } 

        }

    } 
}