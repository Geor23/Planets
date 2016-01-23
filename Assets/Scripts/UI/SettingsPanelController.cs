using UnityEngine;
using System.Collections;

public class SettingsPanelController : MonoBehaviour {

	public GameObject settingsPanel;
	bool isOpen = false;	

	public void controlSettings(){
		if(isOpen == false){
			settingsPanel.SetActive(true);
			isOpen = true;
		}else if(isOpen == true){
			settingsPanel.SetActive(false);
			isOpen = false;
		}
		}


}
