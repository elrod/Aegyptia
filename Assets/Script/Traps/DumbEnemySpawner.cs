using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Trap))]

public class DumbEnemySpawner : MonoBehaviour {

	public GameObject enemyPreFab;
	public GameObject playerToFollow;
	public float enemyToSpawn;
	public float interval;
	float elapsedTime;
	float spawnedEnemy = 0;
	float speedOffset = 1f;

	Trap trapInfo;

	// Use this for initialization
	void Start () {
		trapInfo = transform.GetComponent<Trap> ();
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if ((spawnedEnemy < enemyToSpawn || enemyToSpawn == 0) && trapInfo.isActive && elapsedTime >= interval) {
			GameObject go = Instantiate<GameObject>(enemyPreFab) as GameObject;
			go.transform.position = transform.position;
			go.transform.GetComponent<FollowingEnemy>().moveSpeed += Random.Range(-speedOffset, speedOffset);
			go.GetComponent<FollowingEnemy>().playerToFollow = playerToFollow;
			spawnedEnemy++;
			elapsedTime = 0f;
		}
	}
}
