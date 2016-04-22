using UnityEngine;
using System.Collections;

public class PlanetariumDomeControl : MonoBehaviour {
    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
}