using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using ProgressBar;

public class RoundPlayerCanvasData : MonoBehaviour {

	public Text id;
	public GameObject piratesTeardrop;
	public GameObject superCorpTeardrop;
	public Text localScore;
	private PersonalPlayerInfo ppi;
 private int team;
 void Start(){
		if(!NetworkClient.active) return;
	   ppi = PersonalPlayerInfo.singleton;
    Debug.Log("ID IS " + ppi.getObsId());
    if (ppi.getObsId().ToString() != null){
        id.text = ppi.getObsId().ToString();
    }
  if (PlayerConfig.singleton.getTeam() == TeamID.TEAM_SUPERCORP){
    team = TeamID.TEAM_SUPERCORP;
		  piratesTeardrop.SetActive(false);
		}else{
    team = TeamID.TEAM_PIRATES;
    superCorpTeardrop.SetActive(false);
		}
	}

 void Update(){
        if (PlayerConfig.singleton.getTeam() != team){
            id.text = ppi.getObsId().ToString();
            if (PlayerConfig.singleton.getTeam() == TeamID.TEAM_SUPERCORP){
                team = TeamID.TEAM_SUPERCORP;
                piratesTeardrop.SetActive(false);
                superCorpTeardrop.SetActive(true);
            } else {
                team = TeamID.TEAM_PIRATES;
                superCorpTeardrop.SetActive(false);
                piratesTeardrop.SetActive(true);
            }
        }
    }

	public void PingPlayer(){
		UniqueObjectMessage uom = new UniqueObjectMessage();
		uom.netId = PersonalPlayerInfo.singleton.getPlayer().getPlayerObject().GetComponent<NetworkIdentity>().netId;
		NetworkManager.singleton.client.Send(Msgs.ping, uom);
	}
	//TODO update
}