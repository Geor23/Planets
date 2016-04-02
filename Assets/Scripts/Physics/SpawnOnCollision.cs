using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnOnCollision : MonoBehaviour {

  public GameObject spawnType;
  public float timeToLive;

  void OnCollisionEnter(Collision col){
    GameObject spawn = (GameObject) Instantiate(spawnType, gameObject.transform.position, Quaternion.identity);
    Destroy(spawn, timeToLive);
  }
}
