using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class DeathTimer : MonoBehaviour {
	
	float timeLeft = 3;
	Text deathTimerText;
	public Text deathText;

	void Start(){
		deathTimerText = GetComponent<Text>();
	}
	
	void Update(){
		if(deathTimerText.enabled == true){
			timeLeft -= Time.deltaTime;
			deathTimerText.text = "0:0" + Mathf.Floor(timeLeft+1).ToString();
			if(!(timeLeft > 0)){
				deathTimerText.enabled = false;
				deathText.enabled = false;
				ClientScene.AddPlayer(NetworkManager.singleton.client.connection, 0);
				GameObject.Find("MainCamera").GetComponent<Camera>().enabled = false;
				timeLeft = 3;
			}
		}
	}
}
