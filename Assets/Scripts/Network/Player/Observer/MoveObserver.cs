using UnityEngine;
using System.Collections;

public class MoveObserver : MonoBehaviour {
    public Camera cam1;
    public Camera cam2;

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A)) {  //Camera one commands
            cam1.transform.position -= transform.right.normalized/10;
        }else if(Input.GetKey(KeyCode.D)){
            cam1.transform.position += transform.right.normalized / 10;
        }

        if(Input.GetKey(KeyCode.W)) {
            cam1.transform.position -= transform.up.normalized / 10;
        }
        else if(Input.GetKey(KeyCode.S)){
            cam1.transform.position += transform.up.normalized / 10;
        }

        if (Input.GetKey(KeyCode.LeftArrow)){  //Camera one commands
            cam2.transform.position += transform.right.normalized / 10;
        }else if (Input.GetKey(KeyCode.RightArrow)){
            cam2.transform.position -= transform.right.normalized / 10;
        }

        if (Input.GetKey(KeyCode.UpArrow)){
            cam2.transform.position += transform.up.normalized / 10;
        }
        else if (Input.GetKey(KeyCode.DownArrow)){
            cam2.transform.position -= transform.up.normalized / 10;
        }
    }
}
