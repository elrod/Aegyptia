using UnityEngine;
using System.Collections;

public class StupidMove : MonoBehaviour {

	public float speed = 2f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * Time.deltaTime);
	}
}
