﻿using UnityEngine;
using System.Collections;

public class RotatingPass : Tool {

    public float velocity = 1;
    bool isFlipped;

    GameObject objLeft;
    GameObject objRight;

    public string requestedObjectLeft;
    public string requestedObjectRight;

	// Use this for initialization
	void Start () {

        isFlipped=true;
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!isFlipped)
        {
            transform.Rotate(new Vector3(0, velocity, 0));
            if (transform.rotation.eulerAngles.y == 180)
            {
                isFlipped = true;
                objLeft.GetComponent<Collider2D>().enabled = true;
                objRight.GetComponent<Collider2D>().enabled = true;
            }
        }
	}

    public override void Use()
    {
        Debug.Log("TODO: open a door and self destroy");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (col.transform.position.x < gameObject.transform.position.x)
            {
                if (col.gameObject.GetComponent<Inventory>().Has(requestedObjectLeft))
                {
                    objLeft = col.gameObject.GetComponent<Inventory>().Give(requestedObjectLeft);
                    objLeft.transform.position = gameObject.transform.position + new Vector3(-1, 0, 0);
                    objLeft.transform.parent = gameObject.transform;
                    objLeft.GetComponent<SpriteRenderer>().enabled = true;
                    if (objectPlaced())
                    {
                        Flip();
                    }
                    else
                    {

                    }
                }
                else if(objLeft == null)
                {
                    FindObjectOfType<LevelEventsManager>().NotifyEvent("Osiris", "OSIRIS_CANNOT_PLACE_OBJECT");
                }
            }
            else
            {
                if (col.gameObject.GetComponent<Inventory>().Has(requestedObjectRight))
                {
                    objRight = col.gameObject.GetComponent<Inventory>().Give(requestedObjectRight);
                    objRight.transform.position = gameObject.transform.position + new Vector3(1, 0, 0);
                    objRight.transform.parent = gameObject.transform;
                    objRight.GetComponent<SpriteRenderer>().enabled = true;
                    if (objectPlaced())
                    {
                        Flip();
                    }
                    
                }
                else if(objRight == null)
                {
                    FindObjectOfType<LevelEventsManager>().NotifyEvent("Isis", "ISIS_CANNOT_PLACE_OBJECT");
                }
            }
        }
    }
    void Flip()
    {
        isFlipped = false;
    }

    bool objectPlaced()
    {
        return (objLeft != null && objRight != null);
    }

}
