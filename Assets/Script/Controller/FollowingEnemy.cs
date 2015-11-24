using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]

public class FollowingEnemy : MonoBehaviour {

	Controller2D controller;
	public GameObject playerToFollow;
	
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	public float moveSpeed = 4;
	
	float gravity = -50;

	Vector3 velocity;
	float velocityXSmoothing;
	
	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
		// If we are colliding on the Y axis we don't want to accumulate gravity,
		// otherwise when we will fall, acumulated gravity will make the player disappear instead of falling
		// because it will be too fast, so when we are colliding above or below, we reset Y velocity
		if(controller.collisions.above || controller.collisions.below){
			velocity.y = 0;
		}

		float directionX = Mathf.Sign (playerToFollow.transform.position.x - transform.position.x);
		float targetVelocityX = directionX * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
