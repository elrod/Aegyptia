using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]

public class Player : MonoBehaviour {

    Controller2D controller;

	public bool isActive = true; 	

    public float jumpHeight = 4;           // How many unity units we want our player to jump
    public float timeToJumpApex = .4f;     // How much time our player will take to reach the top of the jump curve.
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;
    
    float gravity;
    float jumpVelocity;

    Vector3 velocity;
    float velocityXSmoothing;

    //vars for the mutation
    public bool CanShapeShift { get; set; }
    public GameObject NewShape { get; set; }
    GameObject anim;
    bool isHuman = true;
    float oldGravity;


	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller2D>();

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
	void Update () {

        // If we are colliding on the Y axis we don't want to accumulate gravity,
        // otherwise when we will fall, acumulated gravity will make the player disappear instead of falling
        // because it will be too fast, so when we are colliding above or below, we reset Y velocity
        if(controller.collisions.above || controller.collisions.below){
            velocity.y = 0;
        }

		if (isActive) {

            if (isHuman)
            {
                // Getting input
                Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                // Jumping logic
                if (Input.GetButtonDown("Jump") && controller.collisions.below)
                {
                    velocity.y = jumpVelocity;
                }

                float targetVelocityX = input.x * moveSpeed;
                // We use smoothDamp to gradually reach our top velocity
                velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

                if (Input.GetButtonDown("ShapeShift"))
                {

                    if (CanShapeShift)
                    {
                        ShapeShift();
                    }

                }

            }
            else
            {
                velocity.x = 0;
                velocity.y = 0;
                if (Input.GetButtonDown("ShapeShift")) // if is not human
                {
                    BackToHuman();
                }
            }
        
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

	}

	public void TurnOn(){
		isActive = true;
		gameObject.tag = "Player";
	}

	public void TurnOff(){
		isActive = false;
		gameObject.tag = "InactivePlayer";
		velocity.x = 0;
	}

    private void ShapeShift()
    {
        anim = Instantiate<GameObject>(NewShape) as GameObject; //create the new shape
        Vector3 pos = transform.position; 
        anim.transform.position = pos; //the new shape's position is the same of the player
        gameObject.GetComponent<SpriteRenderer>().enabled = false; //unactive the player and make it invisible
        gameObject.transform.parent = anim.transform; //the player became the child of the new shape
        oldGravity = gravity;
        gravity = 0;
        isHuman = false;
    }

    private void BackToHuman()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gravity = oldGravity;
        isHuman = true;
        gameObject.transform.parent = null;
        Destroy(anim);
    }

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "MovingPlatform") {
			transform.parent = coll.transform;
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.tag == "MovingPlatform") {
			transform.parent = null;
		}
	}
    public void ManageShapeOnRespawn()
    {
        if (!isHuman)
        {
            BackToHuman();
        }
    }
}
