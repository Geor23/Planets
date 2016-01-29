using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class LobbyGUI : MonoBehaviour {

  NetworkManager nm;
	public Text teamA;
	public Text teamB;

  public void Start(){
    nm = NetworkManager.singleton;
	nm.client.connection.RegisterHandler (Msgs.serverTeamMsg, OnClientReceiveTeamList);
  }

  public void ChooseTeamPirates(){
    TeamChoice tc = new TeamChoice();
    tc.teamChoice = (int) TeamID.TEAM_PIRATES;
    nm.client.Send(Msgs.clientTeamMsg, tc);
  }

  public void ChooseTeamSuperCorp(){
    TeamChoice tc = new TeamChoice();
    tc.teamChoice =(int) TeamID.TEAM_SUPERCORP;
    nm.client.Send(Msgs.clientTeamMsg, tc);
  }

  public void ChooseObserver(){
    TeamChoice tc = new TeamChoice();
    tc.teamChoice = (int) TeamID.TEAM_OBSERVER;
    nm.client.Send(Msgs.clientTeamMsg, tc);
  }

  public void StartGame(){
    if(PlayerConfig.singleton.isObserver)
      ChooseObserver();
    nm.client.Send(Msgs.startGame, new TeamChoice());
  }
	public void OnClientReceiveTeamList(NetworkMessage msg){
		TeamList tl = msg.ReadMessage<TeamList>(); 
		if (tl.team == 0) {
			teamA.text = tl.teamList;
		} else if (tl.team == 1) {
			teamB.text = tl.teamList;
		} else {
			//error
		}
	}

}