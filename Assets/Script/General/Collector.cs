using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour {

    int toCollect;
    int collected;

	// Use this for initialization
	void Start () {
        toCollect = FindObjectsOfType<Collectable>().Length;
        collected = 0;
	}
	
	public void ObjectCollected()
    {
        collected++;
    }

    public int getCollected()
    {
        return collected;
    }

    public int getTotal()
    {
        return toCollect;
    }
}
