using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeamButton : MonoBehaviour {

public bool pirate = false;
public bool superCorp = false;
public LobbyGUI lobbyGUI;
public Text header;
public Text otherHeader;

	void Start () {
		header.color = new Color(1F,0.5F,0.5F,1F);
		Debug.Log (GetComponent<RectTransform>().rect.size);
	}

	void OnGUI () {
		Debug.Log ("OnGUI called");
		if (Event.current.type == EventType.MouseUp && GetComponent<RectTransform>().rect.Contains(Event.current.mousePosition))
		{
			Debug.Log("Clicked: ");
			if (pirate) { 
				Debug.Log("Pirate Team");
				lobbyGUI.ChooseTeamPirates();
			} else { 
				Debug.Log("SuperCorp Team");
				lobbyGUI.ChooseTeamSuperCorp();
			}
			header.color = new Color (1F,1F,1F,1F);
			otherHeader.color = new Color (1F,0.5F,0.5F,1F);
		}
	}

	void Update () {
		if (Input.GetMouseButtonDown(0) && GetComponent<RectTransform>().rect.Contains(Input.mousePosition)) {
			Debug.Log("Clicked: ");
			if (pirate) { 
				Debug.Log("Pirate Team");
				lobbyGUI.ChooseTeamPirates();
			} else { 
				Debug.Log("SuperCorp Team");
				lobbyGUI.ChooseTeamSuperCorp();
			}
			header.color = new Color (1F,1F,1F,1F);
			otherHeader.color = new Color (1F,0.5F,0.5F,1F);
		}
	}

	public void buttonEffect () {
		Debug.Log("Clicked: ");
		if (pirate) { 
			Debug.Log("Pirate Team");
			lobbyGUI.ChooseTeamPirates();
		} else { 
			Debug.Log("SuperCorp Team");
			lobbyGUI.ChooseTeamSuperCorp();
		}
		header.color = new Color (1F,1F,1F,1F);
		otherHeader.color = new Color (1F,0.5F,0.5F,1F);
	}

}
