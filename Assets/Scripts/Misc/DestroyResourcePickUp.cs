using UnityEngine;
using System.Collections;

public class DestroyResourcePickUp : MonoBehaviour {

	public float destroyTime = 3.0f;

	void Start () {
		Destroy(gameObject, destroyTime);	
	}
}
