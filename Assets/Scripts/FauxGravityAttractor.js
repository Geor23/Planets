#pragma strict

public var gravity: float;
private var gravityUp: Vector3;
private var bodyUp: Vector3;
private var targetRotation: Quaternion; 

function Start(){
	
	gravity = -10;

}

function Attract(body: Transform){
	
	gravityUp = (body.position - transform.position).normalized;
	bodyUp = body.up;

	body.GetComponent.<Rigidbody>().AddForce(gravityUp * gravity);
	targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
	body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
}
