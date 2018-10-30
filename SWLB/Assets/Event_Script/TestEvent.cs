using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEvent : MonoBehaviour {
    public Button m_testBtn;
    ISubscriber m_testGetEvent;
    private void Awake()
    {
        m_testGetEvent = EventsManager.Subscribe(EventNames.Gui_test1);
        m_testGetEvent.Handler = testEvent;
    }
    // Use this for initialization
    void Start () {
        m_testBtn.onClick.AddListener(() =>
        {
            EventsManager.Notify(EventNames.Gui_test1, "test1");
        });
	}
	

    void testEvent(object[] args)
    {
        foreach(var item in args)
        {
            Debug.Log("testEvent = " + item);
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
