using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class ChangeToStory : MonoBehaviour
{
    GameStatsManager gsm;
    PlanetsNetworkManager pm;
    public GameObject obsScene;
    public GameObject story1;
    public GameObject story2;
    // Use this for initialization
    void Start()
    {
        pm = (PlanetsNetworkManager)NetworkManager.singleton;
        int time = pm.getRoundOverTimer() / 2;
        gsm = GameStatsManager.singleton;
        Invoke("StartStory", time);
    }

    void StartStory(){
        if (PlayerConfig.singleton.GetObserver()){
            if (gsm.getLatestRound() == 1) {
                obsScene.SetActive(false);
                story1.SetActive(true);
            } else if (gsm.getLatestRound() == 2) {
                obsScene.SetActive(false);
                story2.SetActive(true);
            }
        }
    }
}
