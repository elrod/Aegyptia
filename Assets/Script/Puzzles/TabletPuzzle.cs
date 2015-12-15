using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TabletPuzzle : MonoBehaviour {

    public GameObject[] sequence;
    public List<GameObject> tilesToPlace;
    public GameObject currentTile;
    public GameObject cursor;

    int cursorIndex = 0;
    int tileToPlaceIndex = 0;
    bool moved = false;

	// Use this for initialization
    void OnStart()
    {
        ResetTablet();
    }
	void OnEnable ()
    {
        ResetTablet();
    }
	
	// Update is called once per frame
	void Update () {
        float stickInput = Input.GetAxis("Horizontal");
        if (stickInput == 0 && moved)
        {
            moved = false;
        }
	    if(stickInput > 0 && !moved)
        {
            cursorIndex++;
            if(cursorIndex >= tilesToPlace.Count)
            {
                cursorIndex = 0;
            }
            cursor.GetComponent<RectTransform>().position = tilesToPlace[cursorIndex].GetComponent<RectTransform>().position;
            moved = true;
        }
        if (stickInput < 0 && !moved)
        {
            cursorIndex--;
            if (cursorIndex < 0)
            {
                cursorIndex = tilesToPlace.Count - 1;
            }
            cursor.GetComponent<RectTransform>().position = tilesToPlace[cursorIndex].GetComponent<RectTransform>().position;
            moved = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            foreach(GameObject tile in tilesToPlace)
            {
                if(cursor.GetComponent<RectTransform>().position == tile.GetComponent<RectTransform>().position)
                {
                    if (tile.GetComponent<Image>().sprite.Equals(currentTile.GetComponent<Image>().sprite))
                    {
                        // CORRECT MOVE
                        tile.SetActive(true);
                        tilesToPlace.Remove(tile);
                        //Debug.Log("CORRECT");
                        if(tilesToPlace.Count == 0)
                        {
                            // PUZZLE CORRECTLY SOLVED
                            //Debug.Log("SOLVED!");
                            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().enabled = true;
                            gameObject.SetActive(false);
                            FindObjectOfType<LevelEventsManager>().NotifyEvent("osiris", "OSIRIS_LEVEL_FINISHED");
                        }
                        else
                        {
                            // PUZZLE NOT SOLVED YET
                            cursorIndex = 0;
                            RandomTileToPlace();
                            cursor.GetComponent<RectTransform>().position = tilesToPlace[cursorIndex].GetComponent<RectTransform>().position;
                        }
                    }
                    else
                    {
                        // TODO: handle incorrect move
                        //Debug.Log("INCORRECT");
                    }
                    break;
                }
            }
        }
    }

    void ResetTablet()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().enabled = false;
        tilesToPlace = new List<GameObject>();
        foreach (GameObject tile in sequence)
        {
            if (!tile.activeSelf)
            {
                tilesToPlace.Add(tile);
            }
        }
        if (tilesToPlace.Count != 0)
        {
            cursor.GetComponent<RectTransform>().position = tilesToPlace[cursorIndex].GetComponent<RectTransform>().position;
            RandomTileToPlace();
        }
    }

    void RandomTileToPlace()
    {
        tileToPlaceIndex = Random.Range(0, tilesToPlace.Count);
        currentTile.GetComponent<Image>().sprite = tilesToPlace[tileToPlaceIndex].GetComponent<Image>().sprite;
    }
}
