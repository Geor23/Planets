using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class DisableIfNotLocalPlayer : MonoBehaviour {

	public NetworkIdentity ni;

	public void Start(){
		if(ni != null && !ni.isLocalPlayer){
			gameObject.SetActive(false);
		}
	}
}