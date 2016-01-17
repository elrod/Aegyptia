using UnityEngine;
using System.Collections;

public class Cat : Animal
{


    Controller2D controller;

    public bool isActive = true;

    public float jumpHeight = 4;           // How many unity units we want our player to jump
    public float timeToJumpApex = .4f;     // How much time our player will take to reach the top of the jump curve.
    public float moveSpeed = 6;            // Max movement speed

    public string idleAnimation = "idle";
    public string walkAnimation = "walk";
    public string jumpStart = "jump1";
    public string jumpClimb = "jump2";
    public string jumpTurn = "jump3";
    public string jumpDescend = "jump4";
    public string jumpLand = "jump5";

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    float gravity;
    float jumpVelocity;

    Vector3 velocity;
    float velocityXSmoothing;

    SkeletonAnimation spineAnim;
    string currentAnimation;
    bool jumping = false;
    bool climbing = false;
    bool descending = false;
    float jumpStartTime = 0f;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        spineAnim = GetComponent<SkeletonAnimation>();

    }

    // Update is called once per frame
    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        if (isActive)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            UpdateAnimation(input);
            if (Input.GetButtonDown("Jump") && controller.collisions.below)
            {
                velocity.y = jumpVelocity;
            }

            float targetVelocityX = input.x * moveSpeed;
            // We use smoothDamp to gradually reach our top velocity
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateAnimation(Vector2 input)
    {
        if(input.x == 0)
        {
            if (spineAnim.state.GetCurrent(0) == null || spineAnim.state.GetCurrent(0).Animation.name == walkAnimation) SetAnimation(idleAnimation, true);
        }
        else
        {
            if (input.x > 0)
            {
                spineAnim.skeleton.FlipX = true;
            }
            else if (input.x < 0)
            {
                spineAnim.skeleton.FlipX = false;
            }
            SetAnimation(walkAnimation, true);
        }
        if (Input.GetButtonDown("Jump") && controller.collisions.below)
        {
            //Debug.Log("salto");
            SetAnimation(jumpStart, false);
            jumping = true;
            climbing = true;
            descending = false;
            jumpStartTime = Time.time;
        }
        else if (jumping && controller.collisions.below)
        {
            //Debug.Log("atterro");
            SetAnimation(jumpLand, false);
            jumping = false;
            climbing = false;
            descending = false;
        }
        else if (jumping && climbing)
        {
            if ((Time.time - jumpStartTime) >= timeToJumpApex)
            {
                climbing = false;
                descending = true;
                SetAnimation(jumpDescend, true);
            }
            else { 
                SetAnimation(jumpClimb, true);
            }
        }
        else if (jumping && descending)
        {
            SetAnimation(jumpDescend, true);
        }
    }

    void SetAnimation(string anim, bool loop)
    {
        if (currentAnimation != anim)
        {
            Debug.Log("NUOVA ANIMAZIONE:" + anim);
            spineAnim.state.SetAnimation(0, anim, loop);
            currentAnimation = anim;
        }
    }

    public override void TurnOn()
    {
        isActive = true;
    }

    public override void TurnOff()
    {
        isActive = false;
        velocity.x = 0;
        velocity.y = 0;
    }
}
