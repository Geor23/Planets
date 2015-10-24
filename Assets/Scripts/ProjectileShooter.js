#pragma strict

var prefab:GameObject;

function Start () {
       prefab = Resources.Load("projectile") as GameObject;
}

function Update () {
    var mouseClick = Input.GetMouseButton(0);
    
    if (mouseClick) {
        var projectile:GameObject = Instantiate(prefab) as GameObject;
        projectile.transform.position = transform.position;         // This onlh shoots at the direction of the camera: +Camera.main.transform.forward *2;
        var rb:Rigidbody = projectile.GetComponent.<Rigidbody>();
        rb.velocity=Camera.main.transform.forward*40;
        Destroy(projectile,10);    
    }
}