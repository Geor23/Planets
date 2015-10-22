#pragma strict

var prefab:GameObject;

function Start () {
       prefab = Resources.Load("projectile") as GameObject;
}

function Update () {
    //var down  = Input.GetKeyDown(KeyCode.Space);
    
    if (Input.GetMouseButton(0)) {
        var projectile:GameObject = Instantiate(prefab) as GameObject;
        projectile.transform.position = transform.position+Camera.main.transform.forward *2;
        var rb:Rigidbody = projectile.GetComponent.<Rigidbody>();
        rb.velocity=Camera.main.transform.forward*40;
        }
}