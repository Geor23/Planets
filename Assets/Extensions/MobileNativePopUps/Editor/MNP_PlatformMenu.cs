using UnityEngine;
using UnityEditor;
using System.Collections;

public class MNP_PlatformMenu : EditorWindow {

	#if UNITY_EDITOR
	
	//--------------------------------------
	//  GENERAL
	//--------------------------------------
	
	[MenuItem("Window/Stan's Assets/Mobile Native Popups/Edit Settings")]
	public static void Edit() {
		Selection.activeObject = MNP_PlatformSettings.Instance;
	}

	[MenuItem("Window/Stan's Assets/Mobile Native Popups/Documentation")]
	public static void Documentation() {
		Application.OpenURL("https://goo.gl/zdCgFx");
	}


	#endif
}
