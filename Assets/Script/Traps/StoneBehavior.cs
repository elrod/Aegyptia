using UnityEngine;
using System.Collections;

public class StoneBehavior : MonoBehaviour {


    Rigidbody2D rb;
    public GameObject fragments;
	// Use this for initialization
	void Start () {
	    rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameInvisible(){
		SelfDestroy();
	}

	void SelfDestroy(){

        Instantiate(fragments, gameObject.transform.position, gameObject.transform.rotation);
		Destroy(gameObject);
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        SelfDestroy();
    }
}
