using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

enum TeamID : int {
  TEAM_PIRATES,
  TEAM_SUPERCORP,
  TEAM_OBSERVER
};


class Msgs : MsgType {
  public const short clientJoinMsg = 50;
  public const short clientTeamMsg = 51;
  public const short startGame = 52;
	public const short serverTeamMsg = 53;
}

public class JoinMessage : MessageBase {
  public string name;
  public short playerControllerID = 0;
}

public class TeamChoice : MessageBase {
  public int teamChoice = 0;
}

public class TeamList : MessageBase {
	public int team ;
	public string teamList;
}

public class AddScore : MessageBase {
	public int score;
	public int team;
}
