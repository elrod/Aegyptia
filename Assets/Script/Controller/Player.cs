using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (AudioSource))]
public class Player : MonoBehaviour {

    Controller2D controller;

	public bool isActive = true; 	

    public float jumpHeight = 4;           // How many unity units we want our player to jump
    public float timeToJumpApex = .4f;     // How much time our player will take to reach the top of the jump curve.
    public float moveSpeed = 6;            // Max movement speed

    public string idleAnimation = "idle";
    public string walkAnimation = "walk";
    public string jumpStart = "jump-salto";
    public string jumpFly = "jump-volo";
    public string jumpLand = "jump-atterro";

	public AudioClip[] audioClip;
	AudioSource audio;
	int clip_jump = 0;

    public bool frontRight = true;

    string currentAnimation = "";

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    
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
    Vector2 humanColliderSize;
    Vector2 humanColliderOffset;
    Vector3 tranformationPoint;

    SkeletonAnimation spineAnim;
    string curr_anim;
    bool jumping = false;

	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller2D>();
		audio = GetComponent<AudioSource> ();

        // Ok we should get gravity and jumpVelocity from jumpHeight and timeToJumpApex
        // For gravity we will use the motion equation: delta(Movement) = v0 * time + (acceleration * time^2) / 2
        // in our case v0 = 0, time = timeToJumpApex, and delta(Movement) = jumpHeight...
        // so gravity = 2 * jumpHeight / timeToJumpApex^2
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);     // we added a "-" because we want it to be negative
        // Now we should get jump velocity from: velocity = v0 + acceleration * time
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        // Debug.Log("gravity: " + gravity + "; jumpVelocity: " + jumpVelocity);
        spineAnim = GetComponent<SkeletonAnimation>();
		spineAnim.Reset();
        spineAnim.state.SetAnimation(0, "idle", true);
        curr_anim = "idle";

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

                if (Input.GetButtonDown("Jump") && controller.collisions.below)
                {
					PlaySound (clip_jump);
                    velocity.y = jumpVelocity;
                }

                UpdateAnimation(input);

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
                    if (CanShapeShift)
                        BackToHuman();
                }
            }
        
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

	}

    void UpdateAnimation(Vector2 input)
    {
        /*Bounds bounds = GetComponent<Collider2D>().bounds;
        bounds.Expand(-0.30f);
        Vector3 rayOrigin = new Vector3((bounds.min.x + bounds.max.x) / 2f, bounds.min.y, transform.position.z);
        Debug.DrawRay(rayOrigin, Vector3.down, Color.green, 1f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 1f, LayerMask.NameToLayer("Obstacles"));*/
        if (input.x == 0)
        {
			if(spineAnim.state.GetCurrent(0) == null || spineAnim.state.GetCurrent(0).Animation.name == walkAnimation) SetAnimation(idleAnimation, true);
		}
        else
        {
            if(input.x > 0)
            {
                spineAnim.skeleton.FlipX = frontRight ? false : true;
            }
            else if(input.x < 0)
            {
               spineAnim.skeleton.FlipX = frontRight ? true : false;
            }
            if (!jumping) SetAnimation(walkAnimation, true);
        }
        if (Input.GetButtonDown("Jump") && controller.collisions.below)
        {
            //Debug.Log("salto");
            SetAnimation(jumpStart, false);
            jumping = true;
        }
        else if (jumping && controller.collisions.below)
        {
            //Debug.Log("atterro");
            SetAnimation(jumpLand, false);
            jumping = false;
        }
        else if (jumping)
        {
            //Debug.Log("volo");
            SetAnimation(jumpFly, true);

        }
    }

    void SetAnimation(string anim, bool loop)
    {
        if (currentAnimation != anim)
        {
            //Debug.Log("NUOVA ANIMAZIONE:" + anim);
            spineAnim.state.SetAnimation(0, anim, loop);
            currentAnimation = anim;
        }
    }

    public void TurnOn(){
		isActive = true;
		gameObject.tag = "Player";
		if (transform.parent != null && transform.parent.GetComponent<Animal>() != null) {
			transform.parent.GetComponent<Animal>().TurnOn();
		}
	}

	public void TurnOff(){
		isActive = false;
		gameObject.tag = "InactivePlayer";
		velocity.x = 0;
		if (transform.parent != null && transform.parent.GetComponent<Animal>() != null) {
			transform.parent.GetComponent<Animal>().TurnOff();
		}
	}

    private void ShapeShift()
    {
        anim = Instantiate<GameObject>(NewShape) as GameObject; //create the new shape
        //Vector3 pos = transform.position;
        Vector3 pos = tranformationPoint;
        //pos.z = 1;
        gameObject.transform.position = pos;
        anim.transform.position = pos; //the new shape's position is the same of the player and the spawnpoint
        gameObject.GetComponent<MeshRenderer>().enabled = false; //unactive the player and make it invisible
        humanColliderSize = gameObject.GetComponent<BoxCollider2D>().size;
        humanColliderOffset = gameObject.GetComponent<BoxCollider2D>().offset;
        gameObject.transform.parent = anim.transform; //the player became the child of the new shape
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(anim.GetComponent<BoxCollider2D>().size.x / gameObject.transform.localScale.x), Mathf.Abs(anim.GetComponent<BoxCollider2D>().size.y / gameObject.transform.localScale.y));
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
        oldGravity = gravity;
        gravity = 0;
        isHuman = false;
    }

    private void BackToHuman()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().size = humanColliderSize;
        gameObject.GetComponent<BoxCollider2D>().offset = humanColliderOffset;
        gravity = oldGravity;
        isHuman = true;
        gameObject.transform.parent = null;
        Vector3 pos = gameObject.transform.position;
        Vector3 rot = gameObject.transform.rotation.eulerAngles;
        rot.z = 0;
        pos.z = -2;
        gameObject.transform.position = pos;
        gameObject.transform.rotation = Quaternion.Euler(rot);
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

    public bool IsHuman()
    {
        return isHuman;
    }

    public void forceBackToHuman()
    {
        BackToHuman();
    }

    public void setTransformationPoint(Vector3 point)
    {
        tranformationPoint = point;
    }

	void PlaySound(int clipIndex){
		audio.clip = audioClip [clipIndex];
		audio.Play ();
	}
}
