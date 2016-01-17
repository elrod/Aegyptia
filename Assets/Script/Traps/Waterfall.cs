using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Trap))]

public class Waterfall : MonoBehaviour {

	// Use this for initialization
    Trap trapInfo;

	void Start () {

        trapInfo = transform.GetComponent<Trap>();
	
	}
	
	// Update is called once per frame
	void Update () {
        if (trapInfo.isActive)
        {
            gameObject.GetComponent<ParticleSystem>().enableEmission = true;
        }
        else
        {
            gameObject.GetComponent<ParticleSystem>().enableEmission = false;
        }
	}
}
