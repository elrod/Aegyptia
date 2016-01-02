using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DangerArea : MonoBehaviour {

	public Image filter;
	public float fadingTime = .5f;
	bool active = false;
	bool red = false;
	float stepTime = 0f;
	
	void Update(){
		
		if (Input.GetButtonDown ("SwitchPlayer")) {
			active = false;
		}
		
		if (active) {
			Fade ();
			
		}
	}
	
	void Fade () { //define Fade parmeters
		float start = 0f;
		float end = .8f;
		float transparency = 0f;
		stepTime += Time.deltaTime;
		float percTime = stepTime / fadingTime;
		if (!red) {
			transparency = Mathf.Lerp (start, end, percTime);
		} else {
			transparency = Mathf.Lerp (end, start, percTime);		
		}
		Color endColor = filter.color;
		endColor.a = transparency;
		filter.color = endColor;
		if (stepTime >= fadingTime) {
			stepTime = 0f;
			red = !red;
		}
	} //end Fade
	
	
	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			active = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			active = false;
			Color endColor = filter.color;
			endColor.a = 0f;
			filter.color = endColor;
			red = false;
		}
	}
}
