using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoundObserverCanvasData : MonoBehaviour {

	public Text latestKill;
	public Text killFeed;
	public RoundEvents re;


	void Start(){
		re = GameObject.Find("RoundEvents").GetComponent<RoundEvents>();
		InvokeRepeating("UpdateKills", 0, 1);
	}

	void UpdateKills(){
		latestKill.text = re.getRoundPlayerObjectManager().getLatestKill();
		killFeed.text = re.getRoundPlayerObjectManager().getKillsAsList(5); // Show last 5 kills;
	}
}