using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class RunningSceneGUI : MonoBehaviour {
	
	NetworkManager nm;
	public Text teamAScore;
	public Text teamBScore;
	
	public void Start() {

		nm = NetworkManager.singleton;
		nm.client.RegisterHandler (Msgs.serverTeamScore, OnClientReceiveScores);

	}
	
	public void OnClientReceiveScores(NetworkMessage msg) {

		TeamScore tl = msg.ReadMessage<TeamScore>(); 
		if (tl.team == 0) { // if we received team pirates

      		//update accordingly
			teamAScore.text = "Team Pirates: " + tl.score.ToString();

		} else if (tl.team == 1) {  // if we received team super-corp 

      		// update accordingly
			teamBScore.text = "Team Super-Corp: " + tl.score.ToString();

		} else {

			Debug.LogError("ERROR[OnClientReceiveScores] : Received wrong team");

		}
	}
	
}

