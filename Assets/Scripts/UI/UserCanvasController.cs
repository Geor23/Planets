using UnityEngine;
using System.Collections;

public class UserCanvasController : MonoBehaviour {

	public GameObject playerCanvas;
	public GameObject observerCanvas;

	bool isObserver;
	
	void Start () {
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
