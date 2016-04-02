using UnityEngine;

// PlayerScript requires the GameObject to have a Rigidbody component
[RequireComponent (typeof (Camera))]
public class FlipCamera : MonoBehaviour {
	void OnPreCull () {
	 GetComponent<Camera>().ResetWorldToCameraMatrix ();
	 GetComponent<Camera>().ResetProjectionMatrix ();
	 GetComponent<Camera>().projectionMatrix = GetComponent<Camera>().projectionMatrix * Matrix4x4.Scale(new Vector3 (1, -1, 1));
	}

	void OnPreRender () {
	 GL.SetRevertBackfacing (true);
	}

	void OnPostRender () {
	 GL.SetRevertBackfacing (false);
	}
}