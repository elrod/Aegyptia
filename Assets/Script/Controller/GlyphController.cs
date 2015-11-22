using UnityEngine;
using System.Collections;

public class GlyphController : MonoBehaviour {

    public GameObject shape;

	// Use this for initialization
	void Start () {

       // shape = GetComponent<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().NewShape = shape;
            other.gameObject.GetComponent<Player>().CanShapeShift = true;
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().CanShapeShift = false;
        }
    }
}
