﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Trap))]

public class FallingStones : MonoBehaviour {

	public GameObject stonePrefab;
    public GameObject stoneFragmentsParticle;

	public int stonesToSpawn;			// 0 means infinity
	public float minSpawnTime;
	public float maxSpawnTime;

	public bool applyXOffset;
	public float xOffsetMagnitude = 0.2f;

	private int spawnedStones = 0;

	Trap trapInfo;

	// Use this for initialization
	void Start () {
		trapInfo = transform.GetComponent<Trap> ();
		if (stonesToSpawn == 0) {
			Invoke("SpawnStone", Random.Range (minSpawnTime, maxSpawnTime));
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (trapInfo.isActive && spawnedStones < stonesToSpawn && stonesToSpawn != 0) {
			Invoke("SpawnStone", Random.Range (minSpawnTime, maxSpawnTime));
			spawnedStones++;
		}
	}

	void SpawnStone(){
		GameObject go = Instantiate<GameObject>(stonePrefab) as GameObject;
		Vector3 stonePosition = transform.position;
		if(applyXOffset){
			stonePosition.x += Random.Range (-1*xOffsetMagnitude, xOffsetMagnitude);
		}
		go.transform.position = stonePosition;
		go.transform.parent = transform;
        go.GetComponent<StoneBehavior>().fragments = stoneFragmentsParticle;
		if (stonesToSpawn == 0) {
			Invoke("SpawnStone", Random.Range (minSpawnTime, maxSpawnTime));
		}
	}
}
