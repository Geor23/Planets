#pragma strict

private var count: int;
private var rb: Rigidbody;
private var health : int;
private var playerStatus : boolean;
//public var winText: UnityEngine.UI.Text;
//public var healthText: UnityEngine.UI.Text;
//public var deathClip : AudioClip;                                  // The audio clip to play when the player dies.
private var jumpDelay: boolean;
private var doubleJump: int = 0;
private var moveSpeed: float;
private var moveDir: Vector3;

function Start(){
	rb = GetComponent.<Rigidbody>();
    moveSpeed = 10;
	health = 10;
	playerStatus = true; 
//	winText.text = "";	
//	healthText.text = "HP: " + health.ToString();
}

function Update() {
    moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);

    if (Input.GetKeyDown(KeyCode.Space) && jumpDelay == false) {
        Jump();
    }
    
    if(playerStatus == true){
        if(health <= 0){
            playerStatus = false;
            //healthText.text = "ur dead fam, u r moist";
             //Play the dying sound effect at the player's location.
           // AudioSource.PlayClipAtPoint(deathClip, transform.position);
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

//function OnCollisionEnter(col: Collision){
//    if(col.gameObject.CompareTag("projectile")){
//        if(health > 0){
//            health = health - 10;
//           healthText.text = "HP: " + health.ToString();
//        }
//        col.gameObject.Destroy(col.gameObject);
//    }
//}