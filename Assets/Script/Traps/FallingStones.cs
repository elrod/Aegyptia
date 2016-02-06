using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Trap))]

public class FallingStones : MonoBehaviour {

	public GameObject stonePrefab;
    public GameObject stoneFragmentsParticle;

	public int stonesToSpawn;			// 0 means infinity
	public float minSpawnTime;
	public float maxSpawnTime;

	bool infiniteStones = false;

	public bool applyXOffset;
	public float xOffsetMagnitude = 0.2f;

	private int spawnedStones = 0;

	Trap trapInfo;
    float initialMinTime;
    float initialMaxTime;
    // Use this for initialization
    void Start()
    {
        initialMinTime = minSpawnTime;
        initialMaxTime = maxSpawnTime;
		trapInfo = transform.GetComponent<Trap> ();
		if (stonesToSpawn == 0) {
			infiniteStones = true;
		}
		/*
		if (stonesToSpawn == 0) {
			Invoke("SpawnStone", Random.Range (minSpawnTime, maxSpawnTime));
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
        if (trapInfo.isActive)
        {
            if (spawnedStones < stonesToSpawn && stonesToSpawn != 0)
            {
                Invoke("SpawnStone", Random.Range(minSpawnTime, maxSpawnTime));
                spawnedStones++;
            }

            if (infiniteStones)
            {
                Invoke("SpawnStone", Random.Range(minSpawnTime, maxSpawnTime));
                infiniteStones = false;
            }
        }
        else
        {
            spawnedStones = 0;
            if (stonesToSpawn == 0)
            {
                infiniteStones = true;
            }

        }
        if (spawnedStones >= stonesToSpawn && stonesToSpawn != 0)
        {
            trapInfo.isActive = false;
        }
	}

	void SpawnStone(){
		GameObject go = Instantiate<GameObject>(stonePrefab) as GameObject;
		Vector3 stonePosition = transform.position + new Vector3(0f,0f,-0.5f);
		if(applyXOffset){
			stonePosition.x += Random.Range (-1*xOffsetMagnitude, xOffsetMagnitude);
		}
		go.transform.position = stonePosition;
		go.transform.parent = transform;
        go.GetComponent<StoneBehavior>().fragments = stoneFragmentsParticle;
		if (trapInfo.isActive && stonesToSpawn == 0) {
			Invoke("SpawnStone", Random.Range (minSpawnTime, maxSpawnTime));
		}
	}
    public void Reset()
    {
        minSpawnTime = initialMinTime;
        maxSpawnTime = initialMaxTime;
    }
}
