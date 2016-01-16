using UnityEngine;
using System.Collections;

public class StoneBehavior : MonoBehaviour {


    Rigidbody2D rb;
    public GameObject fragments;
    public bool persistentStone = true;
	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

	void OnBecameInvisible(){
        if (!persistentStone)
        {
            SelfDestroy();
        }
	}

	void SelfDestroy(){
        fragments.GetComponent<Fragments>().ShowParticles();
	    Destroy(gameObject);
	}
    void OnTriggerEnter2D(Collider2D col)
    {
		if (!col.gameObject.CompareTag("ZoomArea")) {
			SelfDestroy ();
		}
    }
}
