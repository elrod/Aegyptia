using UnityEngine;
using System.Collections;

public class FishBehavior : MonoBehaviour {

	public Vector3 dir = Vector3.left;
	public float minSwimmingSpeed = 2f;
	public float maxSwimmingSpeed = 4f;

	SkeletonAnimation spineAnim;
	float swimmingSpeed;
	Vector3 prevDir;

	bool collisionLock = false;
	float collisionLockTime = 0.2f;

	// Use this for initialization
	void Start () {
		spineAnim = GetComponent<SkeletonAnimation>();
		swimmingSpeed = Random.Range(minSwimmingSpeed, maxSwimmingSpeed);
		prevDir = dir;
	}
	
	// Update is called once per frame
	void Update () {
		if(!dir.Equals(prevDir)){
			spineAnim.skeleton.flipX = dir.Equals(Vector3.left) ? false : true;
		}
		transform.Translate(dir * swimmingSpeed * Time.deltaTime);
		prevDir = dir;
	}

	void OnCollisionEnter2D(Collision2D col){
        if (!collisionLock && LayerMask.LayerToName(col.gameObject.layer).Equals("Obstacles")){
			dir.x *= -1;
			collisionLock = true;
			Invoke("UnlockCollision", collisionLockTime);
		}
	}

	void UnlockCollision(){
		collisionLock = false;
	}
}
