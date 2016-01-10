using UnityEngine;
using System.Collections;

public class Crocodile : Animal
{

    Controller2D controller;

    bool isActive = true;
    bool isInTheWater = false;
    float smoothMove = 0.1f;
    float moveSpeed = 5;
    float swimSpeed = 2;
    float gravity;

    public float swimHeight = 1;           // the crocodile swims upwards using the jump button (super mario style)
    public float timeToJumpApex = .4f;     // How much time our player will take to reach the top of the swim curve.
    public float gravityOutOfWater;
    float acceletarionTimeWaterborne = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    Vector3 velocity;
    float jumpVelocity; //actually is the velocity of the "jump" into the water. Out of water can't jump 

    float velocityXSmoothing;
    float velocityYSmoothing;

    bool goingLeft = true;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * swimHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (isActive)
        {
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            float targetVelocityX;

            if (isInTheWater)
            {

                gravity = -(2 * swimHeight) / Mathf.Pow(timeToJumpApex, 2);
                if (Input.GetButtonDown("Jump"))
                {
                    velocity.y = jumpVelocity;
                }

                targetVelocityX = input * swimSpeed;
            }

            else
            {
                gravity = gravityOutOfWater;
                targetVelocityX = input * moveSpeed;
            }


            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (isInTheWater) ? acceletarionTimeWaterborne : accelerationTimeGrounded);

        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if(input < 0 && !goingLeft)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            goingLeft = true;
        }
        else if(input > 0 && goingLeft)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            goingLeft = false;
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


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            //Debug.Log("pluf!");
            isInTheWater = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            //Debug.Log("plif!");
            isInTheWater = false;
        }
    }
}
