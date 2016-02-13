using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FauxGravityBody : MonoBehaviour{
  private FauxGravityAttractor attractor;
  private Rigidbody rb;

  void Start() {

    rb = GetComponent<Rigidbody>();
    rb.constraints = RigidbodyConstraints.FreezeRotation;
    rb.useGravity = false;
    attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
 
  }

  void Update(){
            attractor.Attract(gameObject);
  }

}