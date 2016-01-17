using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

    Collector collector;

	// Use this for initialization
	void Start () {
        collector = FindObjectOfType<Collector>();
        if(collector == null)
        {
            Debug.LogError("COLLECTABLE COULD NOT FIND A COLLECTOR... IS IT IN THE SCENE?");
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            collector.ObjectCollected();
            Destroy(gameObject);
        }
    }
}
