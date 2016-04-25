using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ProgressBar;

public class RoundObserverCanvasData : MonoBehaviour {

	public Text latestKill;
	public Text killFeed;
	public Text pirateScore;
	public Text superCorpScore;
	public GameObject piratesBar;
	public GameObject superCorpBar;

	// public GameObject piratesWinning;
	// public GameObject superCorpWinning;
	public RoundEvents re;


	void Start(){
		re = GameObject.Find("RoundEvents").GetComponent<RoundEvents>();
		if(re == null) {
			Debug.LogError("Couldn't find RoundEvents!");
		}
		InvokeRepeating("UpdateKills", 0, 1);
		piratesBar.GetComponent<ProgressBarBehaviour>().SetFillerSize(0.5f);
        piratesBar.GetComponent<ProgressBarBehaviour>().SetFillerSizeAsPercentage(50);    
		superCorpBar.GetComponent<ProgressBarBehaviour>().SetFillerSize(0.5f);
        superCorpBar.GetComponent<ProgressBarBehaviour>().SetFillerSizeAsPercentage(50);   

	}

	void UpdateKills(){
		int pirateScoreInt = re.getRoundScoreManager().getPirateScore();
		int superCorpScoreInt = re.getRoundScoreManager().getSuperCorpScore();
		latestKill.text = re.getRoundPlayerObjectManager().getLatestKill();
		killFeed.text = re.getRoundPlayerObjectManager().getKillsAsList(5); // Show last 5 kills;
		pirateScore.text = pirateScoreInt.ToString();
		superCorpScore.text = superCorpScoreInt.ToString();

		// piratesBar.GetComponent<ProgressBarBehaviour>().UpdateValue((float)time);

		// if(pirateScoreInt > superCorpScoreInt){
		// 	piratesWinning.SetActive(true);
		// 	superCorpWinning.SetActive(false);
		// }
		// if(pirateScoreInt < superCorpScoreInt){
		// 	piratesWinning.SetActive(false);
		// 	superCorpWinning.SetActive(true);	
		// }
	}
}