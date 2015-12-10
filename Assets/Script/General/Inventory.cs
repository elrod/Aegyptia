using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public struct InventoryObject
	{
		public Tool tool;
		public string name;
		public bool autoConsume;
	}

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

	public void PickUp(GameObject theObject){
		InventoryObject inventObj;
		inventObj.tool = theObject.GetComponent<Tool>();
		inventObj.name = theObject.name;
		inventObj.autoConsume = true;
		inventory.Add(inventObj);
		selectedObject = inventObj.name;
		Debug.Log (inventObj.name + " in inventory");
	}

	public void Use(string name){
		foreach(InventoryObject obj in inventory){
			if(obj.name.Equals(name)){
				obj.tool.Use();
				inventory.Remove(obj);
				break;
			}
		}
	}
}
