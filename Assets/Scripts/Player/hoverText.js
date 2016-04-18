var target : Transform;		// Object that this label should follow
var offset = Vector3.up;	// Units in world space to offset; 1 unit above object by default
var clampToScreen = false;	// If true, label will be visible even if object is off screen
var clampBorderSize = .05;	// How much viewport space to leave at the borders when a label is being clamped
// var useMainCamera = false;	// Use the camera tagged MainCamera
var cameraToUse : Camera;	// Only use this if useMainCamera is false
// var cameraToUse2 : Camera;	// Only use this if useMainCamera is false
// private var cam : Camera;
// private var cam2 : Camera;
private var thisTransform : Transform;
private var camTransform : Transform;
// private var camTransform2 : Transform;
// var myLayerMask : LayerMask;
// var hitInfo: RaycastHit;

function Start () {
	thisTransform = transform;
	// if (useMainCamera)
	// 	cam = Camera.main;
	// else {
		// cam = cameraToUse;
	// 	// cam2 = cameraToUse2;
	// }
	camTransform = cameraToUse.transform;
	// camTransform2 = cam2.transform;
}
 
function Update () {

	// if (clampToScreen) {

	// 	var relativePosition = camTransform.InverseTransformPoint(target.position);
	// 	relativePosition.z = Mathf.Max(relativePosition.z, 1.0);
	// 	thisTransform.position = cam.WorldToViewportPoint(camTransform.TransformPoint(relativePosition + offset));
	// 	thisTransform.position = Vector3(Mathf.Clamp(thisTransform.position.x, clampBorderSize, 1.0-clampBorderSize),
	// 									 Mathf.Clamp(thisTransform.position.y, clampBorderSize, 1.0-clampBorderSize),
	// 									 thisTransform.position.z);
	// 	Debug.Log("x: " + thisTransform.position.x + " : " + target.position.x);
	//     Debug.Log("y: " + thisTransform.position.y + " : " + target.position.y);
	//     Debug.Log("z: " + thisTransform.position.z + " : " + target.position.z);
	
	// } else {

		// var rayToCameraPos: Ray = new Ray(thisTransform.position, camTransform1.position-thisTransform.position);
		// var dir = target.position - camTransform.position;
		// if(Physics.Raycast(camTransform1.position, dir, hitInfo, 1000, myLayerMask)) {
		    //Debug.Log("on the other side of cam1");
		    thisTransform.position = cameraToUse.WorldToViewportPoint(target.position + offset);
		    Debug.Log("x: " + thisTransform.position.x + " : " + target.position.x);
		    Debug.Log("y: " + thisTransform.position.y + " : " + target.position.y);
		    Debug.Log("z: " + thisTransform.position.z + " : " + target.position.z);

		// } else {
		// 	//Debug.Log("on the side of cam1");
		// 	thisTransform.position = cam1.WorldToViewportPoint(target.position + offset);
		// }
	// }
}
 
@script RequireComponent(GUIText)