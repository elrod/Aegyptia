using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LevelEventsManager : MonoBehaviour {

    public Notificator osirisNotificator;
    public Notificator isisNotificator;

    Dictionary<string, string> events = new Dictionary<string, string>(){
        {"OSIRIS_BEGIN","I should find a way out of here!"},
        {"ISIS_BEGIN", "I must find Osiris!"},
        {"OSIRIS_KEY_FOUND", "This may be useful to Isis..." },
        {"ISIS_TABLET_FOUND", "This may be useful to Osiris..." },
        {"OSIRIS_TABLET_OBTAINED", "This will be useful to get out of here!" },
        {"ISIS_KEY_OBTAINED", "I can use this to get out!"},
        {"OSIRIS_TABLET_MISSING", "Mmmm, looks like I need some kind of tablet here..." },
        {"ISIS_KEY_MISSING", "I don't have the key"},
        {"OSIRIS_LEVEL_FINISHED", "I did it! Let's move on!" },
        {"ISIS_LEVEL_FINISHED", "I did it! Let's move on!" }
    };

    Dictionary<string, bool> triggeredEvents = new Dictionary<string, bool>(){
        {"OSIRIS_BEGIN", false},
        {"ISIS_BEGIN", false},
        {"OSIRIS_KEY_FOUND", false },
        {"ISIS_TABLET_FOUND", false },
        {"OSIRIS_TABLET_OBTAINED", false },
        {"ISIS_KEY_OBTAINED", false },
        {"OSIRIS_TABLET_MISSING", false },
        {"ISIS_KEY_MISSING", false},
        {"OSIRIS_LEVEL_FINISHED", false },
        {"ISIS_LEVEL_FINISHED", false }
    };

    // Use this for initialization
    void Start () {
        
	}

    public void NotifyEvent(string dest, string event_key)
    {
        dest = dest.ToLower();
        Notificator selectedNot = null;
        if (dest.Equals("osiris"))
        {
            selectedNot = osirisNotificator;
        }
        if (dest.Equals("isis"))
        {
            selectedNot = isisNotificator;
        }
        if (selectedNot != null)
        {
            if (!triggeredEvents[event_key])
            {
                selectedNot.gameObject.SetActive(true);
                selectedNot.Notify(events[event_key]);
                triggeredEvents[event_key] = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
