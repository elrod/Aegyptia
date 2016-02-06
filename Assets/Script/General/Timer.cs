using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerLabel;
	float time = 0;
	int minutes;
	int seconds;
	int fraction;
	bool start = false;
	
	void Update() {
		
		if (start) {
			time += Time.deltaTime;
		}
		//Debug.Log (time);
		minutes = (int) time / 60; //Divide the guiTime by sixty to get the minutes.
		seconds = (int) (time - minutes*60);//Use the euclidean division for the seconds.
		fraction = (int) ((time - minutes*60 - seconds)*1000);
		
		//update the label value
		timerLabel.text = string.Format ("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
	}

	public void StartTimer(){
		start = true;
	}

	public void StopTimer(){
		start = false;
	}

	public float GetTimeFloat(){
		return minutes*60f + seconds + fraction/1000f;
	}

	public string GetTimeText(){
		return timerLabel.text;
	}
}
