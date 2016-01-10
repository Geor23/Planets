using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

/*
  The full API documentation for NetworkManager can be found here:
    http://docs.unity3d.com/ScriptReference/Networking.NetworkManager.html
*/

public class PlanetsNetworkManager : NetworkManager {
	
	[SerializeField] GameObject player1;
	[SerializeField] GameObject player2;
	
	//	[SerializeField] Mesh pl1;
	//	[SerializeField] Mesh pl2;
	GameObject chosenCharacter; // character1, character2, etc.
	
	/*
    Override the virtual default functions to build on existing behaviour 
    */
	
	/*
    Server functions */
	// called when a client connects 
	//public override void OnServerConnect(NetworkConnection conn);
	public bool hasPickedTeam = false; 
	public bool hasConnected = false;
	
	// called when a client disconnects
	public override void OnServerDisconnect(NetworkConnection conn)
	{
		NetworkServer.DestroyPlayersForConnection(conn);
	}
	
	// called when a client is ready
	public override void OnServerReady(NetworkConnection conn)
	{
		NetworkServer.SetClientReady(conn);
		ClientScene.RegisterPrefab(player1);
		ClientScene.RegisterPrefab(player2);

		
	}
	
	// called when a new player is added for a client
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		/* This is where you can register players with teams, and spawn the player at custom points in the team space */
		//hasConnected = true;
		GameObject player = Instantiate (chosenCharacter, GetStartPosition ().position, Quaternion.identity) as GameObject;
		//		GameObject player = Instantiate (playerPrefab, GetStartPosition ().position, Quaternion.identity) as GameObject;
		
		NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
		
	}
	
	//called when a player is removed for a client
	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController)
	{
		GameObject player = playerController.gameObject;
		if (player != null)
		{
			if (playerController.unetView != null)
				NetworkServer.Destroy(player);
		}
	}
	
	// called when a network error occurs
	// public override void OnServerError(NetworkConnection conn, int errorCode);
	
	/*
    Client functions */
	// called when connected to a server
	public override void OnClientConnect(NetworkConnection conn)
	{
		hasConnected = true;
		//NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
		Debug.Log("Client connected!");
	}
	
	// called when disconnected from a server
	public override void OnClientDisconnect(NetworkConnection conn)
	{
		StopClient();
	}
	
	public override void OnClientSceneChanged(NetworkConnection conn){
		ClientScene.Ready(conn);
	}
	
	public void OnGUI() {
		if (!hasPickedTeam && GameObject.Find ("UsernameObject") != null) {
			
			if (GameObject.Find ("UsernameObject").GetComponent<PlayerUsernameScript> ().textOnScreen.IsActive ()) {
				
				GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
				GUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();
				GUILayout.BeginVertical ();
				GUILayout.FlexibleSpace ();
				
				if (GUILayout.Button (" Team A ")) {
					chosenCharacter = player1 ;
					//GameObject player = Instantiate ((GameObject)playerPrefab, GetStartPosition ().position, Quaternion.identity) as GameObject;
					//					MeshFilter objMesh = GameObject.AddComponent<MeshFilter>();
					//					GameObject newMesh = Resources.Load(pl1) as GameObject;
					//					objMesh.mesh = newMesh.GetComponent<MeshFilter>().sharedMesh;
					
					ClientScene.AddPlayer (0);
					//Debug.Log (GameObject.Find ("Player(Clone)"));
					GameObject.Find ("Player(Clone)").GetComponent<TeamMember> ().RpcSetTeamID (1); 
					hasPickedTeam = true;
				}
				
				if (GUILayout.Button (" Team B ")) {
					chosenCharacter = player2 ;
					//						
					//						MeshFilter objMesh = GameObject.AddComponent<MeshFilter>();
					//						GameObject newMesh = Resources.Load(pl2) as GameObject;
					//						objMesh.mesh = newMesh.GetComponent<MeshFilter>().sharedMesh;
					//					//GameObject player = Instantiate ((GameObject)playerPrefab, GetStartPosition ().position, Quaternion.identity) as GameObject;
					ClientScene.AddPlayer (0);
					//Debug.Log (GameObject.Find ("Player(Clone)"));
					GameObject.Find ("Player2(Clone)").GetComponent<TeamMember> ().RpcSetTeamID (2); 
					hasPickedTeam = true;
				}
				
				if (GUILayout.Button ("Auto assign")) {
					chosenCharacter = player1 ;
					//GameObject player = Instantiate ((GameObject)playerPrefab, GetStartPosition ().position, Quaternion.identity) as GameObject;
					ClientScene.AddPlayer (0);
					//Debug.Log (GameObject.Find ("Player(Clone)"));
					GameObject.Find ("Player(Clone)").GetComponent<TeamMember> ().RpcSetTeamID (1); 
					hasPickedTeam = true;
				}
				
				GUILayout.FlexibleSpace ();
				GUILayout.EndVertical ();
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				GUILayout.EndArea ();
			}
		}
	}
	
	// called when a network error occurs
	//public override void OnClientError(NetworkConnection conn, int errorCode);
	
	// called when told to be not-ready by a server
	//public override void OnClientNotReady(NetworkConnection conn);
}