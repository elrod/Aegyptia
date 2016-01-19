using UnityEngine;
using System.Collections;

public class Door : Tool {

	public bool isClosed = true;
	public bool automaticWithObject;
	public GameObject obj;
	public float speed = 1;
	bool used = false;
	bool opening = false;
	bool closing = false;
	GameObject door;

	void Start () {
		door = transform.GetChild (0).gameObject;
	}

	void Update(){
		if (opening) {
			Open ();
		}
		if (closing) {
			Close();
		}
	}
	
	public override void Use(){
		if (isClosed) {
			opening = true;
			closing = false;
		} else {
			closing = true;
			opening = false;
		}
		GetComponent<AudioSource> ().Play ();
	}
	
	void Open(){
		//door.SetActive (false);
		if (door.transform.eulerAngles.y < 90) {
			door.transform.Rotate(new Vector3 (0, speed, 0));
		} else {
			door.transform.eulerAngles = new Vector3(0, 90, 0);
			opening = false;
		}
		isClosed = false;
	}
	
	void Close(){
		//door.SetActive (true);
		if (door.transform.eulerAngles.y < 350) {
			door.transform.Rotate(new Vector3 (0, -speed, 0));
		} else {
			door.transform.eulerAngles = Vector3.zero;
			closing = false;
		}
		isClosed = true;
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (automaticWithObject && coll.gameObject.CompareTag ("Player")) {
			if(coll.gameObject.GetComponent<Inventory>().Has(obj.name)){
				coll.gameObject.GetComponent<Inventory>().Use(obj.name);
				used = true;
			}
		}
	}

	// This does not work because in the scene I rotate the object, and the collider 2D disappear
	void OnTriggerExit2D(Collider2D coll){
		if (automaticWithObject && used && coll.gameObject.CompareTag ("Player")) {
			if (!opening && ! closing){
				Use ();
				used = false;
			}
		}
	}
}
