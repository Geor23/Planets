using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class GameOverGUI : MonoBehaviour {

  NetworkManager nm;
	public Text teamP;
	public Text teamS;
  public Text ScoreP;
  public Text ScoreS;
  public GameObject tumor-pirate;
  public GameObject tumor-supercorp;
  public GameObject maze-pirate;
  public GameObject maze-supercorp;
  public GameObject city-pirate;
  public GameObject city-supercorp;

  public void Start() {

    nm = NetworkManager.singleton;
	  nm.client.RegisterHandler (Msgs.serverTeamMsg, OnClientReceiveTeamList);
    nm.client.RegisterHandler (Msgs.serverFinalScores, OnClientReceiveScores);
    nm.client.Send(Msgs.requestTeamMsg, new EmptyMessage());
    nm.client.Send(Msgs.requestFinalScores, new EmptyMessage());

  }

	public void OnClientReceiveTeamList(NetworkMessage msg){
		TeamList tl = msg.ReadMessage<TeamList>(); 
		if (tl.team == TeamID.TEAM_PIRATES) { // if we received team pirates
			teamP.text = tl.teamList;
		} else if (tl.team == TeamID.TEAM_SUPERCORP) {  // if we received team super-corp 
			teamS.text = tl.teamList;
		} else {
			Debug.LogError("ERROR[OnClientReceiveTeamList] : Received wrong team ");
		}
	}


  public void OnClientReceiveScores(NetworkMessage msg) {
    FinalScores tl = msg.ReadMessage<FinalScores>(); 
    int pirateCounter = 0;
    int superCorpCounter = 0;

    if (tl.round1P >= tl.round1S) {
      pirateCounter ++;
      tumor-pirate.SetActive(true);
      tumor-supercorp.SetActive(false);
    } else {
      superCorpCounter ++;
      tumor-pirate.SetActive(false);
      tumor-supercorp.SetActive(true);
    }

    if (tl.round2P >= tl.round2S) {
      pirateCounter ++;
      maze-pirate.SetActive(true);
      maze-supercorp.SetActive(false);
    } else {
      superCorpCounter ++;
      maze-pirate.SetActive(false);
      maze-supercorp.SetActive(true);
    }

    if (tl.round3P >= tl.round3S) {
      pirateCounter ++;
      city-pirate.SetActive(true);
      city-supercorp.SetActive(false);
    } else {
      superCorpCounter ++;
      city-pirate.SetActive(false);
      city-supercorp.SetActive(true);
    }

    ScoreP.text = pirateCounter.ToString();
    ScoreS.text = superCorpCounter.ToString();

  }

}