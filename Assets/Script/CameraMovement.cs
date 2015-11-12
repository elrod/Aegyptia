using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	Vector2 velocity;
	
	public float smoothTimeY = 0.2f;
	public float smoothTimeX = 0.2f;
	
	GameObject activePlayer;
	
	public bool bounds;
	public Vector3 minCameraPos;
	public Vector3 maxCameraPos;
	
	// Use this for initialization
	void Start () {
		// It uses the Tag to know which player is active
		activePlayer = GameObject.FindGameObjectWithTag ("Player");
	}
	
	void Update (){
		activePlayer = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		// With SmoothDamp we reach the position of the prayer gradually 
		float posX = Mathf.SmoothDamp (transform.position.x, activePlayer.transform.position.x, ref velocity.x, smoothTimeX);
		float posY = Mathf.SmoothDamp (transform.position.y, activePlayer.transform.position.y, ref velocity.y, smoothTimeY);
		
		transform.position = new Vector3 (posX, posY, transform.position.z);
		
		// If the bounds option is active, we stop the camera when it reaches the min/max position
		if (bounds) {
			transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
			                                 Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
			                                 Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z)
			                                 );
		}
		
	}
}
