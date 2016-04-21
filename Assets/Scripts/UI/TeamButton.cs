using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeamButton : MonoBehaviour {

public bool pirate;
public bool superCorp;
public LobbyGUI lobbyGUI;
public Text header;
public Text otherHeader;
private RectTransform rTransform;

	void Start () {
		header.color = new Color(1F,0.5F,0.5F,1F);
		rTransform = GetComponent<RectTransform>();
	}

	void OnGUI () {
		if (Event.current.type == EventType.MouseUp && RectTransformUtility.RectangleContainsScreenPoint(rTransform, Event.current.mousePosition))
		{
			if (pirate) { 
				lobbyGUI.ChooseTeamPirates();
			} else { 
				lobbyGUI.ChooseTeamSuperCorp();
			}
			header.color = new Color (1F,1F,1F,1F);
			otherHeader.color = new Color (1F,0.5F,0.5F,1F);
		}
	}

	void Update () {
		if (Input.GetMouseButtonDown(0) && RectTransformUtility.RectangleContainsScreenPoint(rTransform, Input.mousePosition)) {
			if (pirate) { 
				lobbyGUI.ChooseTeamPirates();
			} else { 
				lobbyGUI.ChooseTeamSuperCorp();
			}
			header.color = new Color (1F,1F,1F,1F);
			otherHeader.color = new Color (1F,0.5F,0.5F,1F);
		}
	}

	public void buttonEffect () {
		if (pirate) { 
			lobbyGUI.ChooseTeamPirates();
		} else { 
			lobbyGUI.ChooseTeamSuperCorp();
		}
		header.color = new Color (1F,1F,1F,1F);
		otherHeader.color = new Color (1F,0.5F,0.5F,1F);
	}

}
