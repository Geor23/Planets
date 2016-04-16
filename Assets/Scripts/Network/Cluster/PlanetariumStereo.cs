using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlanetariumStereo : MonoBehaviour {
	
	public bool stereoEnabled;

	PlanetariumStereo singleton;

	void Start(){
		singleton = this;
	}

	bool stereoRunning(){
		return stereoEnabled;
	}

	bool isMaster(){
		return ClusterNetwork.isMasterOfCluster;
	}
	
}