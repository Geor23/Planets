#pragma strict

public var speed: float;

function Update () {
	transform.Rotate(Vector3.up, speed * Time.deltaTime);
}