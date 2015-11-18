import UnityEngine.Networking;

	var nIdentity:NetworkIdentity;
	var cam1 : Camera;
	var audioListener:AudioListener;
	
	
	// Use this for initialization
	function Start(){
	if (nIdentity.isLocalPlayer){
			GameObject.Find ("Camera").SetActive (false);
			GetComponent("PlayerController").enabled = true;
			cam1.enabled = true;
			audioListener.enabled = true;
		}
	}