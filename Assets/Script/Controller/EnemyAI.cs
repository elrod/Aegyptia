using UnityEngine;
using System.Collections;


[RequireComponent(typeof(EnemyController2D))]
public class EnemyAI : MonoBehaviour {

	EnemyController2D controller;

	public Transform leftPatrolPoint;
	public Transform rightPatrolPoint;
	public float patrolSpeed;
	public float followSpeed;
	bool goRight;
	bool patrol = true;

	
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	
	float gravity = -50;
	Vector3 velocity;
	float velocityXSmoothing;


	// Use this for initialization
	void Start () {
		controller = GetComponent<EnemyController2D>();
		if (transform.position.x == rightPatrolPoint.position.x) {
			goRight = false;
		} else {
			goRight = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(controller.collisions.above || controller.collisions.below){
			velocity.y = 0;
		}



		if (controller.collisions.enemyLeft || controller.collisions.enemyRight) {
			patrol = false;
		} else {
			patrol = true;
		}

		if (patrol) {
			PatrolMovement ();
		} else {
			FollowEnemy();
		}


	}

	void PatrolMovement(){
		if (transform.position.x <= leftPatrolPoint.position.x) {
			goRight = true;
		} else if (transform.position.x >= rightPatrolPoint.position.x) {
			goRight = false;
		}
		
		float direction;
		if (goRight) {
			direction = 1;
		} else {
			direction = -1;
		}
		float targetVelocityX = direction * patrolSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}

	void FollowEnemy(){
		if (controller.collisions.enemyRight) {
			goRight = true;
		} else {
			goRight = false;
		}

		float direction;
		if (goRight) {
			direction = 1;
		} else {
			direction = -1;
		}
		float targetVelocityX = direction * followSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}
}
