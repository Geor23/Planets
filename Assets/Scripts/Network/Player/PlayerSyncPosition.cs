using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

[NetworkSettings(channel=1)]
public class PlayerSyncPosition : NetworkBehaviour {
	private Vector3 syncPos;
		
	public NetworkIdentity nIdentity;

	public Transform myTransform;
	public float lerpRate = 15;

	private Vector3 lastPost;
	private float threshold = 0.5f;

	// Update is called once per frame

	void Update(){
		LerpPosition();
	}

	void FixedUpdate () {
		TransmitPosition();
	}
	//Updates location of only other players
	void LerpPosition(){
	    if (!nIdentity.isLocalPlayer) {
	        if (!float.IsNaN(myTransform.position.x) && !float.IsNaN(myTransform.position.y) && !float.IsNaN(myTransform.position.z)){
	            myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);
	        }
		}
	}
	//Command sent to server from client. Called by client, run on server
	[Command]
	void CmdProvidePositionToServer(Vector3 pos){
		Debug.Log("HIIIIII");
		syncPos = pos;
		foreach (NetworkConnection nc in ((PlanetsNetworkManager)PlanetsNetworkManager.singleton).getUpdateListeners()){
			#if UNITY_5_4_OR_NEWER
			TargetUpdatePos(nc, pos);
			#else
			UpdatePos up = new UpdatePos();
			up.netId = nIdentity.netId;
			up.pos = pos;
			NetworkServer.SendToClient(nc.connectionId, Msgs.updatePos, up);
			#endif
		}
	}

	#if UNITY_5_4_OR_NEWER
	[TargetRpc]
	void TargetUpdatePos(NetworkConnection target, Vector3 pos){
		syncPos = pos;
	}
	#else
	public void TargetUpdatePos(UpdatePos up){
		syncPos = up.pos;
	}
	#endif

	[Client]
	void TransmitPosition (){
		if (nIdentity.isLocalPlayer && Vector3.Distance(myTransform.position, lastPost) > threshold) {
			CmdProvidePositionToServer (myTransform.position);
			lastPost = myTransform.position;
		}
	}
}