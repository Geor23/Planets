using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class LobbyGUI : MonoBehaviour {

  NetworkManager nm;
	public Text teamA;
	public Text teamB;

  public void Start() {
    
    nm = NetworkManager.singleton;
	  nm.client.RegisterHandler (Msgs.serverTeamMsg, OnClientReceiveTeamList);
    nm.client.Send(Msgs.requestTeamMsg, new EmptyMessage());

  }

  public void ChooseTeamPirates() {

    TeamChoice tc = new TeamChoice();
    tc.teamChoice = (int) TeamID.TEAM_PIRATES;
    nm.client.Send(Msgs.clientTeamMsg, tc);
    PlayerConfig.singleton.SetObserver(false);

  }

  public void ChooseTeamSuperCorp() {

    TeamChoice tc = new TeamChoice();
    tc.teamChoice =(int) TeamID.TEAM_SUPERCORP;
    nm.client.Send(Msgs.clientTeamMsg, tc);
    PlayerConfig.singleton.SetObserver(false);
    }


  public void StartGame() {

    if(PlayerConfig.singleton.GetObserver()) {
      Debug.Log("Player is observer on start game!");
    }
    nm.client.Send(Msgs.startGame, new EmptyMessage());

  }


	public void OnClientReceiveTeamList(NetworkMessage msg){
		TeamList tl = msg.ReadMessage<TeamList>(); 
    //Debug.Log("bo");
		if (tl.team == 0) { // if we received team pirates
      //Debug.Log("team 00000");
      //update accordingly
			teamA.text = tl.teamList;
      Debug.Log(tl.teamList);

		} else if (tl.team == 1) {  // if we received team super-corp 
      //Debug.Log("team 11");
      // update accordingly
			teamB.text = tl.teamList;

		} else {

			Debug.LogError("ERROR[OnClientReceiveTeamList] : Received wrong team ");

		}

	}

}