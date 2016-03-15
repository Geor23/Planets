using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class ResourceController : MonoBehaviour {

    private int minScore = 1;
    private int maxScore = 11;
    private int score = 1;
    private NetworkManager nm = NetworkManager.singleton;
    // Use this for initialization
    void Start(){
        InvokeRepeating("UpdateValue", 1, 1);
        Rescale();
    }

    public int getScore(){
        return score;
    }

    public void setScore(int scoreToSet){
        score = scoreToSet;
        Rescale();
    }

    void UpdateValue(){
        score += 1;
       if (score > 2 * maxScore) { score = 2 * maxScore; }
        Rescale();
    }

    void Rescale(){
        float scale;
        float tmp = (maxScore - minScore) / 5;
        if (score==1) {
            scale = 0.2f;
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
        else if (score - minScore < tmp * 2) {
            scale = 0.4f;
            gameObject.GetComponent<Renderer>().enabled = true;
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }
        else if (score - minScore < tmp * 3) { scale = 0.6f; }
        else if (score - minScore < tmp * 4) { scale = 0.8f; }
        else if (score - minScore < tmp * 5) { scale = 1.0f; }
        else if (score - minScore < tmp * 6) { scale = 1.2f; }
        else if (score - minScore < tmp * 7) { scale = 1.4f; }
        else if (score - minScore < tmp * 8) { scale = 1.6f; }
        else if (score - minScore < tmp * 9) { scale = 1.8f; }
        else { scale = 1.8f; }
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.CompareTag("PlayerPirate")|| col.gameObject.CompareTag("PlayerSuperCorp")){
            NetworkIdentity nIdentity = col.gameObject.GetComponent<NetworkIdentity>();
            if (nIdentity.isLocalPlayer){
                Debug.Log("Collided with a player");
                col.gameObject.GetComponent<PlayerControllerMobile>().SetScoreTextNew(score);
                AddScore sc = new AddScore();
                sc.team = 0;
                sc.score = score;
                sc.obj = gameObject;
                nm.client.Send(Msgs.clientTeamScore, sc);
            }
            else
            {
                Debug.Log("I AM NOT A LOCAL IDENTITY INTERACTING");
            }
            //This means each client locally updates score upon witnessing a collision
            setScore(1);
        }
    }
}
