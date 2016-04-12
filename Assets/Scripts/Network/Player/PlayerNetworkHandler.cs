using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class PlayerNetworkHandler : NetworkBehaviour {
    private Transform transformT;
    private GameObject projectile;
    public GameObject ResourcePickUp;
    public int projectileLifetime;
    public GUIText idForObsScreen;

    [Command]
    public void CmdSpawnProjectile(Vector3 position, Vector3 direction, String projectileName, String name){
      GameObject projectile = Instantiate(Resources.Load(projectileName)) as GameObject;
      projectile.GetComponent<Transform>().position = position;
      projectile.GetComponent<ProjectileMovement>().setDirection(direction);
      Destroy(projectile, projectileLifetime);
      NetworkServer.Spawn(projectile);
      foreach (NetworkConnection nc in ((PlanetsNetworkManager)PlanetsNetworkManager.singleton).getUpdateListeners()){
        projectile.GetComponent<ServerSyncPos>().TargetSyncClient(nc, position, direction);
      }
      Text playerWhoFired = projectile.GetComponent<Text>();
      playerWhoFired.text = name;
    }

    [Command]
    public void CmdSpawnResource(Vector3 position, int score){
          //spawn a resource in the position the player died
      GameObject objClone = (GameObject)Instantiate(ResourcePickUp, position, Quaternion.identity);
      objClone.GetComponent<Text>().text = score.ToString();
      objClone.GetComponent<DeathResourceProperties>().setScore(score);
      Debug.Log("Setting the spawned resource's score to " + score);
      NetworkServer.Spawn(objClone);
    }

    [Command]
    public void CmdDestroyDeathResource(GameObject obj){
        NetworkServer.Destroy(obj);
    }

    [Command]
    public void CmdSetId(string id) {
      idForObsScreen.text = id;
      Debug.Log(idForObsScreen.text);
    }
}