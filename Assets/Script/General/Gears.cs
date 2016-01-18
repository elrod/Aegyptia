using UnityEngine;
using System.Collections;

public class Gears : Tool {

    public GameObject gear;

    bool gearInserted = false;
    GameObject insertedGear;
    public float velocity;
    public Tool[] toolsToUse;

    bool tools = false;
	// Use this for initialization
	void Start () {
        if (toolsToUse.Length > 0)
        {
            tools = true;
        }
	
	}
	
	// Update is called once per frame
	void Update () {
        if (gearInserted)
        {
            insertedGear.transform.Rotate(new Vector3(0,0, velocity));
        }
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (col.gameObject.GetComponent<Inventory>().Has(gear.name))
            {
                insertedGear = col.gameObject.GetComponent<Inventory>().Give(gear.name);
                insertedGear.transform.position = gameObject.transform.position +  new Vector3(0.27f, 0.4f, -0.5f);
                insertedGear.GetComponent<SpriteRenderer>().enabled = true;               
                insertedGear.GetComponent<Collider2D>().enabled = false;
                gearInserted = true;
                Use();
            }
        }
    }

    public override void Use()
    {
        if (tools)
        {
            foreach (Tool tool in toolsToUse)
            {
                tool.Use();
            }
        }
    }
}
