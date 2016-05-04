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
        totalDeathsP.text = pirateDeaths.ToString();
        totalKillsP.text = totalKillsP.ToString();

        int superCorpDeaths = gsm.getRoundPlayerData(latestRound).superCorpDeaths;
        int superCorpKills = gsm.getRoundPlayerData(latestRound).superCorpKills;
        totalDeathsS.text = superCorpDeaths.ToString();
        totalKillsS.text = superCorpKills.ToString();

        string leastDeathsPlayerP = gsm.getRoundPlayerData(latestRound).getPlayerWithLeastDeaths(0).getPlayerName();
        string mostKillsPlayerP = gsm.getRoundPlayerData(latestRound).getPlayerWithMostKills(0).getPlayerName();
        leastDeathsP.text = leastDeathsPlayerP;
        mostKillsP.text = mostKillsPlayerP;

        string leastDeathsPlayerS = gsm.getRoundPlayerData(latestRound).getPlayerWithLeastDeaths(1).getPlayerName();
        string mostKillsPlayerS = gsm.getRoundPlayerData(latestRound).getPlayerWithMostKills(1).getPlayerName();
        leastDeathsS.text = leastDeathsPlayerS;
        mostKillsS.text = mostKillsPlayerS;

    } 

}