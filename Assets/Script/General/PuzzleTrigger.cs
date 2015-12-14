using UnityEngine;
using System.Collections;

public class PuzzleTrigger : MonoBehaviour {

    public GameObject thePuzzle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            thePuzzle.SetActive(true);
        }
    }
}
