#pragma strict

var player:GameObject;
var rb:Rigidbody = GetComponent.<Rigidbody>();

function Start(){
  player = GameObject.Find("Player(Clone)");
  var textOnScreen: UnityEngine.UI.Text;
  textOnScreen = player.GetComponent("Text");
  var name:String = textOnScreen.text;
  Debug.Log("Got name "+name);
  // add component player name to projectile
  rb.position=player.transform.position + player.transform.forward;
}


function Update () {
  rb.velocity=player.transform.forward*40; 
}