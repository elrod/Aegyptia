using UnityEngine;
using System.Collections;

public class ParticleSelfDestroy : MonoBehaviour {

	public float timer = 1.5f;

	// Use this for initialization
	void Start () {
		Invoke("SelfDestroy", timer);
	}
	
	// Update is called once per frame
	void SelfDestroy () {
		Destroy(gameObject);
	}
}
