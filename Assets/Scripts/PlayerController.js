#pragma strict

private var count: int;
private var rb: Rigidbody;
private var health : int;
private var playerStatus : boolean;
public var speed: float;
public var countText: UnityEngine.UI.Text;
public var winText: UnityEngine.UI.Text;
public var healthText: UnityEngine.UI.Text;

private var jumpDelay: boolean;
private var doubleJump: int = 0;

function Start(){

	rb = GetComponent(Rigidbody);
	count = 0;
	health = 100;
	playerStatus = true; 
	countText.text = "Count: " + count.ToString();	
	winText.text = "";	
	healthText.text = "HP: " + health.ToString();
}

function Update() {
    if (Input.GetKeyDown(KeyCode.Space) && jumpDelay == false) {
        Jump();
    }
    
    if(playerStatus == true){
        if(health <= 0){
            playerStatus = false;
            healthText.text = "ur dead fam, u r moist";
            gameObject.SetActive(false);
        }    
    }
}

function Jump() {
    if (doubleJump <=1) {
        rb.velocity.y = 20;
        jumpTimer();
    }
}

function jumpTimer() {
    if (Input.GetKeyDown(KeyCode.Space)) {
        doubleJump++;
    }
    if(doubleJump > 1) {
        doubleJump = 0;
        jumpDelay = true;
        yield WaitForSeconds(3);
        jumpDelay = false;
    }
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

    function OnCollisionEnter(col: Collision){
        if(col.gameObject.CompareTag("projectile")){
            if(health > 0){
                health = health - 10;
                healthText.text = "HP: " + health.ToString();
            }
            col.gameObject.Destroy(col.gameObject);
        }
    }