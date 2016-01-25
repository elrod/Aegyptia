using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerLabel;
	float time = 0;
	float minutes;
	float seconds;
	float fraction;
	bool start = false;
	
	void Update() {

		minutes = time / 60; //Divide the guiTime by sixty to get the minutes.
		seconds = time % 60;//Use the euclidean division for the seconds.
		fraction = (time * 100) % 100;

		if (start) {
			time += Time.deltaTime;
		}
		
		//update the label value
		timerLabel.text = string.Format ("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
	}

	public void StartTimer(){
		start = true;
	}

	public void StopTimer(){
		start = false;
	}

	public float[] GetTimeFloat(){
		float[] timeArray = new float[] {minutes, seconds, fraction};
		return timeArray;
	}

	public string GetTimeText(){
		return timerLabel.text;
	}
}
