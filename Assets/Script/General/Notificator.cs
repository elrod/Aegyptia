using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Notificator : MonoBehaviour {

    public Text notificatorText;
    public float timeVisible = 3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Notify(string msg)
    {
        notificatorText.text = msg;
        Invoke("SelfDeactivate", timeVisible);
    }

    void SelfDeactivate()
    {
        gameObject.SetActive(false);
    }
}
