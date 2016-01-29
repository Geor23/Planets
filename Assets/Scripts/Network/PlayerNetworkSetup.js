import UnityEngine.Networking;

	var nIdentity:NetworkIdentity;
	var cam1 : Camera;
	
	
	// Use this for initialization
	function Start(){
		Debug.Log(nIdentity.isLocalPlayer);
		if (nIdentity.isLocalPlayer){
			cam1.gameObject.SetActive(true);
		}
		Debug.Log(cam1.enabled);
	}