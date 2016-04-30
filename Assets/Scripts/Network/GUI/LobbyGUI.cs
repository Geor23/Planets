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
    if(NetworkServer.active && !NetworkClient.active) return;
    nm = NetworkManager.singleton;
	  nm.client.RegisterHandler (Msgs.serverTeamMsg, OnClientReceiveTeamList);
    nm.client.Send(Msgs.requestTeamMsg, new EmptyMessage());

  }

  public void ChooseTeamPirates() {

    TeamChoice tc = new TeamChoice();
    tc.teamChoice = (int) TeamID.TEAM_PIRATES;
    nm.client.Send(Msgs.clientTeamMsg, tc);
    PlayerConfig.singleton.SetTeam(0);

  }

  public void ChooseTeamSuperCorp() {

    TeamChoice tc = new TeamChoice();
    tc.teamChoice =(int) TeamID.TEAM_SUPERCORP;
    nm.client.Send(Msgs.clientTeamMsg, tc);
    PlayerConfig.singleton.SetTeam(1);
  }


  public void StartGame() {
    GameObject.Find("FadeTexture").GetComponent<SceneFadeInOut>().EndScene();
    nm.client.Send(Msgs.startGame, new EmptyMessage());
  }


	public void OnClientReceiveTeamList(NetworkMessage msg){
		TeamList tl = msg.ReadMessage<TeamList>(); 
		if (tl.team == TeamID.TEAM_PIRATES) { 
			teamA.text = tl.teamList;
		} else if (tl.team == TeamID.TEAM_SUPERCORP) {  // if we received team super-corp 
			teamB.text = tl.teamList;
		} else {
			Debug.LogError("ERROR[OnClientReceiveTeamList] : Received wrong team ");

		}

	}

	void Update () {
		if (Input.GetKeyDown("s")) {
			StartGame();
		}
	}

}