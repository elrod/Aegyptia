using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyController2D))]


public class NewEnemyAI : MonoBehaviour {

	EnemyController2D controller;

	public Transform leftPatrolPoint;
	public Transform rightPatrolPoint;
	public float patrolSpeed;
	public float followSpeed;
	public float lookAroundTime = 1.5f;
    public GameObject playerToFollow;
	bool goRight;
	bool patrol = true;
	bool lookAround = false;
	bool looking = false;
	float startLookingAround;

    public float movementRange = 2f;
    Vector3 tempPosition;

	float accelerationTimeGrounded = .01f;
	
	float gravity = -50;
	Vector3 velocity;
	float velocityXSmoothing;

    public string idleAnimation = "idle";
    public string walkAnimation = "cammina";
    public string runAnimation = "corre";

    string currentAnimation = "";
    SkeletonAnimation spineAnim;


    // Use this for initialization
    void Start () {
		controller = GetComponent<EnemyController2D>();
        spineAnim = GetComponent<SkeletonAnimation>();
        if (transform.position.x >= rightPatrolPoint.position.x) {
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
        if (controller.collisions.left || controller.collisions.right)
        {
            goRight = !goRight; 
        }


		if (controller.collisions.enemyLeft || controller.collisions.enemyRight) {
			patrol = false;

		} else 
            {
                if (!patrol && !lookAround)
                {
                    startLookingAround = Time.time;
                    lookAround = true;
                }
                
		}



		if (patrol) {
			PatrolMovement ();
		} else if (lookAround) {
			followGuessedEnemy ();
		} else {
		    FollowEnemy();
		}


	}

	void PatrolMovement(){
        //Debug.Log("Patrol");
        SetAnimation(walkAnimation, true);
        if (transform.position.x <= leftPatrolPoint.position.x) {
			goRight = true;
		} else if (transform.position.x >= rightPatrolPoint.position.x) {
			goRight = false;
		}
        //if (controller.collisions.left || controller.collisions.right) {
        //    goRight = !goRight;
        //}
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
            spineAnim.skeleton.flipX = false;
        } else if (controller.collisions.enemyLeft) {
			goRight = false;
			direction = -1f;
            spineAnim.skeleton.flipX = true;
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

    void followGuessedEnemy()
	{
        if (Time.time - startLookingAround < lookAroundTime)
        {
            if (transform.position.x <= leftPatrolPoint.position.x && looking){
				looking = false;
                PatrolMovement();
                return;
			} else if (transform.position.x - leftPatrolPoint.position.x < movementRange && !looking){
				PatrolMovement();
				return;
			} else if (transform.position.x >= rightPatrolPoint.position.x && looking){
				looking = false;
                PatrolMovement();
                return;
			} else if (transform.position.x - rightPatrolPoint.position.x > -movementRange && !looking){
				PatrolMovement();
				return;
			} else {
				looking = true;
			}
            //Debug.Log("followGuessed " + (Time.time - startLookingAround));
            // Check the distance between the enemy and its current target
            float distance = playerToFollow.transform.position.x - transform.position.x;

            // If the enemy is arrived then pick a random target if it is followin the player or return following him
            //if (Mathf.Abs(distance) < 0.1f)
            //{
            //    tempPosition = playerToFollow.transform.position + (new Vector3(Random.Range(-movementRange, movementRange), 0f, 0f));
            //}

            // If there is a collision that forbid the enemy to move pick a new random target
            //if (controller.collisions.left || controller.collisions.right)
            //{
                //    following = false;
                //    tempPosition = playerToFollow.transform.position + (new Vector3(Random.Range(-movementRange, movementRange), 0f, 0f));
               // patrol = true;
            //}


            float directionX = Mathf.Sign(distance);
            float targetVelocityX = directionX * patrolSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            lookAround = false;
            patrol = true;
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
}

