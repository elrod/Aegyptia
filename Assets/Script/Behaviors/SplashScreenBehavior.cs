using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreenBehavior : MonoBehaviour {

    public Image splash;
    public float splashScreenTimeout = 3f;
    public float splahScreenFadeOutTime = 0.5f;
    public float fadeOutSpeed = 10f;

    public string levelToLoad = "TitleMenu";
    public bool screenFadeOut = true;

    bool fadeOut = false;

	// Use this for initialization
	void Start () {
        Invoke("StartFadeOut", splashScreenTimeout);
	}
	
	// Update is called once per frame
	void Update () {
        if (fadeOut)
        {
            if (screenFadeOut) { 
                Color theColor = splash.color;
                theColor.a -= fadeOutSpeed * Time.deltaTime;
                if(theColor.a <= 0)
                {
                    theColor.a = 0;
                    Application.LoadLevel(levelToLoad);
                }
                splash.color = theColor;
            }
            else
            {
                Application.LoadLevel(levelToLoad);
            }
        }
	}

    void StartFadeOut()
    {
        fadeOut = true;
    }
}
