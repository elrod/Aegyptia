using UnityEngine;
using System.Collections;

public class Cat : Animal
{


    Controller2D controller;

    public bool isActive = true;

    public float jumpHeight = 4;           // How many unity units we want our player to jump
    public float timeToJumpApex = .4f;     // How much time our player will take to reach the top of the jump curve.
    public float moveSpeed = 6;            // Max movement speed

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    float gravity;
    float jumpVelocity;

    Vector3 velocity;
    float velocityXSmoothing;

    void Start()
    {

        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

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
