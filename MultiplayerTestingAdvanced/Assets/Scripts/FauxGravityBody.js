﻿#pragma strict

public var attractor: FauxGravityAttractor;
private var myTransform: Transform;
private var rb: Rigidbody;

function Start(){
	rb = GetComponent.<Rigidbody>();
	rb.constraints = RigidbodyConstraints.FreezeRotation;
	rb.useGravity = false;	
	myTransform = transform;
}

function Update(){
	attractor.Attract(myTransform);
}