using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {


	public AudioSource[] allMyAudioSources;
	public AudioSource clip1;
	public AudioSource clip2;
	public AudioSource clip3;
	public AudioSource clip4;
    public AudioSource clip5;
    public AudioSource clip6;
	public AudioSource currentClip;

	// Use this for initialization
	void Start () {
 		//AudioSource[] allMyAudioSources = GetComponents<AudioSource>();
 		clip1 = allMyAudioSources[0];
        Debug.Log("Clip 1 " + clip1.name);

 		clip2 = allMyAudioSources[1];
        Debug.Log("Clip 2 " + clip2.name);

 		clip3 = allMyAudioSources[2];
        Debug.Log("Clip 3 " + clip3.name);

 		clip4 = allMyAudioSources[3];
        Debug.Log("Clip 4 " + clip4.name);

        clip5 = allMyAudioSources[4];
        Debug.Log("Clip 4 " + clip4.name);

        clip6 = allMyAudioSources[5];
        Debug.Log("Clip 4 " + clip4.name);

        currentClip = allMyAudioSources[0];
        Debug.Log("Current Clip " + currentClip.name);

	}
	
    void Update () {

//Select clip 
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
        	currentClip = clip1;
            Debug.Log("currentClip " + currentClip.name);

        }
  		
  		if (Input.GetKeyDown(KeyCode.Alpha2)) {   
            currentClip = clip2;
            Debug.Log("currentClip " + currentClip.name);
		
        }
    
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
        	currentClip = clip3;;        
            Debug.Log("currentClip " + currentClip.name);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) {
        	currentClip = clip4;           
            Debug.Log("currentClip " + currentClip.name);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            currentClip = clip5;           
            Debug.Log("currentClip " + currentClip.name);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            currentClip = clip6;           
            Debug.Log("currentClip " + currentClip.name);
        }
        
//Volume Up
        if (Input.GetKeyDown(KeyCode.Equals)) {   
        	currentClip.volume = currentClip.volume + 0.1F;
            Debug.Log("Volume Up " + currentClip.volume);
        }

//Volume Down
        if (Input.GetKeyDown(KeyCode.Minus)) {
           currentClip.volume = currentClip.volume - 0.1F;
            Debug.Log("Volume Down " + currentClip.volume);       
        }

//Play Clip

        if (Input.GetKeyDown(KeyCode.P)) {
        	currentClip.Play();
            currentClip.loop = true;
            Debug.Log("Play Clip " + currentClip.name);        
        }

//Stop Clip
        if (Input.GetKeyDown(KeyCode.O)) {
           	currentClip.Stop();
            Debug.Log("Stop " + currentClip.name);
           }


    }

	void onScoreDifference() {
		//if score difference between teams greater than X volume of winning 
		//team clip increases
	}

}
