using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FauxGravityAttractor : MonoBehaviour {
  public float gravity;
  private Vector3 gravityUp;
  private Vector3 bodyUp;
  private Quaternion targetRotation; 

  private void Start(){
  }

  public void Attract(GameObject obj){
    var body = obj.transform;
    var tmpGrav = gravity;
    if (obj.tag == "ProjectileSuperCorp" || obj.tag == "ProjectilePirate") tmpGrav = -1000;
    gravityUp = (body.position - transform.position).normalized;
    bodyUp = body.up;
    if (!float.IsNaN(body.rotation.w)){
      obj.GetComponent<Rigidbody>().AddForce(gravityUp * tmpGrav);
      targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
      body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
    }
  }
}
