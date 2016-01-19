using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

    Collector collector;
	bool destroy = false;

	// Use this for initialization
	void Start () {
        collector = FindObjectOfType<Collector>();
        if(collector == null)
        {
            Debug.LogError("COLLECTABLE COULD NOT FIND A COLLECTOR... IS IT IN THE SCENE?");
        }
	}

	void Update() {
		if (destroy && !GetComponent<AudioSource>().isPlaying) {
			Destroy (gameObject);
		}
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
			GetComponent<AudioSource>().Play ();

            collector.ObjectCollected();
			destroy = true;
			GetComponent<Renderer>().enabled = false;
			GetComponent<Collider2D>().enabled = false;
        }
    }
}
