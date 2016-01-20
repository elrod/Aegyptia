using UnityEngine;
using System.Collections;

public class RandomAnimations : MonoBehaviour {

    public string[] animations;
    SkeletonAnimation spineAnim;

	// Use this for initialization
	void Start () {
        spineAnim = GetComponent<SkeletonAnimation>();
        spineAnim.state.SetAnimation(0, animations[Random.Range(0, animations.Length)], true);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
