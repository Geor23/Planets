using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class PlayerNetworkHandler : NetworkBehaviour {
    private Transform transform;
    private GameObject projectile;

    void Start(){
      if(!isServer){
/*
        projectile = Instantiate(Resources.Load("projectile")) as GameObject;
        projectile.GetComponent<Transform>().position = new Vector3(100,100,100);
        ClientScene.RegisterPrefab(projectile);
*/
      }
    }
    [Command]
    public void CmdSpawnProjectile(Vector3 position, Vector3 direction){
      GameObject projectile = Instantiate(Resources.Load("projectile")) as GameObject;
      projectile.GetComponent<Transform>().position = position;
      projectile.GetComponent<ProjectileMovement>().setDirection(direction);
      Destroy(projectile, 16);
      NetworkServer.Spawn(projectile);
      projectile.GetComponent<ServerSyncPos>().RpcSyncClient(position, direction);
    }
}