using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlanetariumStereo : MonoBehaviour {
	
	public bool stereoEnabled;

	public static PlanetariumStereo singleton;

	void Start(){
		singleton = this;
	}

	public bool stereoRunning(){
		return stereoEnabled;
	}

	public bool isMaster(){
		#if UNITY_STANDALONE
		return ClusterNetwork.isMasterOfCluster;
		#else
		return false;
		#endif
	}
	
}