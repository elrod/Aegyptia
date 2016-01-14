using UnityEngine;
using System.Collections;

public class PuzzleTrigger : MonoBehaviour {

    public GameObject thePuzzle;
    public GameObject neededObject;

	bool canMove = false;
	public float deltaY = 3f;
	Vector3 startPosition;
    string neededObjName = "";

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
        if(neededObject != null)
            neededObjName = neededObject.name;
	}
	
	// Update is called once per frame
	void Update () {
		TabletPuzzle puzzle = thePuzzle.GetComponentInChildren<TabletPuzzle>();
		if(canMove && transform.position.y < startPosition.y + deltaY){
			transform.Translate(Vector3.up * Time.deltaTime);
		}
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

	public void SetCanMove(bool value){
		canMove = value;
	}
}
