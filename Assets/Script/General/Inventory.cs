using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public struct InventoryObject
	{
		public Tool tool;
		public Sprite itemPic;
		public string name;
		public bool autoConsume;
	}

	public Image GUIItemPic;
	public Text GUIItemText;


	string selectedObject;

	List<InventoryObject> inventory;	
	// Use this for initialization
	void Start () {
		inventory = new List<InventoryObject>();
	}
	
	// Update is called once per frame
	void Update () {
		// TODO: Complete by showing up the inventory and letting user select an object
		if(Input.GetKeyDown(KeyCode.F)){
			Use (selectedObject);
		}
	}

	public void PickUp(GameObject theObject, Sprite GUIPic){
		InventoryObject inventObj;
		inventObj.tool = theObject.GetComponent<Tool>();
		inventObj.itemPic = GUIPic;
		inventObj.name = theObject.name;
		inventObj.autoConsume = true;
		inventory.Add(inventObj);
		selectedObject = inventObj.name;
		GUIItemPic.sprite = inventObj.itemPic;
		GUIItemPic.enabled = true;
		GUIItemText.text = inventObj.name;
        // TODO: fix this... its all hardcoded for the demo!
        //if(gameObject.name == "Osiris" && inventObj.name == "Osiris Key")
        //{
        //    FindObjectOfType<LevelEventsManager>().NotifyEvent("osiris", "OSIRIS_KEY_FOUND");
        //}
        //if(gameObject.name == "Isis" && inventObj.name == "Isis Board")
        //{
        //    FindObjectOfType<LevelEventsManager>().NotifyEvent("isis", "ISIS_TABLET_FOUND");
        //}
        //if(gameObject.name == "Osiris" && inventObj.name == "Isis Board")
        //{
        //    FindObjectOfType<LevelEventsManager>().NotifyEvent("osiris", "OSIRIS_TABLET_OBTAINED");
        //}
        //if (gameObject.name == "Isis" && inventObj.name == "Osiris Key")
        //{
        //    FindObjectOfType<LevelEventsManager>().NotifyEvent("osiris", "ISIS_KEY_OBTAINED");
        //}
        //Debug.Log (inventObj.name + " in inventory");
	}

	public bool Use(string name){
		foreach(InventoryObject obj in inventory){
			if(obj.name.Equals(name)){
				obj.tool.Use();
				inventory.Remove(obj);
                updateInventoryGui();
                return true;
			}
		}
        return false;
	}

    public bool Has(string name)
    {
        foreach (InventoryObject obj in inventory)
        {
            if (obj.name.Equals(name))
            {
                return true;
            }
        }
        return false;
    }

    public GameObject Give(string objToGive)
    {
        
        foreach (InventoryObject obj in inventory)
        {
            if (obj.name.Equals(objToGive))
            {
                GameObject toGive = obj.tool.gameObject;
                inventory.Remove(obj);
                updateInventoryGui();
                Debug.Log(inventory.Count);
                return toGive;
            }
        }
        return new GameObject(); // shouldn't reach this
        
    }

    private void updateInventoryGui()
    {
        if (inventory.Count == 0)
        {
            GUIItemPic.sprite = null;
            GUIItemPic.enabled = false;
            GUIItemText.text = "Inventory Empty";
        }
        else
        {
            selectedObject = inventory[inventory.Count - 1].name;
            GUIItemPic.sprite = inventory[inventory.Count - 1].itemPic;
            GUIItemPic.enabled = true;
            GUIItemText.text = inventory[inventory.Count - 1].name;
        }
    }
}
