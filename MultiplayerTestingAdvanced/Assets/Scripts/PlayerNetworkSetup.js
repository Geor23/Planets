import UnityEngine.Networking;

//public class PlayerNetworkSetup : NetworkBehaviour {
	var nIdentity:NetworkIdentity;
	var FPSCharacterCam:Camera;
	var audioListener:AudioListener;
	// Use this for initialization
	function Start(){
	if (nIdentity.isLocalPlayer){
			GameObject.Find ("Main Camera").SetActive (false);
			//GetComponent<CharacterController>().enabled = true;
			GetComponent(UnityStandardAssets.Characters.FirstPerson.FirstPersonController).enabled=true;
			FPSCharacterCam.enabled=true;
			audioListener.enabled = true;
		}
	}
//}