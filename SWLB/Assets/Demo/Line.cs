using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DrawLine(List<Vector3> points)
    {
        LineRenderer lineR = this.GetComponent<LineRenderer>();
        lineR.SetVertexCount(points.Count);
        for (int i = 0; i < points.Count; i++)
        {
            lineR.SetPosition(i, points[i]);
            //points.Add(new Vector2(points[i].x, points[i].y));
        }
    }
}
