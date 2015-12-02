using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public string startLevel;
	public string sceneSelect;

	public void NewGame () {
		Application.LoadLevel (startLevel);
	}
	
	public void SceneSelect () {
		Application.LoadLevel (sceneSelect);
	}

	public void QuitGame () {
		Application.Quit ();
	}
}
