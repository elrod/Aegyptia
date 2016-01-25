using UnityEngine;
using System.Collections;

public class AnimatedDoor : Tool {

	public bool isClosed = true;
	public bool automaticWithObject;
	public GameObject obj;
	bool used = false;
	bool opening = false;
	bool closing = false;

	Animator anim;

	void Start () {
		//door = transform.GetChild (0).gameObject;
		anim = GetComponent<Animator>();
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
		//Debug.Log ("OPEN");
		anim.SetTrigger("open");
		opening = false;
		isClosed = false;
	}
	
	void Close(){
		//Debug.Log ("CLOSE");
		anim.SetTrigger("close");
		closing = false;
		isClosed = true;
	}
	
	void OnTriggerEnter2D(Collider2D coll){
		if (automaticWithObject && coll.gameObject.CompareTag ("Player")) {
			if(coll.gameObject.GetComponent<Inventory>().Has(obj.name)){
				coll.gameObject.GetComponent<Inventory>().Use(obj.name);
				opening = true;
				used = true;
				gameObject.layer = LayerMask.NameToLayer("Default");
			}
		}
	}
	
	// This does not work because in the scene I rotate the object, and the collider 2D disappear
	void OnTriggerExit2D(Collider2D coll){
		if (automaticWithObject && used && coll.gameObject.CompareTag ("Player")) {
			if (!opening && ! closing){
				closing = true;
				Use ();
				used = false;
				gameObject.layer = LayerMask.NameToLayer("Obstacles");
			}
		}
	}
}
