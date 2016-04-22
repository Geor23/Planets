using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyListUpdater : MonoBehaviour {
	public LobbyGUI lobbyGUI;
	public Text teamPirate;
	public Text teamSuperCorp;
	
	void Start () {
		lobbyGUI.teamA = teamPirate;
		lobbyGUI.teamB = teamSuperCorp;
	}
}
