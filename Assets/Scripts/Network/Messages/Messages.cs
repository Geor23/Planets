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
  public const short startGame = 52;      // 
  public const short serverTeamMsg = 53;  // server sends client the team list
  public const short requestTeamMsg = 54; // client sends server a request for the team list
  public const short clientTeamScore = 55;
  public const short serverTeamScore = 56;
  public const short requestTeamScores = 57;
  public const short requestCurrentTime = 58;
  public const short sendCurrentTime = 59;
  public const short serverFinalScores = 60;
  public const short requestFinalScores = 61;
  public const short destroyObjectRequest = 62;
  public const short clientKillFeed = 63;
  public const short serverKillFeed = 64;


}

public class JoinMessage : MessageBase {
  public string name;
  public short playerControllerID = 0;
}

public class TeamChoice : MessageBase {
  public int teamChoice = 0;
}

public class FinalScores : MessageBase {

  public int round1P ;
  public int round1S ;
  public int round2P ;
  public int round2S ;
  public int round3P ;
  public int round3S ;

}

public class Kill : MessageBase {
  public string msg ;
}

public class TeamList : MessageBase {
	public int team ;
	public string teamList;
}

public class AddScore : MessageBase {
	public int score;
	public int team;
    public GameObject obj;
}

public class TeamScore : MessageBase {
  public int team;
  public int score;
}

public class TimeMessage : MessageBase{
    public float time;
}