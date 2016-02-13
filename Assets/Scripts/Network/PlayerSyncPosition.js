import UnityEngine.Networking;

public class PlayerSyncPosition extends NetworkBehaviour {
	@SyncVar //Synchronises values from server to all clients
	var syncPos:Vector3;
		
	var nIdentity:NetworkIdentity;

	var myTransform:Transform;
	var lerpRate:float = 15;

	private var lastPost:Vector3;
	private var threshold:float = 0.5f;

	// Update is called once per frame
	function Update(){
		LerpPosition();
	}

	function FixedUpdate () {
		TransmitPosition();
	}
//Updates location of only other players
	function LerpPosition(){
	    if (!nIdentity.isLocalPlayer) {
	        if (!float.IsNaN(myTransform.position.x) && !float.IsNaN(myTransform.position.y) && !float.IsNaN(myTransform.position.z)){
	            myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);
	        }
		}
	}
	//Command sent to server from client. Called by client, run on server
@Command
	function CmdProvidePositionToServer(pos:Vector3){
		syncPos = pos;
	}
@ClientCallback
	function TransmitPosition (){
		if (nIdentity.isLocalPlayer && Vector3.Distance(myTransform.position, lastPost) > threshold) {
			CmdProvidePositionToServer (myTransform.position);
			lastPost = myTransform.position;
		}
	}
}