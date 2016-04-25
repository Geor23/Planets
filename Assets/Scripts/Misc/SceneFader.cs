using UnityEngine;
using System.Collections;

public class SceneFader : MonoBehaviour {

    public Texture2D fadeOutTexture;  //texture that will overlay the screen. 
    public float fadeSpeed = 0.8f;   // fading speed

    private int drawDepth = -1000; //texetures order in the draw hierarchy: low number means it renders on top
    private float alpha = 1.0f;    //textures alpha value between 0 and 1
    private int fadeDir = -1;       //direction to fade:in = -1 , out - 1

    // void Start(){
    //     BeginFade(-1);
    // }

    void OnGUI ()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade (int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    void OnLevelWasLoaded ()
    {
        BeginFade(-1);
    }


}
