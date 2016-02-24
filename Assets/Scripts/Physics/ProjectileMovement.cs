using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileMovement : MonoBehaviour {
  private Rigidbody rb;
  private Vector3 direction;
  public int speedFactor;
  public void setDirection(Vector3 dir){
    rb = GetComponent<Rigidbody>();
    direction = dir;
    rb.velocity = direction*speedFactor;
  }

  void Update () {
  	
  }
}