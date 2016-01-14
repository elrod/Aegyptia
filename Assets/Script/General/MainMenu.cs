using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public string startLevel;
	public string creditScene;

	public void NewGame () {
		Application.LoadLevel (startLevel);
	}
	
	public void Credits () {
		Application.LoadLevel (creditScene);
	}

	public void QuitGame () {
		Application.Quit ();
	}
}
