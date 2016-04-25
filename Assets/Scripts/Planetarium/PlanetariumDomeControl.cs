using UnityEngine;
using System.Collections;

public class PlanetariumDomeControl : MonoBehaviour {

    public bool enablePlanetariumMode = false;

    void Start() {
        if(!enablePlanetariumMode) {
            GetComponent<Domemaster>().enabled = false;
            return;
        }
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