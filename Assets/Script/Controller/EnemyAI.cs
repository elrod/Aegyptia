using UnityEngine;
using System.Collections;


[RequireComponent(typeof(EnemyController2D))]
public class EnemyAI : MonoBehaviour {

	EnemyController2D controller;

	public Transform leftPatrolPoint;
	public Transform rightPatrolPoint;
	public float patrolSpeed;
	public float followSpeed;
	public float lookAroundTime = 1.5f;
	bool goRight;
	bool patrol = true;
	bool lookAround = false;
	float startLookingAround;


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
			if(!patrol){
				startLookingAround = Time.time;
				lookAround = true;
			}
			patrol = true;
		}

		if (patrol && !lookAround) {
			PatrolMovement ();
		} else if (patrol && lookAround) {
			LookAround();
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
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
		
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}

	void FollowEnemy(){
		float direction;
		if (controller.collisions.enemyRight) {
			goRight = true;
			direction = 1f;
		} else if (controller.collisions.enemyLeft) {
			goRight = false;
			direction = -1f;
		} else {
			direction = 0f;
		}
		float targetVelocityX = direction * followSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
		
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}

	void LookAround(){
		if (Time.time - startLookingAround < lookAroundTime) {
			float direction;
			if (goRight) {
				direction = 1;
			} else {
				direction = -1;
			}
			float targetVelocityX = direction * patrolSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
			
			velocity.y += gravity * Time.deltaTime;
			controller.Move (velocity * Time.deltaTime);
		} else {
			lookAround = false;
			goRight = !goRight;
		}
	}
}
