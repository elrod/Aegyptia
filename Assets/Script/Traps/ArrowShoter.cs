using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
[RequireComponent(typeof(Trap))]
public class ArrowShoter : MonoBehaviour {

    public GameObject arrowPrefab;
    public int arrowToSpawn;			// 0 means infinity
	public float minSpawnTime;
	public float maxSpawnTime;
	public bool applyYOffset;
	public float yOffsetMagnitude = 0.2f;
	public AudioClip arrowSpawn;
	AudioSource audio;

    private int spawnedArrows = 0;
    bool infiniteArrows = false;
    Trap trapInfo;
    float initialMinTime;
    float initialMaxTime;
    // Use this for initialization
    void Start()
    {
        initialMinTime = minSpawnTime;
        initialMaxTime= maxSpawnTime;
        trapInfo = transform.GetComponent<Trap>();
        if (arrowToSpawn == 0)
        {
            infiniteArrows = true;
        }
        audio = GetComponent<AudioSource> ();
        //Invoke("SpawnArrow",Random.Range (minSpawnTime, maxSpawnTime));
        
	
	}
	
	// Update is called once per frame
	void Update () {
        //if (trapInfo.isActive && (spawnedArrows < arrowToSpawn || infiniteArrows))
        //{
        //    Invoke("SpawnArrow", Random.Range(minSpawnTime, maxSpawnTime));
        //}
        //else spawnedArrows = 0;
        //if (trapInfo.isActive && spawnedArrows < arrowToSpawn && arrowToSpawn != 0)
        //{
        //    Invoke("SpawnArrow", Random.Range(minSpawnTime, maxSpawnTime));
        //    spawnedArrows++;
        //}

        if (trapInfo.isActive)
        {
            if (spawnedArrows< arrowToSpawn && arrowToSpawn != 0)
            {
                Invoke("SpawnArrow", Random.Range(minSpawnTime, maxSpawnTime));
                spawnedArrows++;
            }
            if (infiniteArrows)
            {
                Invoke("SpawnArrow", Random.Range(minSpawnTime, maxSpawnTime));
                infiniteArrows = false;
            }
        }
        else
        {
            spawnedArrows = 0;
            if (arrowToSpawn == 0)
            {
                infiniteArrows = true;
            }

        }
        if (spawnedArrows >= arrowToSpawn && arrowToSpawn != 0)
        {
            trapInfo.isActive = false;
        }
	}

    void SpawnArrow(){
		audio.clip = arrowSpawn;
		audio.Play ();
        GameObject go = Instantiate<GameObject>(arrowPrefab) as GameObject;
        Vector3 arrowPosition = transform.position;
        if (applyYOffset)
        {
            arrowPosition.y += Random.Range(-1 * yOffsetMagnitude, yOffsetMagnitude);
        }
        go.transform.position = arrowPosition;
        go.transform.parent = transform;
        //spawnedArrows++;
        //Debug.Log(gameObject.name + " Active " + trapInfo.isActive);
        if (trapInfo.isActive && arrowToSpawn == 0)
        {
            Invoke("SpawnArrow", Random.Range(minSpawnTime, maxSpawnTime));
        }
        
    }

    public void Reset()
    {
        minSpawnTime = initialMinTime;
        maxSpawnTime = initialMaxTime;
    }
}