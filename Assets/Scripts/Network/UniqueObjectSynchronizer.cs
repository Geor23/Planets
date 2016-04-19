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
		nm = NetworkManager.singleton;
		nm.client.RegisterHandler(Msgs.updatePos, UpdateLocalPos);
		nm.client.RegisterHandler(Msgs.updateRot, UpdateLocalRot);
		nm.client.RegisterHandler(Msgs.updateRotTurret, UpdateLocalRotTurret);
		nm.client.RegisterHandler(Msgs.fireProjectile, FireProjectile);
	}


	void UpdateLocalPos(NetworkMessage msg){
		UpdatePos up = msg.ReadMessage<UpdatePos>();
        GameObject obj = ClientScene.FindLocalObject(up.netId);
        obj.GetComponent<PlayerSyncPosition>().TargetUpdatePos(up);
	}

	void UpdateLocalRot(NetworkMessage msg){
		UpdateRot ur = msg.ReadMessage<UpdateRot>();
        GameObject obj = ClientScene.FindLocalObject(ur.netId);
        obj.GetComponent<PlayerSyncRotation>().TargetUpdateRot(ur);
	}

	void UpdateLocalRotTurret(NetworkMessage msg){
		UpdateRotTurret ur = msg.ReadMessage<UpdateRotTurret>();
        GameObject obj = ClientScene.FindLocalObject(ur.netId);
        obj.GetComponent<PlayerSyncRotationTurret>().TargetUpdateRot(ur);
	}

	void FireProjectile(NetworkMessage msg){
		UniqueObjectMessage fp = msg.ReadMessage<UniqueObjectMessage>();
        GameObject obj = ClientScene.FindLocalObject(fp.netId);
        obj.GetComponent<UnityStandardAssets.CrossPlatformInput.PlayerControllerMobile>().TargetFireProjectile(fp);
	}
}
#endif