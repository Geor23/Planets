using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class RoundOverGUI : MonoBehaviour {

  NetworkManager nm;
	public Text teamP;
	public Text teamS;
  public Text ScoreP;
  public Text ScoreS;
  public Text timer;

  float time=99.0F;
   

  public void Start() {

    nm = NetworkManager.singleton;
	  nm.client.RegisterHandler (Msgs.serverTeamMsg, OnClientReceiveTeamList);
    nm.client.RegisterHandler (Msgs.serverFinalScores, OnClientReceiveScores);
    nm.client.Send(Msgs.requestTeamMsg, new EmptyMessage());
    nm.client.Send(Msgs.requestFinalScores, new EmptyMessage());
    nm.client.RegisterHandler(Msgs.sendCurrentTime, OnClientReceiveTime); //Creates a handler for when the client recieves a time from the server
    nm.client.Send(Msgs.requestCurrentTime, new EmptyMessage()); //Requests the current time in-game 

  }

  void OnClientReceiveTime(NetworkMessage msg){
        TimeMessage tm = msg.ReadMessage<TimeMessage>();
        time = tm.time;
        Debug.Log(time);
        timer.text = time.ToString();
       
    }

    void Update(){
        time -= Time.deltaTime;
        if (time > 0){
            timer.text = time.ToString();
        } else {
            //textField.text = "Round ending...";
        }
    }

	public void OnClientReceiveTeamList(NetworkMessage msg){
		TeamList tl = msg.ReadMessage<TeamList>(); 
		if (tl.team == 0) { // if we received team pirates

      //update accordingly
			teamP.text = tl.teamList;

		} else if (tl.team == 1) {  // if we received team super-corp 

      // update accordingly
			teamS.text = tl.teamList;

		} else {

			Debug.LogError("ERROR[OnClientReceiveTeamList] : Received wrong team ");

		}

	}


  public void OnClientReceiveScores(NetworkMessage msg) {
    FinalScores tl = msg.ReadMessage<FinalScores>(); 
    int pirateCounter = 0;
    int superCorpCounter = 0;

    if (tl.round1P >= tl.round1S) pirateCounter ++;
    else superCorpCounter ++;

    if (tl.round2P >= tl.round2S) pirateCounter ++;
    else superCorpCounter ++;

    if (tl.round3P >= tl.round3S) pirateCounter ++;
    else superCorpCounter ++;

    ScoreP.text = pirateCounter.ToString();
    ScoreS.text = superCorpCounter.ToString();

  }

}