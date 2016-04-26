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
  public GameObject tumorpirate;
  public GameObject tumorsupercorp;
  public GameObject mazepirate;
  public GameObject mazesupercorp;
  public GameObject citypirate;
  public GameObject citysupercorp;

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
      tumorpirate.SetActive(true);
      tumorsupercorp.SetActive(false);
    } else {
      superCorpCounter ++;
      tumorpirate.SetActive(false);
      tumorsupercorp.SetActive(true);
    }

    if (tl.round2P >= tl.round2S) {
      pirateCounter ++;
      mazepirate.SetActive(true);
      mazesupercorp.SetActive(false);
    } else {
      superCorpCounter ++;
      mazepirate.SetActive(false);
      mazesupercorp.SetActive(true);
    }

    if (tl.round3P >= tl.round3S) {
      pirateCounter ++;
      citypirate.SetActive(true);
      citysupercorp.SetActive(false);
    } else {
      superCorpCounter ++;
      citypirate.SetActive(false);
      citysupercorp.SetActive(true);
    }

    ScoreP.text = pirateCounter.ToString();
    ScoreS.text = superCorpCounter.ToString();

  }

}