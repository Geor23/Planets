using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput {
	public class PlayerControllerMobile : MonoBehaviour {
		public float speed = 3.0F;
		public float rotSpeed = 12.0F;
		public Transform planet;
		public Transform model;
		public Transform turret;
		Rigidbody rb;
		void Update () {
			rb = GetComponent<Rigidbody>();
			float aimH = CrossPlatformInputManager.GetAxis ("AimH");
			float aimV = CrossPlatformInputManager.GetAxis ("AimV");
			Vector3 turretDirection = new Vector3 (aimH, 0, aimV).normalized;
			rotateObject(turret, turretDirection);
			Vector3 forward = transform.forward;
			Vector3 right = transform.right;
			float forwardSpeed = speed * CrossPlatformInputManager.GetAxis("MoveV");
			float strafeSpeed = speed * CrossPlatformInputManager.GetAxis("MoveH");
			Vector3 moveDir = forwardSpeed * forward + strafeSpeed * right;
			rotateObject(model, moveDir.normalized);
			rb.MovePosition(transform.position + moveDir * Time.deltaTime);
		}
		void rotateObject (Transform obj, Vector3 direction) {
			if (direction.magnitude > 0.1) {
				Vector3 upDir = (model.position - planet.position).normalized;
				Quaternion currentRotation = Quaternion.LookRotation(direction, upDir);
				obj.rotation = Quaternion.Lerp(obj.rotation, currentRotation, Time.deltaTime*rotSpeed);
			}
		}
	}
}