using UnityEngine;
using System.Collections;

public class FallingStones : MonoBehaviour {

	public GameObject stonePrefab;

	public int stonesToSpawn;			// 0 means infinity
	public float minSpawnTime;
	public float maxSpawnTime;

	public bool applyXOffset;
	public float xOffsetMagnitude = 0.2f;

	private int spawnedStones = 0;

	// Use this for initialization
	void Start () {
		Invoke("SpawnStone", Random.Range (minSpawnTime, maxSpawnTime));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SpawnStone(){
		if(spawnedStones < stonesToSpawn || stonesToSpawn == 0){
			GameObject go = Instantiate<GameObject>(stonePrefab) as GameObject;
			Vector3 stonePosition = transform.position;
			if(applyXOffset){
				stonePosition.x += Random.Range (-1*xOffsetMagnitude, xOffsetMagnitude);
			}
			go.transform.position = stonePosition;
			go.transform.parent = transform;
			spawnedStones++;
			Invoke("SpawnStone", Random.Range (minSpawnTime, maxSpawnTime));
		}
	}
}
