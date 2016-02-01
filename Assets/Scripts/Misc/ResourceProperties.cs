using UnityEngine;
using System.Collections;

public class ResourceProperties : MonoBehaviour {
	public int minScore;
	public int maxScore;
	//Add some way to influence score frequency (ie, make lower 70% of values appear 90% of the time etc)
	private int score;
	// Use this for initialization
	void Start () {
		score = Random.Range (minScore, maxScore);
		Rescale();
	}
	public int getScore () {
		return score;
	}
	void OnCollisionEnter(Collision col) {
		if(col.gameObject.CompareTag("ResourcePickUp")){
				if (GetInstanceID() > col.gameObject.GetInstanceID()){
					ResourceProperties resProp = col.gameObject.GetComponent<ResourceProperties>();
					score = score + resProp.getScore();
					if (score > 2*maxScore) {score = 2*maxScore;}
					Rescale();
					Destroy(col.gameObject);
				}
		}
	}
	void Rescale () {
		float scale;
		float tmp = (maxScore - minScore)/5;
		if (score - minScore < tmp) {scale = 0.2f;}
		else if (score - minScore < tmp*2) {scale = 0.4f;}
		else if (score - minScore < tmp*3) {scale = 0.6f;}
		else if (score - minScore < tmp*4) {scale = 0.8f;}
		else if (score - minScore < tmp*5) {scale = 1.0f;}
		else if (score - minScore < tmp*6) {scale = 1.2f;}
		else if (score - minScore < tmp*7) {scale = 1.4f;}
		else if (score - minScore < tmp*8) {scale = 1.6f;}
		else if (score - minScore < tmp*9) {scale = 1.8f;}
		else {scale = 2.0f;}
		transform.localScale = new Vector3 (scale, scale, scale);
	}
}