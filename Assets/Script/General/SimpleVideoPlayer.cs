    using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleVideoPlayer : MonoBehaviour {

    public string levelToLoad = "";

    MovieTexture movie;
    AudioSource audio;
    bool played = false;

	// Use this for initialization
	void Start () {
        RawImage img = GetComponent<RawImage>();
        movie = (MovieTexture)img.mainTexture;
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (played && !movie.isPlaying)
        {
            // Load next level
            Application.LoadLevel(levelToLoad);
        }
        if (!played && !movie.isPlaying && movie.isReadyToPlay)
        {
            movie.Play();
            audio.Play();
            played = true;
        }
        if (Input.anyKeyDown)
        {
            Application.LoadLevel(levelToLoad);
        }
    }
}
