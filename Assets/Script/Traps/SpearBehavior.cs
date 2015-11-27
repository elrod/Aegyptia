using UnityEngine;
using System.Collections;

public class SpearBehavior : MonoBehaviour {

    public float velocityUp = 1;
    public float velocityDown = 1;
    public float deltaMovement = 5;

    bool isGoingUp = true, isGoingDown = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isGoingDown) goDown();
        else goUp();
	}

    void goUp()
    {
        Vector3 pos = transform.position;
        pos += new Vector3(Mathf.Pow(Mathf.Cos(transform.eulerAngles.z),2),Mathf.Pow(Mathf.Sin(transform.eulerAngles.z),2),0) * velocityUp * Time.deltaTime;
        transform.position = pos;
    }

    void goDown()
    {

    }
}
