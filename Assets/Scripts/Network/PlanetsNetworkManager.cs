using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

/*
  The full API documentation for NetworkManager can be found here:
    http://docs.unity3d.com/ScriptReference/Networking.NetworkManager.html
*/

public class PlanetsNetworkManager : NetworkManager {
  /*
    Override the virtual default functions to build on existing behaviour 
    */

  /*
    Server functions */
  // called when a client connects 
  //public override void OnServerConnect(NetworkConnection conn);

  // called when a client disconnects
  public override void OnServerDisconnect(NetworkConnection conn)
  {
      NetworkServer.DestroyPlayersForConnection(conn);
  }

  // called when a client is ready
  public override void OnServerReady(NetworkConnection conn)
  {
      NetworkServer.SetClientReady(conn);
  }

  // called when a new player is added for a client
  public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
  {
      /* This is where you can register players with teams, and spawn the player at custom points in the team space */
      GameObject player = Instantiate((GameObject) playerPrefab, GetStartPosition().position, Quaternion.identity) as GameObject;
      NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
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

  // called when a network error occurs
  //public override void OnClientError(NetworkConnection conn, int errorCode);

  // called when told to be not-ready by a server
  //public override void OnClientNotReady(NetworkConnection conn);
}