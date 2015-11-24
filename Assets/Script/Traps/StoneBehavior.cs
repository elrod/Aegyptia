using UnityEngine;
using System.Collections;

public class StoneBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameInvisible(){
		SelfDestroy();
	}

	void SelfDestroy(){
		Destroy(gameObject);
	}
}
