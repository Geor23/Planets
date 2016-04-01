import UnityEngine.Networking;

public class PlayerSyncRotationTurret extends NetworkBehaviour {
	@SyncVar private var syncPlayerRotation:Quaternion;

	var nIdentity:NetworkIdentity;
	public var playerTransform:Transform;
	public var lerpRate:float = 15;


	private var lastPlayerRot:Quaternion;
	private var threshold:float = 5;
	
	// Update is called once per frame
	function Update(){
	    if((!nIdentity.isLocalPlayer)||(isServer)) LerpRotations ();
	}


	    function FixedUpdate () {
	        TransmitRotations ();
	    }

	    function LerpRotations (){
	        if (!float.IsNaN(playerTransform.rotation.x) && !float.IsNaN(playerTransform.rotation.y) && !float.IsNaN(playerTransform.rotation.z) && !float.IsNaN(syncPlayerRotation.x)&& !float.IsNaN(syncPlayerRotation.y)&& !float.IsNaN(syncPlayerRotation.z)){
	            playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
	        }
	    }

	@Command
	    function CmdProvideRotationsToServer(playerRot:Quaternion){
	        syncPlayerRotation = playerRot;
	    }
        @ClientCallback
	        function TransmitRotations(){
	            if (nIdentity.isLocalPlayer){
	                if(Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold) {
	                    CmdProvideRotationsToServer(playerTransform.rotation);
	                    lastPlayerRot = playerTransform.rotation;
	                }
	            }
	        }
	    }