using UnityEngine;
using System.Collections;

public class Key : Tool {
	public Tool tool;
	public bool respawn;

	Vector3 initialPosition;
	
	Renderer theRenderer;
	Collider2D theCollider;

	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
		theRenderer = GetComponent<Renderer>();
		theCollider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Use(){
        if (tool != null)
			tool.Use ();
		if (respawn) {
			gameObject.GetComponent<Pickable>().StopPick();
			transform.position = initialPosition;
			theCollider.enabled = true;
			theRenderer.enabled = true;
		}
	}
}
