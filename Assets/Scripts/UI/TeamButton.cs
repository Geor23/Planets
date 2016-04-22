using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeamButton : MonoBehaviour {

public bool pirate;
public bool superCorp;
public LobbyGUI lobbyGUI;
public Text header;
public Text otherHeader;
private Rect rect;
public Camera camera;

	void Start () {
		header.color = new Color(1F,0.5F,0.5F,1F);
		rect = GetScreenRect(GetComponent<RectTransform>());
	}

	void OnGUI () {
		if (Event.current.type == EventType.MouseUp && rect.Contains(Event.current.mousePosition))
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

		if (Input.GetMouseButtonDown(0) && rect.Contains(Input.mousePosition)) {
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

	private Rect GetScreenRect(RectTransform rTransform) {
        
        Vector3[] corners = new Vector3[4];
        Vector3[] screenCorners = new Vector3[2];
 
        rTransform.GetWorldCorners(corners);

        screenCorners[0] = RectTransformUtility.WorldToScreenPoint(camera, corners[1]);
        screenCorners[1] = RectTransformUtility.WorldToScreenPoint(camera, corners[3]);
 
        screenCorners[0].y = Screen.height - screenCorners[0].y;
        screenCorners[1].y = Screen.height - screenCorners[1].y;
 
        return new Rect(screenCorners[0], screenCorners[1] - screenCorners[0]);
    }

}
