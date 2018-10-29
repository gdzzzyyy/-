using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {

    private bool m_isRund = false;
    private List<Vector3> m_points = new List<Vector3>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoRun(List<Vector3> points)
    {
        m_points = points;
        for(int i = 0; i < points.Count; i++)
        {
            StartCoroutine(doRunIe(points[i]));
        }
        
    }

    IEnumerator doRunIe(Vector3 point)
    {
        Vector3 raw_rotation = transform.eulerAngles;
        transform.LookAt(point);
        Vector3 lookat_rotation = transform.eulerAngles;
        transform.eulerAngles = raw_rotation;
        while (transform.position != point)
        {
            
            transform.position = Vector3.MoveTowards (transform.position, point, 1 * Time.deltaTime);
            transform.eulerAngles = Vector3.RotateTowards(transform.eulerAngles, lookat_rotation, 1, 1);
            yield return 0;
        }
    }

}
