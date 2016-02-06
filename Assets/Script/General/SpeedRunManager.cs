using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedRunManager : MonoBehaviour {

	public int timeBonus = 5;
	public Text totalTime;
	public Text bonus;
	public Text finalTime;

	public void UpdateTimes(){
		int totalBonus = FindObjectOfType<Collector> ().getCollected () * timeBonus;
		float time = FindObjectOfType<Timer> ().GetTimeFloat() - totalBonus;
		int minutes = (int) time / 60; //Divide the guiTime by sixty to get the minutes.
		int seconds = (int) (time - minutes*60);//Use the euclidean division for the seconds.
		int fraction = (int) ((time - minutes*60 - seconds)*1000);
		totalTime.text = FindObjectOfType<Timer> ().GetTimeText();
		bonus.text = totalBonus.ToString() + " seconds";
		finalTime.text = string.Format ("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
	}
}
