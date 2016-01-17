using UnityEngine;
using System.Collections;

public class LevelEndPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoToMainMenu()
    {
        Application.LoadLevel("TitleMenu");
    }

    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void GoToNextLevel()
    {
        Application.LoadLevel(FindObjectOfType<LevelManager>().nextLevel);
    }
}
