using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuControllerSupport : MonoBehaviour {

	public Button[] buttonList;
	public bool horizontal = true;
	int index = 0;
	float elapsedTime = 0f;
	float interval = .1f;

	// Use this for initialization
	void Start () {
		buttonList [index].Select ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (elapsedTime > interval) {
			Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			if ((horizontal && input.x > 0) || (!horizontal && input.y < 0)) {
				if (index < buttonList.Length - 1) {
					index++;
					buttonList [index].Select ();
				}
			} else if ((horizontal && input.x < 0) || (!horizontal && input.y > 0)) {
				if (index > 0) {
					index--;
					buttonList [index].Select ();
				}
			} 
			if (Input.GetButtonDown ("Jump")) {
				buttonList [index].onClick.Invoke ();
			}
			elapsedTime = 0f;
		} else {
			elapsedTime += Time.deltaTime;
		}
	}
}
