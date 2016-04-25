using UnityEngine;
using System.Collections;

public class PlanetariumOnly : MonoBehaviour {

	public void Start(){
		GameObject[] controllers = GameObject.FindGameObjectsWithTag("PlanetariumController");

		if(controllers.Length == 0) {
			gameObject.SetActive(false);
		} else {
			// if(!controllers[0].GetComponent<PlanetariumDomeControl>().enablePlanetariumMode){
			// 	gameObject.SetActive(false);
			// }
		}
	}
}