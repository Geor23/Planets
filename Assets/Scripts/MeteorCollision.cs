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
        if ((col.gameObject.CompareTag("Planet")) || (col.gameObject.CompareTag("Tree")) || (col.gameObject.CompareTag("Mound")) || (col.gameObject.CompareTag("CityPlanetStructure"))) {
            gameObject.GetComponent<Exploder>().expl();

            resourcePowerUpManager.meteorCollision(gameObject);
        }
    }
}
