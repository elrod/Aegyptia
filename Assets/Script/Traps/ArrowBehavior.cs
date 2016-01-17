using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class ArrowBehavior : MonoBehaviour {

    public float velocity = 1;
	public AudioClip arrowHit;
	AudioSource audio;
	bool moving = true;
	bool hit = false;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (moving) {
			Vector3 pos = transform.position;
			pos += (new Vector3 (1, 0, 0)) * velocity * Time.deltaTime;
			transform.position = pos;
		} else if (!audio.isPlaying && hit) {
			Destroy (gameObject);
		}
	}

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    Destroy(gameObject);
    //}

    void OnTriggerEnter2D(Collider2D coll){
		if (!coll.tag.Equals("ArrowShoter") && !coll.tag.Equals("ZoomArea")){
			moving = false;
			GetComponent<Renderer>().enabled = false;
			GetComponent<BoxCollider2D>().enabled = false;
			audio.clip = arrowHit;
			audio.Play ();
			hit = true;
        }
    }
}
