using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

    bool allowExit = false;
    float allowExitTime = 1f;

	// Use this for initialization
	void Start () {
        Invoke("EnableInput", allowExitTime);
	}
	
	// Update is called once per frame
	void Update () {
	    if(allowExit && Input.anyKeyDown)
        {
            Application.LoadLevel("TitleMenu");
        }
	}

    void EnableInput()
    {
        allowExit = true;
    }
}
