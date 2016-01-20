using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Trap))]
public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPreFab;
	public Transform leftPatrolPoint;
	public Transform rightPatrolPoint;
	public float patrolSpeed = 3f;
	public float followSpeed = 6f;
	public float enemyToSpawn;
	public float interval;
	public bool randomizeSpeed = true;

	float elapsedTime;
	float spawnedEnemy = 0;
	float speedOffset = .5f;
	
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
			go.transform.GetComponent<EnemyAI>().patrolSpeed = patrolSpeed;
			go.transform.GetComponent<EnemyAI>().followSpeed = followSpeed;
			go.transform.GetComponent<EnemyAI>().leftPatrolPoint = leftPatrolPoint;
			go.transform.GetComponent<EnemyAI>().rightPatrolPoint = rightPatrolPoint;
			if(randomizeSpeed){
				go.transform.GetComponent<EnemyAI>().patrolSpeed += Random.Range(-speedOffset, speedOffset);
				go.transform.GetComponent<EnemyAI>().followSpeed += Random.Range(-speedOffset, speedOffset);
			}
			spawnedEnemy++;
			elapsedTime = 0f;
		}
	}
}
