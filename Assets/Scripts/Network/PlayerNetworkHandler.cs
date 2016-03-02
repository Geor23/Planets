using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class PlayerNetworkHandler : NetworkBehaviour {
    private Transform transformT;
    private GameObject projectile;
    public GameObject ResourcePickUp;
    public int projectileLifetime;

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
      Destroy(projectile, projectileLifetime);
      NetworkServer.Spawn(projectile);
      projectile.GetComponent<ServerSyncPos>().RpcSyncClient(position, direction);
    }

    [Command]
    public void CmdSpawnResource(Vector3 position, int score){
          //spawn a resource in the position the player died
      GameObject objClone = (GameObject)Instantiate(ResourcePickUp, position, Quaternion.identity);
      
      objClone.GetComponent<DeathResourceProperties>().setScore(score);
      Debug.Log("Setting the spawned resource's score to " + score);
      NetworkServer.Spawn(objClone);
    }
}