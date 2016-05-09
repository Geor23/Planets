using UnityEngine;
using System.Collections;

public class MeteorCollision : MonoBehaviour {

    private ResourcePowerUpManager resourcePowerUpManager;

    // Use this for initialization
    void Start () {
        resourcePowerUpManager = GameObject.FindGameObjectWithTag("Planet").GetComponent<ResourcePowerUpManager>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Planet"))
        {
            resourcePowerUpManager.meteorCollision(gameObject);
        }
    }
    }
