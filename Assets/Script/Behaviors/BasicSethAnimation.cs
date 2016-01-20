using UnityEngine;
using System.Collections;

public class BasicSethAnimation : MonoBehaviour {

    public string[] animations = { "idle", "idle2", "idle3", "idle4", "idle5" };
    string currentAnimation = "";
    SkeletonAnimation spineAnim;

    float minPlayTime = 2f;
    float maxPlayTime = 3f;
    bool canSwitchAnimation = true;

    // Use this for initialization
    void Start () {
        spineAnim = GetComponent<SkeletonAnimation>();
	}
	
	// Update is called once per frame
	void Update () {
        if (canSwitchAnimation)
        {
            // 50% probability to flip on x
            if(Random.Range(0,2) == 0)
            {
                spineAnim.skeleton.flipX = !spineAnim.skeleton.flipX;
            }
            SetAnimation(animations[Random.Range(0, animations.Length)], true);
            canSwitchAnimation = false;
            Invoke("EnableSwitchAnimation", Random.Range(minPlayTime, maxPlayTime));
        }
    }

    void SetAnimation(string anim, bool loop)
    {
        if (currentAnimation != anim)
        {
            //Debug.Log("NUOVA ANIMAZIONE:" + anim);
            spineAnim.state.SetAnimation(0, anim, loop);
            currentAnimation = anim;
        }
    }

    void EnableSwitchAnimation()
    {
        canSwitchAnimation = true;
    }
}
