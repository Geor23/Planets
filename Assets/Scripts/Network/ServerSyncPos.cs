using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class ServerSyncPos : NetworkBehaviour {

  private Transform transformT;
  private Rigidbody rb;

  /*[TargetRpc]*/
  public void TargetSyncClient(NetworkConnection nc, Vector3 position, Vector3 direction){
    if(isServer)
      return;
    GetComponent<Transform>().position = position;
    GetComponent<ProjectileMovement>().setDirection(direction);
  }
}