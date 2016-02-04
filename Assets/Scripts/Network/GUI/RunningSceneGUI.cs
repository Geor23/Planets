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
	int teamA;
	int teamB;
	
	public void Start() {

		nm = NetworkManager.singleton;
		nm.client.RegisterHandler (Msgs.serverTeamScore, OnClientReceiveScores);
		nm.client.Send(Msgs.requestTeamScores, new EmptyMessage());
	}
	
	public void OnClientReceiveScores(NetworkMessage msg) {

		TeamScore tl = msg.ReadMessage<TeamScore>(); 
		if (tl.team == 0) { // if we received team pirates

			teamA = tl.score;
			if (teamA > teamB ) {
				teamAScore.text = " <quad material=1 size=20 x=0.1 y=0.1 width=0.5 height=0.5 /> ";
			} else {
				teamAScore.text = "";
			}
      		//update accordingly
			teamAScore.text += "Team Pirates: " + tl.score.ToString();

		} else if (tl.team == 1) {  // if we received team super-corp 
			teamB = tl.score;
			if (teamA < teamB ) {
				teamBScore.text = " <quad material=1 size=20 x=0.1 y=0.1 width=0.5 height=0.5 /> ";
			} else {
				teamBScore.text = "";
			}
      		// update accordingly
			teamBScore.text += "Team Super-Corp: " + tl.score.ToString();

		} else {

			Debug.LogError("ERROR[OnClientReceiveScores] : Received wrong team");

		}
	}
	
}

