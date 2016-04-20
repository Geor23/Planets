using UnityEngine;
using System.Collections;

public class Force : MonoBehaviour {

	void OnMouseDown()
	{
		rigidbody.AddForce(-transform.forward * 600);
			rigidbody.useGravity = true;
	}
}