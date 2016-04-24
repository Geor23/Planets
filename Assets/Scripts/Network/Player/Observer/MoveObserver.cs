using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MoveObserver : MonoBehaviour {
    public GameObject obj1;
    public NetworkIdentity nIdentity;

	// Update is called once per frame
	void Update () {
        if(!nIdentity.isLocalPlayer) return;
        if (Input.GetKey(KeyCode.A)) {  //Camera one commands
            obj1.transform.position -= transform.right.normalized/10;
        }else if(Input.GetKey(KeyCode.D)){
            obj1.transform.position += transform.right.normalized / 10;
        }

        if(Input.GetKey(KeyCode.W)) {
            obj1.transform.position -= transform.up.normalized / 10;
        }
        else if(Input.GetKey(KeyCode.S)){
            obj1.transform.position += transform.up.normalized / 10;
        }

        if (Input.GetKey(KeyCode.E)){
            obj1.transform.position += transform.forward.normalized / 10;
        }
        else if (Input.GetKey(KeyCode.Q)){
            obj1.transform.position -= transform.forward.normalized / 10;
        }
    }
}
