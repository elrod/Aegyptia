using UnityEngine;
using System.Collections;

public class SpearBehavior : MonoBehaviour {

    public float velocityUp;
    public float velocityDown;
    public float deltaMovement;

    Vector3 initPosition;
    bool isGoingDown = false;

	// Use this for initialization
	void Start () {
        initPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (isGoingDown) goDown();
        else goUp();
	}

    void goDown()
    {
        Vector3 pos = transform.position;
        pos += new Vector3(Mathf.Cos((transform.eulerAngles.z - 90) * Mathf.Deg2Rad), Mathf.Sin((transform.eulerAngles.z - 90) * Mathf.Deg2Rad), 0) * velocityDown * Time.deltaTime;
        transform.position = pos;
        if (pos.y < initPosition.y - deltaMovement)
        {
            isGoingDown = false;
        }
    }

    void goUp()
    {
        Vector3 pos = transform.position;
        pos -= new Vector3(Mathf.Cos((transform.eulerAngles.z - 90) * Mathf.Deg2Rad), Mathf.Sin((transform.eulerAngles.z - 90) * Mathf.Deg2Rad), 0) * velocityUp * Time.deltaTime;
        transform.position = pos;
        if (pos.y > initPosition.y + deltaMovement)
        {
            isGoingDown = true;
        }
    }
}
