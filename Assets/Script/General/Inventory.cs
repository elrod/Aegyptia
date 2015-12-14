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
		Debug.Log (inventObj.name + " in inventory");
	}

	public void Use(string name){
		foreach(InventoryObject obj in inventory){
			if(obj.name.Equals(name)){
				obj.tool.Use();
				inventory.Remove(obj);
				if(inventory.Count == 0){
					GUIItemPic.sprite = null;
					GUIItemPic.enabled = false;
					GUIItemText.text = "Inventory Empty";
				}
				else{
					selectedObject = inventory[inventory.Count - 1].name;
					GUIItemPic.sprite = inventory[inventory.Count - 1].itemPic;
					GUIItemPic.enabled = true;
					GUIItemText.text = inventory[inventory.Count - 1].name;
				}
				break;
			}
		}
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
                return toGive;
            }
        }
        return new GameObject();
        
    }
}
