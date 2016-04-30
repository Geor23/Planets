using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using ProgressBar;

public class TimerDisplayScript : MonoBehaviour {
    public Text textField;
    float time=99.0F;
    public AudioClip endRoundSound;
    // public GameObject timer;
    private AudioSource source;

	// Use this for initialization
	void Start () {
      if(!NetworkClient.active) {
        this.enabled = false;
        return;
      }
      NetworkManager nm = NetworkManager.singleton;
      nm.client.RegisterHandler(Msgs.sendCurrentTime, OnClientReceiveTime); //Creates a handler for when the client recieves a time from the server
      nm.client.Send(Msgs.requestCurrentTime, new EmptyMessage()); //Requests the current time in-game
      source = GetComponent<AudioSource>();
  } 

    void OnClientReceiveTime(NetworkMessage msg){
       TimeMessage tm = msg.ReadMessage<TimeMessage>();
       time = tm.time;
       textField.text = time.ToString();
       // timer.GetComponent<ProgressBarBehaviour>().SetFillerSize(1);
       // timer.GetComponent<ProgressBarBehaviour>().SetFillerSizeAsPercentage(100);   
    }

    void Update(){
        time -= Time.deltaTime;
        if (time <= 1.0) {
              // source.PlayOneShot(endRoundSound, 1.0F);
        } 
        if (time > 0){
            textField.text = ((int)time).ToString();
            if (time <= 1) {
              GameObject planet = GameObject.Find("Planet");
              planet.GetComponent<Exploder>().expl();
              // Debug.Log("explodin");
            }
            // timer.GetComponent<ProgressBarBehaviour>().UpdateValue((float)time);

            
        }else{
            textField.text = "Round ending...";
        }
    }
}
