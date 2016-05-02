#if UNITY_5_4_OR_NEWER
#else

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class UniqueObjectSynchronizer : MonoBehaviour {
	NetworkManager nm;

	void Start(){
        nm = NetworkManager.singleton;
        if(NetworkClient.active){
    		nm.client.RegisterHandler(Msgs.updatePos, UpdateLocalPos);
    		nm.client.RegisterHandler(Msgs.updateRot, UpdateLocalRot);
    		nm.client.RegisterHandler(Msgs.updateRotTurret, UpdateLocalRotTurret);
    		nm.client.RegisterHandler(Msgs.fireProjectile, FireProjectile);
            nm.client.RegisterHandler(Msgs.ping, PingPlayer);
        }
        if(NetworkServer.active){
            NetworkServer.RegisterHandler(Msgs.ping, OnServerRecievePing);
        }
	}

    /* CLIENT FUNZ */

	void UpdateLocalPos(NetworkMessage msg){
		UpdatePos up = msg.ReadMessage<UpdatePos>();
        GameObject obj = ClientScene.FindLocalObject(up.netId);
        if(obj == null) {
        	Debug.LogError("UpdateLocalPos: GameObject with netId " + up.netId + " could not be found");
        	return;
        }
        obj.GetComponent<PlayerSyncPosition>().TargetUpdatePos(up);
	}

	void UpdateLocalRot(NetworkMessage msg){
		UpdateRot ur = msg.ReadMessage<UpdateRot>();
        GameObject obj = ClientScene.FindLocalObject(ur.netId);
        if(obj == null) {
        	Debug.LogError("UpdateLocalRot: GameObject with netId " + ur.netId + " could not be found");
       		return;
        }
        obj.GetComponent<PlayerSyncRotation>().TargetUpdateRot(ur);
	}

	void UpdateLocalRotTurret(NetworkMessage msg){
		UpdateRotTurret ur = msg.ReadMessage<UpdateRotTurret>();
        GameObject obj = ClientScene.FindLocalObject(ur.netId);
        if(obj == null) {
        	Debug.LogError("UpdateLocalRotTurret: GameObject with netId " + ur.netId + " could not be found");
       		return;
        }
        obj.GetComponent<PlayerSyncRotationTurret>().TargetUpdateRot(ur);
	}

	void FireProjectile(NetworkMessage msg){
		FireProjectile fp = msg.ReadMessage<FireProjectile>();
        GameObject obj = ClientScene.FindLocalObject(fp.netId);
        if(obj == null) {
        	Debug.LogError("FireProjectile: GameObject with netId " + fp.netId + " could not be found");
        	return;
        }
        obj.GetComponent<PlayerControllerMobile>().TargetFireProjectile(fp);
	}

    void PingPlayer(NetworkMessage msg){
        UniqueObjectMessage m = msg.ReadMessage<UniqueObjectMessage>();
        GameObject obj = ClientScene.FindLocalObject(m.netId);
        if(obj == null){
            Debug.LogError("PingPlayer: GameObject with netId " + m.netId + " could not be found.");
            return;
        }
        obj.GetComponent<PlayerControllerMobile>().Ping();
    }



    /* SERVER FUNZ */

    void OnServerRecievePing(NetworkMessage msg){
        foreach (NetworkConnection nc in ((PlanetsNetworkManager)PlanetsNetworkManager.singleton).getUpdateListeners()){
            NetworkServer.SendToClient(nc.connectionId, Msgs.ping, msg.ReadMessage<UniqueObjectMessage>());
        }
    }

}
#endif