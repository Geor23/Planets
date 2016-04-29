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
  	public Text scoreText;
	public Image piratesWin;
	public Image superCorpWin;

	public Image idImgPir;
	public Image idImgSup;

	
	public Text killFeed;

	int teamA;
	int teamB;
	
	public void Start() {
		if(!NetworkClient.active) {
	  		this.enabled = false;
	  		return;
  		}
		nm = NetworkManager.singleton;
		nm.client.RegisterHandler (Msgs.serverTeamScore, OnClientReceiveScores);
		nm.client.RegisterHandler (Msgs.serverKillFeed, OnClientReceiveKillFeed);

    }
	
    public void OnClientReceiveKillFeed(NetworkMessage msg) {
		Kill tl = msg.ReadMessage<Kill>(); 
		killFeed.text = tl.msg;
	}


	public void OnClientReceiveScores(NetworkMessage msg) {

		TeamScore tl = msg.ReadMessage<TeamScore>(); 
		if (tl.team == TeamID.TEAM_PIRATES) { // if we received team pirates

			teamA = tl.score;
			if (teamA > teamB ) {
                piratesWin.gameObject.SetActive(true);
				superCorpWin.gameObject.SetActive(false);
			} else {
				piratesWin.gameObject.SetActive(false);
				superCorpWin.gameObject.SetActive(true);
			}
			teamAScore.text = tl.score.ToString();

		} else if (tl.team == TeamID.TEAM_SUPERCORP) {  // if we received team super-corp 
			teamB = tl.score;
			if (teamA < teamB ) {
				piratesWin.gameObject.SetActive(false);
				superCorpWin.gameObject.SetActive(true);
			} else {
				piratesWin.gameObject.SetActive(true);
				superCorpWin.gameObject.SetActive(false);
			}
      		// update accordingly
			teamBScore.text = tl.score.ToString();

		} else {

			Debug.LogError("ERROR[OnClientReceiveScores] : Received wrong team");

		}
	}
	
}

