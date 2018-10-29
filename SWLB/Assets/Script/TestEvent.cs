using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEvent : MonoBehaviour {
    public Button m_testBtn;
	// Use this for initialization
	void Start () {
        m_testBtn.onClick.AddListener(() =>
        {
            EventsManager.Notify(EventNames.Gui_test1);
        });


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
