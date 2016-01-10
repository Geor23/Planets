using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
public class TeamMember : NetworkBehaviour {
	public NetworkIdentity nIdentity;
	public int TeamID;
	
	public int getTeamID() {
		int id = TeamID;
		return id;
	}
	
	[ClientRpc]
	public void RpcSetTeamID(int id) {	
		//ClientScene.AddPlayer (0);
		TeamID = id ;
		//this.transform.GetComponentInChildren<MeshFilter>().mesh;
		//
	}
}