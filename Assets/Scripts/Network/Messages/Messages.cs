using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

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
  public const short deathResourceCollision = 65;
  public const short updateLocalScore = 66;
  public const short requestName = 67;
  public const short serverName = 68;
  public const short updatePos = 69;
  public const short updateRot = 70;
  public const short updateRotTurret = 71;
  public const short fireProjectile = 72;
  public const short killPlayer = 73;
  public const short killPlayerRequestClient = 74;
  public const short updatePlayer = 75;
  public const short addNewPlayer = 76;
  public const short spawnPlayer = 77;
  public const short addNewPlayerToObserver = 78;
  public const short updatePlayerToObserver = 79;
  public const short spawnSelf = 80;
  public const short givePlayerScores = 81;
  public const short sendRoundOverValuesToPlayer = 82;
  public const short playerReadyToSpawn = 83;
  public const short ping = 84;
}

public class JoinMessage : MessageBase {
  public string name;
  //public short playerControllerID = 0;
  public int team;
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

public class Name : MessageBase {
  public string name;
  public int id;
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

public class DeathResource : MessageBase{
    public int team;
    public int score;
    public GameObject drID;
}

public class UpdateLocalScore : MessageBase {
    public int score;
}

public class UniqueObjectMessage : MessageBase {
    public NetworkInstanceId netId;
}

public class UpdatePos : MessageBase {
    public NetworkInstanceId netId;
    public Vector3 pos;
}

public class UpdateRot : MessageBase {
    public NetworkInstanceId netId;
    public Quaternion rot;
}

public class UpdateRotTurret : MessageBase {
    public NetworkInstanceId netId;
    public Quaternion rot;
}

public class FireProjectile : MessageBase {
    public NetworkInstanceId netId;
    public Quaternion turretRot;
}

public class KillPlayer : MessageBase {
    public NetworkInstanceId netId;
    public GameObject obj;
}

public class PlayerValues : MessageBase {
    public int dictId;
    public int oldId;
    public int connVal;
    public string playerIP;
    public string playerName;
    public int playerTeam;
    public int deaths;
    public int kills;
    public int scoreAcc;
    public int scoreTotal;
    public int scoreRound;
}

public class PlayerSpawnMsg : MessageBase {
    public int playerId;
    public Vector3 pos;
    public Quaternion rot;
}