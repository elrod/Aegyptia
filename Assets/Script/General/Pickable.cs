﻿using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour {

	public Sprite inventoryPicture;
	public float resizeFactor = 4f;
	public float pickUpTime = 1f;
	public float movementStep = .1f;
	public float rotationSpeed = 25f;

	Renderer theRenderer;
	Collider2D theCollider;
	bool showPickUp = false;
	float elapsedTime = 0f;

	// Use this for initialization
	void Start () {
		theRenderer = GetComponent<Renderer>();
		theCollider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (showPickUp) {
			Show();
		}
	}

	void Show(){
		if (elapsedTime < pickUpTime) {
			elapsedTime += Time.deltaTime;
			float percTime = elapsedTime / pickUpTime;
			float newScale = Mathf.Lerp (1f, resizeFactor, percTime);
			transform.localScale = new Vector3 (newScale, newScale, newScale);
			transform.position += new Vector3 (0f, movementStep, 0f);
			transform.Rotate(new Vector3(0f, rotationSpeed, 0f));
		} else if (elapsedTime < 2 * pickUpTime) {
			elapsedTime += Time.deltaTime;
			float percTime = (elapsedTime - pickUpTime) / pickUpTime;
			float newScale = Mathf.Lerp (resizeFactor, 1f, percTime);
			transform.localScale = new Vector3 (newScale, newScale, newScale);
			transform.position += new Vector3 (0f, -movementStep, 0f);
			transform.Rotate(new Vector3(0f, rotationSpeed, 0f));
		} else {
			elapsedTime = 0f;
			theRenderer.enabled = false;
			showPickUp = false;
			transform.position += new Vector3(0f, 0f, +2f);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Player"){
			col.gameObject.GetComponent<Inventory>().PickUp(gameObject, inventoryPicture);
			GetComponent<AudioSource>().Play ();
			theCollider.enabled = false;
			showPickUp = true;
			transform.position += new Vector3(0f, 0f, -2f);
			//theRenderer.enabled = false;
		}
	}
}
