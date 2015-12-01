using UnityEngine;
using System.Collections;

public class Scarab : MonoBehaviour {

    Controller2D controller;

    bool isActive = true;
    float smoothMove = 0.1f;
    float moveSpeed = 6;

    Vector3 velocity;

    float velocityXSmoothing;
    float velocityYSmoothing;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isActive)
        {
            // Getting input
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            float targetVelocityX = input.x * moveSpeed;
            float targetVelocityY = input.y * moveSpeed;
            // We use smoothDamp to gradually reach our top velocity
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, smoothMove);
            velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, smoothMove);
        }
        controller.Move(velocity * Time.deltaTime);
    }

    public void TurnOn()
    {
        isActive = true;
    }

    public void TurnOff()
    {
        isActive = false;
        velocity.x = 0;
        velocity.y = 0;
    }

}
