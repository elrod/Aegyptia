using UnityEngine;
using System.Collections;

public class Key : Tool {
	public Tool tool;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Use(){
        if(tool != null)
		    tool.Use ();
	}
}
