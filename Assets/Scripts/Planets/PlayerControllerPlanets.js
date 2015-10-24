#pragma strict
public var moveSpeed: float;
public var rb: Rigidbody;
private var moveDir: Vector3;

function Start(){
	rb = GetComponent.<Rigidbody>();
}

function Update(){
	moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
}

function FixedUpdate(){
	rb.MovePosition(GetComponent.<Rigidbody>().position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);

}