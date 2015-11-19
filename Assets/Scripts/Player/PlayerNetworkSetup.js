import UnityEngine.Networking;

	var nIdentity:NetworkIdentity;
	var cam1 : Camera;
	
	
	// Use this for initialization
	function Start(){
	if (nIdentity.isLocalPlayer){
			cam1.enabled = true;
		}
	}