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
}
