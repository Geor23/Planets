﻿using UnityEngine;
using System.Collections;

public class ClickTest : MonoBehaviour {
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Pressed left click.");
			Debug.Log(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed right click.");
        
        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
        }
}