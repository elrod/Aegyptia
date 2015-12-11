using UnityEngine;
using System.Collections;

public class Lever : Tool {

	// Declare your serializable data.
	[System.Serializable]
	public class TileArea
	{
		public Transform bottomLeft;
		public Transform topRight;
		public int tileToSpawnIndex; //Look at th level maker to find it
		public bool isALoop = false;
		public int loopToSpawnIndex; //Look at th level maker to find it
	}

	public TileArea[] destroyArea;
	public TileArea[] spawnArea;

	int currentArea = 0;
	bool currentAreaChanged = true;

	public Tool[] toolsToUse;
	bool active = false;
	LevelMaker level;
	bool used = false;
	float xDestrMin;
	float xDestrMax;
	float yDestrMin;
	float yDestrMax;
	float xSpawnMin;
	float xSpawnMax;
	float ySpawnMin;
	float ySpawnMax;
	float z;
	float yDestr;
	float ySpawn;

	public float interval = 1f;
	float elapsedTime = 0f;
	bool destroying = false;
	bool spawning = false;
	bool start = false;
	
	
	
	// Use this for initialization
	void Start () {
		level = FindObjectOfType<LevelMaker>();
		if (destroyArea.Length > 0) {
			destroying = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

		// Surely not the best way to call it, but I've rewritten this so many times that 
		// at the moement I don't want to change it.
		if (Input.GetKeyDown(KeyCode.Q)){
			if (active) {
				start = true;
			}
		}
		if (start){
			Use ();
		}
	}

	public override void Use(){
		// the lever is designed to be used only one time.
		if (!used){
			// First there is the part in which we destroy the existing tiles (if it has to be done)
			if(destroying && destroyArea.Length != 0){

				// If it is a new area the bounds are set
				if(currentAreaChanged){
					xDestrMin = destroyArea[currentArea].bottomLeft.position.x;
					xDestrMax = destroyArea[currentArea].topRight.position.x;
					yDestrMin = destroyArea[currentArea].bottomLeft.position.y;
					yDestrMax = destroyArea[currentArea].topRight.position.y;
					z = destroyArea[currentArea].topRight.position.z;
					yDestr = yDestrMax;
					currentAreaChanged = false;
				}

				// Remove one row
				for(float xDestr = xDestrMin; xDestr<=xDestrMax; xDestr +=1f){
					level.RemoveTileAt(new Vector3(xDestr, yDestr, z));
				}

				// If it is not the last row wait the interval and then move on to the next row.
				// Otherwise if it wasn't the last area increment currentArea, if it was the last
				// reset the variables and go active the spawning section.
				if(yDestr!= yDestrMin){
					if (elapsedTime<interval){
						elapsedTime += Time.deltaTime;
					} else {
						yDestr-=1f;
						elapsedTime = 0f;
					}
				} else {
					if (currentArea+1 < destroyArea.Length){
						currentArea++;
					} else {
						destroying = false;
						spawning = true;
						currentArea = 0;
					}
					currentAreaChanged = true;
				}
			}

			if(spawning && spawnArea.Length != 0){
				// If it is a new area set the tile, the loop and the bounds
				if(currentAreaChanged){

					if(spawnArea[currentArea].isALoop){
						level.EnableLoop();
						level.SelectLoop(spawnArea[currentArea].loopToSpawnIndex);
					}else{
						level.DisableLoop(); 
						level.SelectTile(spawnArea[currentArea].tileToSpawnIndex);
					}

					xSpawnMin = spawnArea[currentArea].bottomLeft.position.x;
					xSpawnMax = spawnArea[currentArea].topRight.position.x;
					ySpawnMin = spawnArea[currentArea].bottomLeft.position.y;
					ySpawnMax = spawnArea[currentArea].topRight.position.y;
					z = spawnArea[currentArea].topRight.position.z;
					ySpawn = ySpawnMin;
					currentAreaChanged = false;
				}	

				// Spawn a row
				for(float xSpawn = xSpawnMin; xSpawn<=xSpawnMax; xSpawn +=1f){
					level.AddTile(new Vector3(xSpawn, ySpawn, z));
				}

				// Like we did to destroy the tiles check if it is the last row, last area, etc.
				if(ySpawn != ySpawnMax){
					if (elapsedTime<interval){
						elapsedTime += Time.deltaTime;
					} else {
						ySpawn+=1f;
						elapsedTime = 0f;
					}
				} else {
					if (currentArea+1 < spawnArea.Length){
						currentArea++;
					} else {
						destroying = false;
						spawning = false;
						currentArea = 0;
					}
					currentAreaChanged = true;
				}
			}

			// Finally use the tools
			if (!destroying && !spawning){
				foreach(Tool tool in toolsToUse){
					tool.Use();
				}
				used = true;
				start = false;
			}
		}
	}
	
	void OnTriggerEnter2D (Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			active = true;
		}
	}
	
	void OnTriggerExit2D (Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			active = false;
		}
	}
	
}

