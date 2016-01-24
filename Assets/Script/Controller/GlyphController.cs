using UnityEngine;
using System.Collections;

public class GlyphController : MonoBehaviour {

    public GameObject shape;
    public Vector3 spawnPoint;
    public ParticleSystem transormationParticleIN;
    public ParticleSystem transormationParticleOUT;

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
            other.gameObject.GetComponent<Player>().setTransformationPoint(gameObject.transform.position + spawnPoint, transormationParticleIN, transormationParticleOUT);
            
            if (!other.gameObject.GetComponent<Player>().IsHuman())
            {
                other.gameObject.GetComponent<Player>().forceBackToHuman();
            }
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
