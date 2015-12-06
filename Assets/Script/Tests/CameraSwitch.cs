using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {

    public float moveSpeed = 50f;
    public float maxZoomDiff = 5f;

    bool moving = false;

    Vector3 p1Pos = new Vector3(0f, 0f, -10f);
    Vector3 p2Pos = new Vector3(19.3f, -14f, -10f);

    Vector3 sPos;
    Vector3 tPos;

    int activePlayer = 0;

    Camera cam;
    float startTime;
    float zoomStartTime;
    float sZoom;
    float midZoom;

    bool zoomIn = false;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        sZoom = cam.orthographicSize;
        midZoom = cam.orthographicSize + maxZoomDiff;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("SwitchPlayer"))
        {
            if (!moving)
            {
                SwitchPlayers();
            }
        }
        if (moving && !PositionReached())
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fracJournery = distCovered / Vector3.Distance(sPos, tPos);
            if(cam.orthographicSize == midZoom && !zoomIn)
            {
                zoomStartTime = Time.time;
                zoomIn = true;
            }
            transform.position = Vector3.Lerp(sPos, tPos, fracJournery);
            float zoomDistCovered = (Time.time - zoomStartTime) * (moveSpeed / 2);
            if (zoomIn)
            {
                float fracZoom = zoomDistCovered / Mathf.Abs(sZoom - midZoom);
                cam.orthographicSize = Mathf.Lerp(midZoom, sZoom, fracZoom);
            }
            else
            {
                float fracZoom = zoomDistCovered / Mathf.Abs(midZoom - sZoom);
                cam.orthographicSize = Mathf.Lerp(sZoom, midZoom, fracZoom);
            }
        }
	}

    void SwitchPlayers()
    {
        sPos = activePlayer == 0 ? p1Pos : p2Pos;
        tPos = activePlayer == 0 ? p2Pos : p1Pos;
        activePlayer = activePlayer == 0 ? 1 : 0;
        moving = true;
        startTime = Time.time;
        zoomStartTime = Time.time;
    }

    bool PositionReached()
    {
        if (transform.position.Equals(tPos))
        {
            moving = false;
            zoomIn = false;
            return true;
        }
        return false;
    }
}
