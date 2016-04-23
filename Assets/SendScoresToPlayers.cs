using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class SendScoresToPlayers : MonoBehaviour {


    public PlanetsNetworkManager nm;
	// Use this for initialization
	void Start () {
        if (PlayerConfig.singleton.GetObserver()) {
            nm = (PlanetsNetworkManager)NetworkManager.singleton;
            nm.sendScoresToPlayers();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
