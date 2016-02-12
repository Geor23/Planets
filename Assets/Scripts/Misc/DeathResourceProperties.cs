using UnityEngine;
using System.Collections;

public class DeathResourceProperties : MonoBehaviour {

	//Add some way to influence score frequency (ie, make lower 70% of values appear 90% of the time etc)
	public int score;
	// Use this for initialization
	void Start () {
	}

	public int getScore () {
		return score;
	}

	public void setScore(int scoreToSet){
		score = scoreToSet;
	}
}