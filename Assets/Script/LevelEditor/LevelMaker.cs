using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LevelMaker : MonoBehaviour {

    [Range(1f,2048f)]
    public float width = 32.0f;
    [Range(1f, 2048f)]
    public float height = 32.0f;

    public Color gridColor = Color.green;
	public bool gridVisible = true;

    public GameObject[] tiles;

    List<GameObject> levelTiles = new List<GameObject>();

    int selectedTile = 0;

    void OnDrawGizmos()
    {
		if(gameObject.GetComponentsInChildren<Transform>().Length - 1 > levelTiles.Count){
			levelTiles.Clear();
			foreach(Transform child in gameObject.GetComponentsInChildren<Transform>()){
				if(!child.gameObject.Equals(gameObject)){
					levelTiles.Add(child.gameObject);
				}
			}
		}
        DrawGrid();
    }

    void DrawGrid()
    {
        Vector3 pos = Camera.current.transform.position;
		if(gridVisible){
			gridColor.a = 1f;
		}
		else{
			gridColor.a = 0f;
		}
		Gizmos.color = gridColor;

        for (float y = pos.y - 800.0f; y < pos.y + 800.0f; y += this.height)
        {
            Gizmos.DrawLine(new Vector3(-1000000.0f, Mathf.Floor(y / this.height) * this.height, 0.0f),
                            new Vector3(1000000.0f, Mathf.Floor(y / this.height) * this.height, 0.0f));
        }
        for (float x = pos.x - 1200.0f; x < pos.x + 1200.0f; x += this.width)
        {
            Gizmos.DrawLine(new Vector3(Mathf.Floor(x / this.width) * this.width, -1000000.0f, 0.0f),
                            new Vector3(Mathf.Floor(x / this.width) * this.width, 1000000.0f, 0.0f));
        }
    }

    public void AddTile(Vector3 position)
    {
        foreach(GameObject tile in levelTiles)
        {
            if(tile.transform.position == position)
            {
                // If we get here, tile is already occupied, ignoring adding request
                //Debug.Log("Tile already occupied, ignoring");
                return;
            }
        }
        GameObject newTile = Instantiate<GameObject>(tiles[selectedTile]) as GameObject;
        newTile.transform.position = position;
        newTile.transform.SetParent(transform);
        levelTiles.Add(newTile);
    }

    public void RemoveTileAt(Vector3 position)
    {
        foreach (GameObject tile in levelTiles)
        {
            if (tile.transform.position == position)
            {
                // Found it! Removing tile.
                //Debug.Log("Removing Tile");
                GameObject.DestroyImmediate(tile);
                levelTiles.Remove(tile);
                return;
            }
        }
    }

    public void ResetLevel()
    {
        // Remove all children
        foreach(Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (!child.gameObject.Equals(gameObject))
            {
                DestroyImmediate(child.gameObject);

            }
        }
        // Clear levelTiles list
        levelTiles.Clear();
    }

    public void SelectTile(int index)
    {
        selectedTile = index;
        // In case of array overflow default to 0
        if(index >= tiles.Length)
            selectedTile = 0;
    }

}
