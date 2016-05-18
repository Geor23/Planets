using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour {

    // Use this for initialization
    void Start () {
        foreach (Transform child in transform) {
            BoxCollider childCollider = child.GetComponent<BoxCollider>();
            childCollider.size = new Vector3(5, 5, 5);
            //Do stuff
        }
    }


    // Update is called once per frame
    void Update () {
	
	}
}