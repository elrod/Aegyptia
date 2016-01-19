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

    public string idleAnimation = "idle";
    public string walkAnimation = "cammina";
    public string runAnimation = "corre";

    string currentAnimation = "";
    SkeletonAnimation spineAnim;


    float accelerationTimeGrounded = .1f;
	
	float gravity = -50;
	Vector3 velocity;
	float velocityXSmoothing;


	// Use this for initialization
	void Start () {
		controller = GetComponent<EnemyController2D>();
        spineAnim = GetComponent<SkeletonAnimation>();
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

    void SetAnimation(string anim, bool loop)
    {
        if (currentAnimation != anim)
        {
            //Debug.Log("NUOVA ANIMAZIONE:" + anim);
            spineAnim.state.SetAnimation(0, anim, loop);
            currentAnimation = anim;
        }
    }

    void PatrolMovement(){
        SetAnimation(walkAnimation, true);
        if (transform.position.x <= leftPatrolPoint.position.x) {
			goRight = true;
		} else if (transform.position.x >= rightPatrolPoint.position.x) {
			goRight = false;
		}
		if (controller.collisions.left || controller.collisions.right) {
			goRight = !goRight;
		}
		float direction;
		if (goRight) {
			direction = 1;
            spineAnim.skeleton.flipX = false;
        } else {
			direction = -1;
            spineAnim.skeleton.flipX = true;
        }
		float targetVelocityX = direction * patrolSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
		
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}

	void FollowEnemy(){
        SetAnimation(runAnimation, true);
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
                spineAnim.skeleton.flipX = false;
            } else {
				direction = -1;
                spineAnim.skeleton.flipX = true;
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
