using UnityEngine;
using System.Collections;

public class StoneBehavior : MonoBehaviour {


    Rigidbody2D rb;
    public GameObject fragments;
    public bool persistentStone = true;
    public float destroyAfterTime = 0 ; // 0 means infinity
	// Use this for initialization
	void Start () {
        if (destroyAfterTime>0)
        {
            Invoke("SelfDestroy",  Random.Range(destroyAfterTime,destroyAfterTime +1));
        }
	   
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
