import UnityEngine.Networking;

public class PlayerSyncRotation extends NetworkBehaviour {
	@SyncVar private var syncPlayerRotation:Quaternion;

	@SerializeField private var playerTransform:Transform;
	@SerializeField private var lerpRate:float = 15;


	private var lastPlayerRot:Quaternion;
	private var threshold:float = 5;
	
	// Update is called once per frame
	function Update(){
	    if(!isLocalPlayer) LerpRotations ();
	}


	    function FixedUpdate () {
	        TransmitRotations ();
	    }

	    function LerpRotations (){
	        playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
	    }

	@Command
	    function CmdProvideRotationsToServer(playerRot:Quaternion){
	        syncPlayerRotation = playerRot;
	    }
        @ClientCallback
	        function TransmitRotations(){
	            if (isLocalPlayer){
	                if(Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold) {
	                    CmdProvideRotationsToServer(playerTransform.rotation);
	                    lastPlayerRot = playerTransform.rotation;
	                }
	            }
	        }
	    }