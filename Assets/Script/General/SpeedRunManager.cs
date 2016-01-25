using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedRunManager : MonoBehaviour {

	public float timeBonus = 10f;
	public Text totalTime;
	public Text bonus;
	public Text finalTime;

	public void UpdateTimes(){
		float totalBonus = FindObjectOfType<Collector> ().getCollected () * timeBonus;
		float[] timeArray = FindObjectOfType<Timer> ().GetTimeFloat();
		totalTime.text = FindObjectOfType<Timer> ().GetTimeText();
		bonus.text = totalBonus.ToString() + " seconds";
		finalTime.text = string.Format ("{0:00} : {1:00} : {2:000}", timeArray[0], timeArray[1]-totalBonus, timeArray[2]);
	}
}
