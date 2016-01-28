using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class LobbyGUI : MonoBehaviour {

  NetworkManager nm;

  public void Start(){
    nm = NetworkManager.singleton;
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
    nm.client.Send(Msgs.startGame, new TeamChoice());
  }
}