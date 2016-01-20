using UnityEngine;
using System.Collections;

public class Fragments : MonoBehaviour {

    public bool selfDisable = true;
    public float timer = 1f;

    ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

	// Use this for initialization
	public void ShowParticles () {
        if(particles != null) { 
			particles.Play();
            Invoke("SelfDisable", timer);
        }
    }
	
	void SelfDisable(){
        particles.Stop();
    }

	void OnTriggerEnter2D(Collider2D coll){
		if(coll.gameObject.CompareTag ("Stone")){
			GetComponent<AudioSource>().Play ();
		}
	}
}
