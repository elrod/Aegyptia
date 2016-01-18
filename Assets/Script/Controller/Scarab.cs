using UnityEngine;
using System.Collections;

public class Scarab : Animal {

	public string idleAnimation = "idle";
	public string walkAnimation = "walk";

	string currentAnimation = "";
	SkeletonAnimation spineAnim;

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
		spineAnim = GetComponent<SkeletonAnimation>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isActive)
        {
            // Getting input
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector3 rot = transform.rotation.eulerAngles;
			UpdateAnimation(input);
            if(input.x > 0 && Mathf.Abs(input.x) >= Mathf.Abs(input.y))
            {
                rot.z = -90;
            }
            else if(input.x < 0 && Mathf.Abs(input.x) >= Mathf.Abs(input.y))
            {
                rot.z = 90;
            }
            else if(input.y > 0 && Mathf.Abs(input.y) >= Mathf.Abs(input.x))
            {
                rot.z = 0;
            }
            else if(input.y < 0 && Mathf.Abs(input.y) >= Mathf.Abs(input.x))
            {
                rot.z = 180;
            }
            transform.rotation = Quaternion.Euler(rot);

            float targetVelocityX = input.x * moveSpeed;
            float targetVelocityY = input.y * moveSpeed;
            // We use smoothDamp to gradually reach our top velocity
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, smoothMove);
            velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, smoothMove);
			if(!input.Equals(Vector2.zero) && !GetComponent<AudioSource>().isPlaying){
				GetComponent<AudioSource>().Play();
			} else {
				GetComponent<AudioSource>().Pause ();
			}
        }

        controller.Move(velocity * Time.deltaTime);
    }

	void UpdateAnimation(Vector2 input){
		if(input.Equals(Vector2.zero)){
			SetAnimation(idleAnimation, true);
		}
		else{
			SetAnimation(walkAnimation, true);
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
