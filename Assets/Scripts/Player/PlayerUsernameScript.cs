using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
public class PlayerUsernameScript : MonoBehaviour {
	public InputField field;
	public Text textBox;
	public NetworkIdentity nIdentity;
	public Text textOnScreen;
	public Button button;
	public void CopyText() {
		//if (nIdentity.isLocalPlayer) {
	    	button.gameObject.SetActive (false);
		    field.gameObject.SetActive (false);
			textBox.text = field.text;
		    textOnScreen.text = field.text;
		    textOnScreen.gameObject.SetActive (true);
	//	}
	}
}
