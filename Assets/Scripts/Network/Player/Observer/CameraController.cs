using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
 
public class CameraController : MonoBehaviour {
   
    private Vector3 sum;
    private Vector3 camUps;
    private int count;
    private Vector3 point;
    private Vector3 avgUp;

    // public GameObject cam;

    public GameObject[] playersS;
    public GameObject[] playersP;
    public float lerpRate = 1f;
    private Vector3 pos;

    void Start(){
        Debug.LogError("AM ALIVE HAHAHA");
        transform.position = new Vector3(0,0,0);
        pos = new Vector3(0, 0, 0);
    }

    void Update () {
        if(!GetComponent<NetworkIdentity>().isLocalPlayer) return;

        count = 0;
        playersS = GameObject.FindGameObjectsWithTag("PlayerSuperCorp");
        playersP = GameObject.FindGameObjectsWithTag("PlayerPirate");
        sum = new Vector3(0,0,0);
        camUps = new Vector3(0,0,0);

        foreach (GameObject player in playersS) {
            sum += player.transform.position;
            camUps += player.transform.GetChild(2).transform.up;
            count++;
        }

        foreach (GameObject player in playersP) {
            sum += player.transform.position;
            camUps += player.transform.GetChild(2).transform.up;
            count++;
        }
        if(count == 0) return;

        point = sum/count;
        avgUp = camUps/count;
        Quaternion newRot = Quaternion.LookRotation(point, -avgUp);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * lerpRate);
        transform.position = pos;
    }
}