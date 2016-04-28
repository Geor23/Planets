using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ProgressBar;

public class RoundPlayerCanvasData : MonoBehaviour {

	public Text id;
	public GameObject piratesTeardrop;
	public GameObject superCorpTeardrop;
	public Text localScore;
	private PersonalPlayerInfo ppi;

	void Start(){
		PersonalPlayerInfo ppi = PersonalPlayerInfo.singleton;
		id.text = ppi.getObsId().ToString();
		if(ppi.getPlayerTeam() == TeamID.TEAM_SUPERCORP){
			piratesTeardrop.SetActive(false);
		}else{
			superCorpTeardrop.SetActive(false);
		}
	}
	//TODO update
}