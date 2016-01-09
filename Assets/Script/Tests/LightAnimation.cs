using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightAnimation : MonoBehaviour {

    public float minLightIntensity = 5f;
    public float maxLightIntensity = 6f;
    public float lightVariationSpeed = 1f;

    Light theLight;
    bool goUp;

	// Use this for initialization
	void Start () {
        theLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        if (goUp) {
            theLight.intensity += lightVariationSpeed * Time.deltaTime;
            if(theLight.intensity >= maxLightIntensity)
            {
                theLight.intensity = maxLightIntensity;
                goUp = false;
            }
        }
        else
        {
            theLight.intensity -= lightVariationSpeed * Time.deltaTime;
            if (theLight.intensity <= minLightIntensity)
            {
                theLight.intensity = minLightIntensity;
                goUp = true;
            }
        }
	}
}
