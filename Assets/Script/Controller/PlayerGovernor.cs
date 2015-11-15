using UnityEngine;
using System.Collections;


public class PlayerGovernor : MonoBehaviour {
	
	public GameObject player1;
	public GameObject player2;

	Controller2D controller;
	Controller2D inactiveController;
	bool isP1Active;
	
	public float jumpHeight = 4;           // How many unity units we want our player to jump
	public float timeToJumpApex = .4f;     // How much time our player will take to reach the top of the jump curve.
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 6;
	
	float gravity;
	float jumpVelocity;
	
	Vector3 velocity;
	Vector3 inactiveVelocity;				
	float velocityXSmoothing;
	
	// Use this for initialization
	void Start () {

		// This is needed to understand which player is active at the beginning
		// Can be avoided if the game starts everytime with one specific player
		if (player1.Equals (GameObject.FindGameObjectWithTag ("Player"))) {
			isP1Active = true;
			controller = player1.GetComponent<Controller2D>();
				inactiveController = player2.GetComponent<Controller2D>();
		} else {
			isP1Active = false;
			controller = player2.GetComponent<Controller2D>();
			inactiveController = player1.GetComponent<Controller2D>();
		}
		
		// Ok we should get gravity and jumpVelocity from jumpHeight and timeToJumpApex
		// For gravity we will use the motion equation: delta(Movement) = v0 * time + (acceleration * time^2) / 2
		// in our case v0 = 0, time = timeToJumpApex, and delta(Movement) = jumpHeight...
		// so gravity = 2 * jumpHeight / timeToJumpApex^2
		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);     // we added a "-" because we want it to be negative
		// Now we should get jump velocity from: velocity = v0 + acceleration * time
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		// Debug.Log("gravity: " + gravity + "; jumpVelocity: " + jumpVelocity);
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		// Check if the player is changed
		if (Input.GetButtonDown("SwitchPlayer")) {
			if (isP1Active) 
				SwitchPlayer(player1, player2);
			else 
				SwitchPlayer(player2, player1);
		}
		
		// If we are colliding on the Y axis we don't want to accumulate gravity,
		// otherwise when we will fall, acumulated gravity will make the player disappear instead of falling
		// because it will be too fast, so when we are colliding above or below, we reset Y velocity
		if(controller.collisions.above || controller.collisions.below){
			velocity.y = 0;
		}
		if (inactiveController.collisions.above || inactiveController.collisions.below) {
			inactiveVelocity.y = 0;
		}
		
		// Getting input
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		
		// Jumping logic
		if(Input.GetButtonDown("Jump") && controller.collisions.below){
			velocity.y = jumpVelocity;
		}
		
		float targetVelocityX = input.x * moveSpeed;
		// We use smoothDamp to gradually reach our top velocity
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		// Apply gravity to both the speed of active and inactive player
		velocity.y += gravity * Time.deltaTime;
		inactiveVelocity.y += gravity * Time.deltaTime;

		controller.Move(velocity * Time.deltaTime);
		inactiveController.Move(inactiveVelocity*Time.deltaTime);
	}
	
	void SwitchPlayer (GameObject activeBefore, GameObject activeNow){
		activeBefore.transform.gameObject.tag = "InactivePlayer";
		activeNow.transform.gameObject.tag = "Player";
		inactiveController = activeBefore.GetComponent<Controller2D> ();
		controller = activeNow.GetComponent<Controller2D> ();
		isP1Active = !isP1Active;
	}
}
