using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class ServerSyncPos : NetworkBehaviour {

  private Transform transformT;
  private Rigidbody rb;

  [ClientRpc]
  public void RpcSyncClient(Vector3 position, Vector3 direction){
    if(isServer)
      return;
    Debug.LogError("We are in SyncPos: " + position);
    GetComponent<Transform>().position = position;
    GetComponent<ProjectileMovement>().setDirection(direction);
  }

}