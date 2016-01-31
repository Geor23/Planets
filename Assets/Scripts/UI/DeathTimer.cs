using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class DeathTimer : MonoBehaviour {
	
	float timeLeft = 3;
	Text deathTimerText;	

	void Start(){
		deathTimerText = GetComponent<Text>();
	}
	
	void Update(){
		if(deathTimerText.enabled == true){
			timeLeft -= Time.deltaTime;
			deathTimerText.text = "0:0" + Mathf.Floor(timeLeft+1).ToString();
			if(timeLeft <= 0){
				deathTimerText.text = "You're Back!";
				ClientScene.AddPlayer(NetworkManager.singleton.client.connection, 0);
			}
		}
	}
}
