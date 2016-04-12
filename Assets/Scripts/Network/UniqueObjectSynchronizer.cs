#if UNITY_5_4_OR_NEWER
#else

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class UniqueObjectSynchronizer : NetworkBehaviour {
	NetworkManager nm;

	[Client]
	void Start(){
		nm.client.RegisterHandler(Msgs.updatePos, UpdateLocalPos);
		nm.client.RegisterHandler(Msgs.updateRot, UpdateLocalRot);
		nm.client.RegisterHandler(Msgs.updateRotTurret, UpdateLocalRotTurret);
		nm.client.RegisterHandler(Msgs.fireProjectile, FireProjectile);
	}

	void UpdateLocalPos(NetworkMessage msg){
        var obj = ClientScene.FindLocalObject(msg.ReadMessage<UpdatePos>().netId);
        obj.GetComponent<PlayerSyncPosition>().TargetUpdatePos(msg);
	}

	void UpdateLocalRot(NetworkMessage msg){
        var obj = ClientScene.FindLocalObject(msg.ReadMessage<UpdateRot>().netId);
        obj.GetComponent<PlayerSyncRotation>().TargetUpdateRot(msg);
	}

	void UpdateLocalRotTurret(NetworkMessage msg){
        var obj = ClientScene.FindLocalObject(msg.ReadMessage<UpdateRotTurret>().netId);
        obj.GetComponent<PlayerSyncRotationTurret>().TargetUpdateRot(msg);
	}

	void FireProjectile(NetworkMessage msg){
        var obj = ClientScene.FindLocalObject(msg.ReadMessage<UniqueObjectMessage>().netId);
        obj.GetComponent<UnityStandardAssets.CrossPlatformInput.PlayerControllerMobile>().TargetFireProjectile(msg);
	}
}
#endif