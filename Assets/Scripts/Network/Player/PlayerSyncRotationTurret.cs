using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

[NetworkSettings(channel=0)]
public class PlayerSyncRotationTurret : NetworkBehaviour {
    private Quaternion syncPlayerRotation;

    public NetworkIdentity nIdentity;
    public Transform playerTransform;
    public float lerpRate = 15;
    public bool updateAll = false;


    private Quaternion lastPlayerRot;
    private float threshold = 5;
    
    // Update is called once per frame
    void Update(){
        if((!nIdentity.isLocalPlayer)||(isServer)) LerpRotations ();
    }


    void FixedUpdate () {
        TransmitRotations ();
    }

    void LerpRotations (){
        if ((!float.IsNaN(playerTransform.rotation.w)) && !float.IsNaN(playerTransform.rotation.x) && !float.IsNaN(playerTransform.rotation.y) && !float.IsNaN(playerTransform.rotation.z) && !float.IsNaN(syncPlayerRotation.x)&& !float.IsNaN(syncPlayerRotation.y)&& !float.IsNaN(syncPlayerRotation.z) && (!float.IsNaN(syncPlayerRotation.w))){
            playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdProvideRotationsToServer(Quaternion playerRot){
        syncPlayerRotation = playerRot;
        foreach (NetworkConnection nc in ((PlanetsNetworkManager)PlanetsNetworkManager.singleton).getUpdateListeners(updateAll)){
            #if UNITY_5_4_OR_NEWER
            TargetUpdateRotation(nc, playerRot);
            #else
            UpdateRotTurret ur = new UpdateRotTurret();
            ur.netId = nIdentity.netId;
            ur.rot = playerRot;
            NetworkServer.SendToClient(nc.connectionId, Msgs.updateRotTurret, ur);
            #endif
        }
    }

    #if UNITY_5_4_OR_NEWER
    [TargetRpc]
    void TargetUpdateRotation(NetworkConnection nc, Quaternion playerRot){
        syncPlayerRotation = playerRot;
    }
    #else
    public void TargetUpdateRot(UpdateRotTurret ur){
        syncPlayerRotation = ur.rot;
    }
    #endif

    [ClientCallback]
    void TransmitRotations(){
        if (nIdentity.isLocalPlayer){
            if(Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold) {
                CmdProvideRotationsToServer(playerTransform.rotation);
                lastPlayerRot = playerTransform.rotation;
            }
        }
    }
}