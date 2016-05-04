using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundOverObserverCanvasData : MonoBehaviour {

    public Text killsP;
    public Text killsS;
    public Text deathsP;
    public Text deathsS;
    public Text mostResourcesP;
    public Text mostResourcesS;
    public Text leastDeathsP;
    public Text leastDeathsS;
    public Text finalResourcesP;
    public Text finalResourcesS;

    public GameStatsManager gsm;

    void Start () {
        gsm = GameStatsManager.singleton;

    }

    public void displayData() {
        int latestRound = gsm.getLatestRound();
        int pirateScore = gsm.getRoundScores(latestRound).getPirateScore();
        int superCorpScore = gsm.getRoundScores(latestRound).getSuperCorpScore();
        finalResourcesP.text = pirateScore.ToString();
        finalResourcesS.text = superCorpScore.ToString();

        int pirateDeaths = gsm.getRoundPlayerData(latestRound).pirateDeaths;
        int pirateKills = gsm.getRoundPlayerData(latestRound).pirateKills;

        int superCorpDeaths = gsm.getRoundPlayerData(latestRound).superCorpDeaths;
        int superCOrpKills = gsm.getRoundPlayerData(latestRound).superCorpKills;

        string leastDeathsPlayerP = gsm.getRoundPlayerData(latestRound).getPlayerWithLeastDeaths(0).getPlayerName();
        string mostKillsPlayerP = gsm.getRoundPlayerData(latestRound).getPlayerWithMostKills(0).getPlayerName();

        string leastDeathsPlayerS = gsm.getRoundPlayerData(latestRound).getPlayerWithLeastDeaths(1).getPlayerName();
        string mostKillsPlayerS = gsm.getRoundPlayerData(latestRound).getPlayerWithMostKills(1).getPlayerName();


    } 

}