using UnityEngine;
using System.Collections;

public class PlanetariumDomeControl : MonoBehaviour {
    void Start() {
        DontDestroyOnLoad(transform.gameObject);
        turnOffOtherCameras();
    }

    void turnOffOtherCameras(){
    	GameObject[] cams = GameObject.FindGameObjectsWithTag("MainCamera");
    	foreach(GameObject cam in cams){
    		cam.SetActive(false);
    	}
    }

    void OnLevelWasLoaded(int level) {
        turnOffOtherCameras();
    }

}