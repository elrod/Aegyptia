using UnityEngine;
using System.Collections;

public class LevelExitDoor : MonoBehaviour {

    public bool objectRequired = true; 
	public GameObject obj;

	void Start () {
	}

	void Update(){
	}

	void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.CompareTag("Player"))
        {
            if (objectRequired)
            {
                if (coll.gameObject.GetComponent<Inventory>().Has(obj.name))
                {
                    coll.gameObject.GetComponent<Inventory>().Use(obj.name);
                    if (coll.gameObject.name.Equals("Isis")) { 
                        FindObjectOfType<LevelEventsManager>().NotifyEvent("isis", "ISIS_LEVEL_FINISHED");
                    }
                    FindObjectOfType<LevelManager>().PlayerExit();
                }
                else
                {
                    if (coll.gameObject.name.Equals("Isis"))
                    {
                        FindObjectOfType<LevelEventsManager>().NotifyEvent("isis", "ISIS_KEY_MISSING");
                    }
                }
            }
            else
            {
                FindObjectOfType<LevelManager>().PlayerExit();
            }
        }
	}
}
