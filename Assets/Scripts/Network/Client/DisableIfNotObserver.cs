using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DisableIfNotObserver : MonoBehaviour {
    private PlanetsNetworkManager nm;
    // Use this for initialization
    void Start () {
        nm = (PlanetsNetworkManager)NetworkManager.singleton;
        if (nm.observerCollisionsOnly()){
           // Debug.Log("Is observer?" + PlayerConfig.singleton.GetObserver());
            if (!PlayerConfig.singleton.GetObserver()){
                gameObject.SetActive(false);
            }else{
    //            Debug.Log("I am an observer");
            }
        }
    }
}
