using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using ProgressBar;

public class RoundOverGUI : MonoBehaviour {

  NetworkManager nm;
	public Text teamP;
	public Text teamS;
  public Text ScoreP;
  public Text ScoreS;
  public Text timer;
  public Text kills;
  public Text deaths;
  public Text mostScores;
  public Text finalScores;
  public GameObject timerRadial;

  public float time=10.0F;
  private float lastUpdatedTime;
   

  public void Start() {
    if(!NetworkClient.active) {
      this.enabled = false;
      return;
    }

    nm = NetworkManager.singleton;
	  nm.client.RegisterHandler (Msgs.serverTeamMsg, OnClientReceiveTeamList);
    nm.client.RegisterHandler (Msgs.serverFinalScores, OnClientReceiveScores);
    nm.client.Send(Msgs.requestTeamMsg, new EmptyMessage());
    // nm.client.Send(Msgs.requestFinalScores, new EmptyMessage());
    nm.client.RegisterHandler(Msgs.sendCurrentTime, OnClientReceiveTime); //Creates a handler for when the client recieves a time from the server
    nm.client.Send(Msgs.requestCurrentTime, new EmptyMessage()); //Requests the current time in-game 
    nm.client.RegisterHandler(Msgs.sendRoundOverValuesToPlayer, OnPlayerRecievePlayerScore);

  }

  void OnClientReceiveTime(NetworkMessage msg){
        TimeMessage tm = msg.ReadMessage<TimeMessage>();
        time = tm.time;
        lastUpdatedTime = time;
        timer.text = time.ToString();
        timerRadial.GetComponent<ProgressRadialBehaviour>().SetFillerSize(1);
        timerRadial.GetComponent<ProgressRadialBehaviour>().SetFillerSizeAsPercentage(100);
  }

  void Update(){
      time -= Time.deltaTime;
      if (time > 0){
          timer.text = ((int)time).ToString();
          if (lastUpdatedTime - time >= 1.0f) {
            lastUpdatedTime = time;
            timerRadial.GetComponent<ProgressRadialBehaviour>().DecrementValue(10);
          }

      }
  }

   public void OnPlayerRecievePlayerScore(NetworkMessage msg){
        Debug.Log(" goooooot it ");
        PlayerValues pv = msg.ReadMessage<PlayerValues>();
        kills.text = "Kills : " + pv.kills.ToString();
        Debug.Log("Kills is " + pv.kills);
        deaths.text = "Deaths : " + pv.deaths.ToString();
        Debug.Log("Deaths is " + pv.deaths);
        mostScores.text = "Total score : " + pv.scoreTotal.ToString();
        Debug.Log("Score Total is " + pv.scoreTotal);
        Debug.Log("Score Acc is " + pv.scoreAcc);
        finalScores.text = "Final scores : " + pv.scoreRound.ToString();
        Debug.Log("ScoreRound is " + pv.scoreRound);
    }

	public void OnClientReceiveTeamList(NetworkMessage msg){
		// TeamList tl = msg.ReadMessage<TeamList>(); 
		// if (tl.team == TeamID.TEAM_PIRATES) {
		// 	teamP.text = tl.teamList;

		// } else if (tl.team == TeamID.TEAM_SUPERCORP) {
		// 	teamS.text = tl.teamList;
		// } else {
		// 	Debug.LogError("ERROR[OnClientReceiveTeamList] : Received wrong team ");
		// }
	}


  public void OnClientReceiveScores(NetworkMessage msg) {
    // FinalScores tl = msg.ReadMessage<FinalScores>(); 
    // int pirateCounter = 0;
    // int superCorpCounter = 0;

    // if (tl.round1P >= tl.round1S) pirateCounter ++;
    // else superCorpCounter ++;
    
    
    // if ((tl.round2P != -1)) {
    //   if (tl.round2P >= tl.round2S) pirateCounter ++;
    //   else superCorpCounter ++;
    // }
      
    // if ((tl.round3P != -1)) {
    //   if (tl.round3P >= tl.round3S) pirateCounter ++;
    //   else superCorpCounter ++;
    // }
    
    // ScoreP.text = pirateCounter.ToString();
    // ScoreS.text = superCorpCounter.ToString();

  }

}