#pragma strict

var player:GameObject;
var rb:Rigidbody = GetComponent.<Rigidbody>();

function Start(){
  player = GameObject.Find("Player(Clone)");
  rb.position=player.transform.position + player.transform.forward;
}

function Update () {
  rb.velocity=player.transform.forward*40; 
}