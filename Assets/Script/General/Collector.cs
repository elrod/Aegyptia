using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Collector : MonoBehaviour {

    public Text collectedText;
    public Text totalText;

    int toCollect;
    int collected;

	// Use this for initialization
	void Start () {
        toCollect = FindObjectsOfType<Collectable>().Length;
        collected = 0;
        totalText.text = toCollect.ToString();
        if(collectedText == null)
        {
            Debug.LogError("COLLECTED TEXT NOT SET IN collector.cs, SET IT FROM INSPECTOR");
        }
        if (totalText == null)
        {
            Debug.LogError("COLLECTED TEXT NOT SET IN collector.cs, SET IT FROM INSPECTOR");
        }
    }
	
	public void ObjectCollected()
    {
        collected++;
        collectedText.text = collected.ToString();
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
