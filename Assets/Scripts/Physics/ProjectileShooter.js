#pragma strict

var player:GameObject;
var rb:Rigidbody = GetComponent.<Rigidbody>();

function Start(){
  player = GameObject.Find("Player(Clone)");
  rb.position=player.transform.position;
}

function Update () {
  rb.velocity=GameObject.Find("CameraPlayer").GetComponent.<Camera>().main.transform.forward*40; 
}