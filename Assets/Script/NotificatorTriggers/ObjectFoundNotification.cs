using UnityEngine;
using System.Collections;

public class ObjectFoundNotification : MonoBehaviour {

    public enum PlayerName { Osiris, Isis };

    public PlayerName target;
    public string messageId = "";

    LevelEventsManager levelEventsManager;

    void Start()
    {
        levelEventsManager = FindObjectOfType<LevelEventsManager>();
    }

	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.gameObject.name.Equals(target.ToString()))
        {
            levelEventsManager.NotifyEvent(target.ToString(), messageId);
        }
    }
}
