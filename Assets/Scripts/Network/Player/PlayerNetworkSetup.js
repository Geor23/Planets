import UnityEngine.Networking;

var nIdentity : NetworkIdentity;
var cam1 : Camera;
var cam2 : Camera;

// Use this for initialization
function Start(){
	cam1.gameObject.SetActive(false);
	cam2.gameObject.SetActive(false);
	if (nIdentity.isLocalPlayer){
		cam1.gameObject.SetActive(true);
		cam2.gameObject.SetActive(true);
	}
}

/*function OnDestroy(){
	if(!nIdentity.isLocalPlayer) return;
	Debug.LogError("I was killed!!????");
    Debug.LogError(UnityEngine.StackTraceUtility.ExtractStackTrace ());
    ClientScene.RemovePlayer(0);
    ClientScene.AddPlayer(NetworkManager.singleton.client.connection, 0);
}*/