using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public GameObject platform;
	public float moveSpeed;
	Transform currentPoint;
	public Transform[] points;
	public int moveToPoint = 1;
	
	void Start () {
		currentPoint = points [moveToPoint];
	}

	void FixedUpdate(){
		platform.transform.position = Vector3.MoveTowards (platform.transform.position, 
		                                                   currentPoint.position, 
		                                                   Time.deltaTime * moveSpeed);
		if (platform.transform.position == currentPoint.position) {
			moveToPoint++;
			if(moveToPoint == points.Length){
				moveToPoint=0;
			}
		}
		currentPoint = points [moveToPoint];
	}
}
