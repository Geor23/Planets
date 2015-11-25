using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace UnityStandardAssets.CrossPlatformInput {
	public class PlayerControllerMobile : MonoBehaviour {
		private NetworkIdentity nIdentity;
		private const float fireRate = 0.3F;
		private float nextFire = 0.0F;

		public float speed = 3.0F;
		public float rotSpeed = 12.0F;
		public Transform planet;
		public Transform model;
		public Transform turret;

		Rigidbody rb;
		void Start(){
			nIdentity = GetComponent<NetworkIdentity>();
		}
		void Update () {
			if(nIdentity.isLocalPlayer){
				rb = GetComponent<Rigidbody>();
				Vector3 forward = transform.forward;
				Vector3 right = transform.right;
				float aimH = CrossPlatformInputManager.GetAxis ("AimH");
				float aimV = CrossPlatformInputManager.GetAxis ("AimV");
				Vector3 turretDirection = ((forward * aimV) + (right * aimH)).normalized;
				rotateObject(turret, turretDirection);


				float forwardSpeed = speed * CrossPlatformInputManager.GetAxis("MoveV");
				float strafeSpeed = speed * CrossPlatformInputManager.GetAxis("MoveH");
				Vector3 moveDir = forwardSpeed * forward + strafeSpeed * right;
				rotateObject(model, moveDir.normalized);
				rb.MovePosition(transform.position + moveDir * Time.deltaTime*5);
			}
		}
		void rotateObject (Transform obj, Vector3 direction) {
			if (direction.magnitude > 0.1) {
				Vector3 upDir = (model.position - planet.position).normalized;	
				Quaternion currentRotation = Quaternion.LookRotation(direction, upDir);
				obj.rotation = Quaternion.Lerp(obj.rotation, currentRotation, Time.deltaTime*rotSpeed);

				if(CrossPlatformInputManager.GetAxis ("AimH") + CrossPlatformInputManager.GetAxis ("AimV") != 0){
					if(Time.time < nextFire)
						return;
		      /*GameObject projectile = Instantiate(Resources.Load("projectile")) as GameObject;
		      projectile.GetComponent<Transform>().position = rb.position + direction + upDir;
		      projectile.AddComponent<ProjectileMovement>().setDirection(direction);
		      projectile.AddComponent<FauxGravityBody>();
		      Destroy(projectile, 4);*/
	        GetComponent<PlayerNetworkHandler>().CmdSpawnProjectile(rb.position + direction + upDir, direction);
	      	nextFire = Time.time + fireRate;
	      }
			}
		}
	}
}