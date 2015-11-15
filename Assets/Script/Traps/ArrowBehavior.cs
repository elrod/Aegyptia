﻿using UnityEngine;
using System.Collections;

public class ArrowBehavior : MonoBehaviour {

    public float velocity = 1;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos += (new Vector3(1, 0, 0)) * velocity * Time.deltaTime;
        transform.position = pos;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }

}
