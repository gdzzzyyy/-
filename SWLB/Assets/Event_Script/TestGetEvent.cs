using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGetEvent : MonoBehaviour {

    ISubscriber m_testGetEvent;
    private void Awake()
    {
        m_testGetEvent = EventsManager.Subscribe(EventNames.Gui_test1);
        m_testGetEvent.Handler = Hell;
    }

    void Hell(object[] args)
    {
        Debug.Log("Hello world!!!");
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
