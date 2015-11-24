using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileMovement : MonoBehaviour {
  private Rigidbody rb;
  private Vector3 direction;
  public void setDirection(Vector3 dir){
    rb = GetComponent<Rigidbody>();
    direction = dir;
    rb.velocity = direction*18;
  }
}