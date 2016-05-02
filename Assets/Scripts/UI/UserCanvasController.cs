using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class UserCanvasController : MonoBehaviour {

	public GameObject playerCanvas;
	public GameObject observerCanvas;

	bool isObserver;
	
	void Start () {
		if(!NetworkClient.active) return;
		isObserver = PlayerConfig.singleton.GetObserver();
	}
	
	void FixedUpdate(){
		if(isObserver == true){
			playerCanvas.SetActive(false);
			observerCanvas.SetActive(true);
		} else if(isObserver == false){
			playerCanvas.SetActive(true);
			observerCanvas.SetActive(false);
		}
	}
}
