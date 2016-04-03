using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class TimerDisplayScript : MonoBehaviour {
    public Text textField;
    float time=99.0F;
    public AudioClip endRoundSound;
    private AudioSource source;

	// Use this for initialization
	void Start () {
      NetworkManager nm = NetworkManager.singleton;
      nm.client.RegisterHandler(Msgs.sendCurrentTime, OnClientReceiveTime); //Creates a handler for when the client recieves a time from the server
      nm.client.Send(Msgs.requestCurrentTime, new EmptyMessage()); //Requests the current time in-game
      source = GetComponent<AudioSource>();
}

    void OnClientReceiveTime(NetworkMessage msg){
       TimeMessage tm = msg.ReadMessage<TimeMessage>();
       time = tm.time;
        Debug.Log(time);
       textField.text = time.ToString();
       
    }

    void Update(){
        time -= Time.deltaTime;
        if (time == 1.0) {
              source.PlayOneShot(endRoundSound, 1.0F);
        } 
        if (time > 0){
            textField.text = time.ToString();
            
        }else{
            textField.text = "Round ending...";
        }
    }
}
