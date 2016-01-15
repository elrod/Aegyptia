using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public string mainMenu;
	public string credits;
	public string[] levels;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
