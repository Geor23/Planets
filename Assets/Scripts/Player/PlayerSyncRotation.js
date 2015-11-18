import UnityEngine.Networking;

//Choose channel and communication
@NetworkSettings(channel=0, sendInterval = 0.033f)
public class PlayerSyncRotation extends NetworkBehaviour {
	@SyncVar private var syncPlayerRotation:Quaternion;
	@SyncVar private var syncCamRotation:Quaternion;

	@SerializeField private var playerTransform:Transform;
	@SerializeField private var camTransform:Transform;
	@SerializeField private var lerpRate:float = 15;


	private var lastPlayerRot:Quaternion;
	private var lastCamRot:Quaternion;
	private var threshold:float = 5;
	
	// Update is called once per frame
	function update(){
		LerpRotations ();
	}


	function FixedUpdate () {
		TransmitRotations ();
	}

	function LerpRotations (){
		playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
		camTransform.rotation = Quaternion.Lerp (camTransform.rotation, syncCamRotation, Time.deltaTime * lerpRate);
	}

	@Command
	function CmdProvideRotationsToServer(playerRot:Quaternion, camRot:Quaternion){
		syncPlayerRotation = playerRot;
		syncCamRotation = camRot;
	}
	@Client
	function TransmitRotations(){
		if (isLocalPlayer){
			if(Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold  || Quaternion.Angle(camTransform.rotation, lastCamRot) > threshold) {
				CmdProvideRotationsToServer(playerTransform.rotation, camTransform.rotation);
				lastPlayerRot = playerTransform.rotation;
				lastCamRot = camTransform.rotation;
			}
		}
	}
}