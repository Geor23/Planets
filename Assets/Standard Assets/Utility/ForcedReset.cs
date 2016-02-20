using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (GUITexture))]
public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        // if we have forced a reset ...
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
#pragma warning disable CS0618 // Type or member is obsolete
                              //... reload the scene
            Application.LoadLevelAsync(Application.loadedLevelName);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
