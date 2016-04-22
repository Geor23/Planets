using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class ServerStartUp : MonoBehaviour {

	public GameObject s;
	void Start(){
		s.GetComponent<StartSceneGUI>().StartDedicatedHost();
	}
	
	void Update(){
	
	}
}
