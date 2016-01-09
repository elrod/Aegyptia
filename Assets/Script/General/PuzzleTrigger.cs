using UnityEngine;
using System.Collections;

public class PuzzleTrigger : MonoBehaviour {

    public GameObject thePuzzle;
    public GameObject neededObject;

    string neededObjName = "";

	// Use this for initialization
	void Start () {
        if(neededObject != null)
            neededObjName = neededObject.name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            if(col.gameObject.GetComponent<Inventory>().Use(neededObjName) || neededObjName == "") {
                thePuzzle.SetActive(true);
            }
            else
            {
                FindObjectOfType<LevelEventsManager>().NotifyEvent("osiris", "OSIRIS_TABLET_MISSING");
                Debug.Log("You need: '" + neededObjName + "' to activate '" + thePuzzle.name + "'");
            }
        }
    }
}
