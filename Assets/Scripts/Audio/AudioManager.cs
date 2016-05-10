using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{


    public AudioSource[] allMyAudioSources;
    public AudioSource currentClip;

    PlayerConfig playerConfig;
    // Use this for initialization
    void Start()
    {
        playerConfig = PlayerConfig.singleton;
        if (playerConfig.GetObserver() == true)
        {
            //AudioSource[] allMyAudioSources = GetComponents<AudioSource>();
            if (allMyAudioSources.Length > 0)
            {
                currentClip = allMyAudioSources[0];
                Debug.Log("Current Clip " + currentClip.name);
                allMyAudioSources[0].Play();
                allMyAudioSources[0].loop = true;
            }

            if (allMyAudioSources.Length > 1)
            {
                allMyAudioSources[1].Play();
                allMyAudioSources[1].loop = true;
            Debug.Log("Current Clip " + allMyAudioSources[1].name);

        }
            }
        else return;
    }

    void Update()
    {

        //Select clip 
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            currentClip = allMyAudioSources[0];
            Debug.Log("currentClip " + currentClip.name);

        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            currentClip = allMyAudioSources[1];
            Debug.Log("currentClip " + currentClip.name);

        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            currentClip = allMyAudioSources[2];
            Debug.Log("currentClip " + currentClip.name);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            currentClip = allMyAudioSources[3];
            Debug.Log("currentClip " + currentClip.name);
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            currentClip = allMyAudioSources[4];
            Debug.Log("currentClip " + currentClip.name);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            currentClip = allMyAudioSources[5];
            Debug.Log("currentClip " + currentClip.name);
        }

        //Volume Up
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            currentClip.volume = currentClip.volume + 0.1F;
            Debug.Log("Volume Up " + currentClip.volume);
        }

        //Volume Down
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            currentClip.volume = currentClip.volume - 0.1F;
            Debug.Log("Volume Down " + currentClip.volume);
        }

        //Play Clip

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            currentClip.Play();
            currentClip.loop = true;
            Debug.Log("Play Clip " + currentClip.name);
        }

        //Stop Clip
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            currentClip.Stop();
            Debug.Log("Stop " + currentClip.name);
        }

        //Mute Clip
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            if (currentClip.mute)
            {
                currentClip.mute = false;
                Debug.Log("Un Mute " + currentClip.name);
            }
            else
            {
                currentClip.mute = true;
                Debug.Log("Mute " + currentClip.name);
            }
        }
    }
}