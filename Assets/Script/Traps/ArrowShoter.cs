using UnityEngine;
using System.Collections;

public class ArrowShoter : MonoBehaviour {

    public GameObject arrowPrefab;
    public int arrowToSpawn;			// 0 means infinity
	public float minSpawnTime;
	public float maxSpawnTime;
	public bool applyYOffset;
	public float yOffsetMagnitude = 0.2f;

    private int spawnedArrows = 0;

	// Use this for initialization
	void Start () {

        Invoke("SpawnArrow",Random.Range (minSpawnTime, maxSpawnTime));
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void SpawnArrow()
    {
        GameObject go = Instantiate<GameObject>(arrowPrefab) as GameObject;
        Vector3 arrowPosition = transform.position;
        if (applyYOffset)
        {
            arrowPosition.y += Random.Range(-1 * yOffsetMagnitude, yOffsetMagnitude);
        }
        go.transform.position = arrowPosition;
        go.transform.parent = transform;
        spawnedArrows++;
        Invoke("SpawnArrow", Random.Range(minSpawnTime, maxSpawnTime));
    }
}