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
	
	public void Start(){
		nm = NetworkManager.singleton;
	}
	
	public void AddScore(int team, int score){
		AddScore sc = new AddScore();
		sc.team = (int) team;
		sc.score = (int)score;
		nm.client.Send(Msgs.clientTeamScore, sc);
	}
	
}

