#pragma strict

public var player: GameObject;
private var offset: Vector3;

function Start () {
	offset = transform.position - player.transform.position;
}

function LateUpdate () {
	transform.position = player.transform.position + offset;
}