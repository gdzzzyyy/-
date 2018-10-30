using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class AttributeTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        FieldInfo[] array = GetComponent<AttributeTest>().GetType().GetFields();
        foreach(var item in array)
        {
            Debug.Log("字段名：" + item);
        }
	}

    public string str = "abc";
	
	// Update is called once per frame
	void Update () {
		
	}
}
