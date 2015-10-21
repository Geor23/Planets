#pragma strict

private var count: int;
private var rb: Rigidbody;
public var speed: float;
public var countText: UnityEngine.UI.Text;
public var winText: UnityEngine.UI.Text;

function Start(){

	rb = GetComponent(Rigidbody);
	count = 0;
	countText.text = "Count: " + count.ToString();	
	winText.text = "";
}

function FixedUpdate(){

	var moveHorizontal: float = Input.GetAxis("Horizontal");
	var moveVertical: float = Input.GetAxis("Vertical");
	var movement: Vector3 = new Vector3(moveHorizontal, 0.0, moveVertical); 	

	rb.AddForce(movement * speed);
	
}

function OnTriggerEnter(other: Collider){
	
	if(other.gameObject.CompareTag("Pick Up")){
		other.gameObject.SetActive(false);
		count = count + 1;
		countText.text = "Count: " + count.ToString();	
		if(count >= 12){
			winText.text = "Well Done Fam, cop my mixtape";
		}
	}
}