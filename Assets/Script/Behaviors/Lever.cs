﻿using UnityEngine;
using System.Collections;

public class Lever : Tool {

	[System.Serializable]
	public struct TileArea
	{
		public GameObject area;
		public float minY;
		public float maxY;
	}
	
	public TileArea[] destroyAreas;
	public TileArea[] createAreas;
	
	int currentArea = 0;
	bool currentAreaChanged = true;
	
	float currentY;
	
	public Tool[] toolsToUse;
	bool active = false;
	bool used = false;
	
	public float interval = 1f;
	float elapsedTime = 0f;
	bool destroying = false;
	bool creating = false;
	bool start = false;

	public float rotationSpeed = 20f;
	bool rotating = false;
	Transform rotationCentre;
	bool rotationDirection = true;
	
	public bool reversible;
	
	
	// Use this for initialization
	void Start () {
		if (destroyAreas.Length > 0) {
			destroying = true;
		} else if (createAreas.Length > 0) {
			creating = true;
		}
		SetMinAndMax ();
		rotationCentre = transform.GetChild (0);
	}
	
	// Update is called once per frame
	void Update () {
		
		// Surely not the best way to call it, but I've rewritten this so many times that 
		// at the moement I don't want to change it.
		if (Input.GetKeyDown(KeyCode.Q)){
			if (active) {
				start = true;
				rotating = true;
				rotationDirection = true;
			}
		}
		if (start){
			Use ();
		}
		if (rotating) {
			RotateLever(rotationDirection);
		}
	}
	
	public override void Use(){
		// the lever is designed to be used only one time.
		if (!used){
			// First there is the part in which we destroy the existing tiles (if it has to be done)
			if(destroying){
				DestroyRow();
			}
			
			if(creating){
				CreateRow();
			}
			
			// Finally use the tools
			if (!destroying && !creating){
				foreach(Tool tool in toolsToUse){
					tool.Use();
				}
				used = true;
				start = false;
				if(reversible){
					Init();
				}
			}
		}
	}
	
	void DestroyRow(){
		// If it is a new area the bounds are set
		float numChild = destroyAreas [currentArea].area.transform.childCount;
		if(currentAreaChanged){

			currentY = destroyAreas[currentArea].maxY;
			currentAreaChanged = false;
		}
		
		for (int i=0; i<numChild; i++) {
			if(destroyAreas [currentArea].area.transform.GetChild(i).position.y == currentY){
				destroyAreas [currentArea].area.transform.GetChild(i).gameObject.SetActive (false);
			}
		}
		
		// If it is not the last row wait the interval and then move on to the next row.
		// Otherwise if it wasn't the last area increment currentArea, if it was the last
		// reset the variables and go active the spawning section.
		if (currentY != destroyAreas [currentArea].minY) {
			if (elapsedTime < interval) {
				elapsedTime += Time.deltaTime;
			} else {
				currentY -= 1f;
				elapsedTime = 0f;
			}
		} else {
			if (currentArea + 1 < destroyAreas.Length) {
				currentArea++;
			} else {
				destroying = false;
				if(createAreas.Length>0){
					creating = true;
				}
				currentArea = 0;
				currentY = 0;
			}
			currentAreaChanged = true;
		}
	}
	
	void CreateRow(){
		// If it is a new area the bounds are set
		float numChild = createAreas [currentArea].area.transform.childCount;
		if(currentAreaChanged){
			currentY = createAreas[currentArea].minY;
			currentAreaChanged = false;
		}
		
		for (int i=0; i<numChild; i++) {
			if(createAreas [currentArea].area.transform.GetChild(i).position.y == currentY){
				createAreas [currentArea].area.transform.GetChild(i).gameObject.SetActive (true);
			}
		}
		
		// If it is not the last row wait the interval and then move on to the next row.
		// Otherwise if it wasn't the last area increment currentArea, if it was the last
		// reset the variables and go active the spawning section.
		if (currentY != createAreas [currentArea].maxY) {
			if (elapsedTime < interval) {
				elapsedTime += Time.deltaTime;
			} else {
				currentY += 1f;
				elapsedTime = 0f;
			}
		} else {
			if (currentArea + 1 < createAreas.Length) {
				currentArea++;
			} else {
				creating = false;
				currentArea = 0;
				currentY = 0;
			}
			currentAreaChanged = true;
		}
	}

	void SetMinAndMax(){
		for (int i=0; i<destroyAreas.Length; i++) {
			float tmpMin = 10000;
			float tmpMax = -10000;
			float numChild = destroyAreas [i].area.transform.childCount;
			for (int k=0; k<numChild; k++){
				if(destroyAreas [i].area.transform.GetChild(k).position.y>tmpMax){
					tmpMax = destroyAreas [i].area.transform.GetChild(k).position.y;
				}
				if(destroyAreas [i].area.transform.GetChild(k).position.y<tmpMin){
					tmpMin = destroyAreas [i].area.transform.GetChild(k).position.y;
				}
			}
			destroyAreas[i].maxY = tmpMax;
			destroyAreas[i].minY = tmpMin;
		}

		for (int i=0; i<createAreas.Length; i++) {
			float tmpMin = 10000;
			float tmpMax = -10000;
			float numChild = createAreas [i].area.transform.childCount;
			for (int k=0; k<numChild; k++){
				if(createAreas [i].area.transform.GetChild(k).position.y>tmpMax){
					tmpMax = createAreas [i].area.transform.GetChild(k).position.y;
				}
				if(createAreas [i].area.transform.GetChild(k).position.y<tmpMin){
					tmpMin = createAreas [i].area.transform.GetChild(k).position.y;
				}
			}
			createAreas[i].maxY = tmpMax;
			createAreas[i].minY = tmpMin;
		}
	}

	void RotateLever(bool direct){
		if (direct) {
			if (transform.eulerAngles.z == 0 || transform.eulerAngles.z > 300){
				transform.RotateAround (rotationCentre.position, Vector3.forward, -rotationSpeed * Time.deltaTime);
			}
			if (transform.eulerAngles.z < 300){
				transform.eulerAngles = new Vector3(0f, 0f, 300f);
				rotating = false;
			}
		} else {
			if (transform.eulerAngles.z >= 300){
				transform.RotateAround (rotationCentre.position, Vector3.forward, rotationSpeed * Time.deltaTime);
			}
			if (transform.eulerAngles.z < 1){
				transform.eulerAngles = Vector3.zero;
				rotating = false;
			}
		}
	}

	void Init(){
		TileArea[] tmp = destroyAreas;
		destroyAreas = createAreas;
		createAreas = tmp;

		if (destroyAreas.Length > 0) {
			destroying = true;
		} else if (createAreas.Length > 0) {
			creating = true;
		}
		SetMinAndMax ();
		used = false;
		rotating = true;
		rotationDirection = false;
	}
	
	void OnTriggerEnter2D (Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			active = true;
		}
	}
	
	void OnTriggerExit2D (Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			active = false;
		}
	}
	
}

