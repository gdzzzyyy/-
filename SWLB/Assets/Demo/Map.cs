using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData
{
    public bool isEnable ;
    public int _posX , _posY, _floorId;
}



public class Map : MonoBehaviour {

    public int m_wight, m_hight, m_gridW, m_gridH, m_startPosX, m_startPosY, m_endPosX, m_endPosY;
    public List<FloorData> m_floorDataMap = new List<FloorData>();
    public GameObject m_floorItem;
    public GameObject m_floorItem2;
    public GameObject m_floorItem3;
    public GameObject m_floorItem4;
    public GameObject m_masterObj;
    public Master m_master;
    public Dictionary<int, FloorItem> m_allFloorItem = new Dictionary<int, FloorItem>();
    public FindWay findway = new FindWay();

    // Use this for initialization
    void Start () {
        if (m_hight == null || m_wight == null || m_gridH == null || m_gridW == null)
        {
            return;
        }

        InitMapData();
        SetFloorItem();
        CrateMaster();
        
    }

    void InitMapData()
    {
        for(int i = 0; i < m_wight; i++)
        {
            for(int j = 0; j < m_hight; j++)
            {
                FloorData _floorData = new FloorData();
                _floorData.isEnable = true;
                _floorData._posX = i;
                _floorData._posY = j;
                _floorData._floorId = (i * 1000 + j);
                m_floorDataMap.Add(_floorData);
            }
        }
    }

    void SetFloorItem()
    {
        if(m_floorDataMap == null || m_floorDataMap.Count < 1)
        {
            return;
        }

        for(int i = 0; i < m_floorDataMap.Count; i++)
        {
            if (m_floorDataMap[i].isEnable == null)
                continue;

            int id = m_floorDataMap[i]._floorId;
            int z = id % 4;
            GameObject obj;
            switch (z)
            {
                case 0 :
                    obj = (GameObject)Instantiate(m_floorItem);
                    break;
                case 1:
                    obj = (GameObject)Instantiate(m_floorItem2);
                    break;
                case 2:
                    obj = (GameObject)Instantiate(m_floorItem3);
                    break;
                case 3:
                    obj = (GameObject)Instantiate(m_floorItem4);
                    break;
                default:
                    obj = null;
                    break;

            }

            if (obj == null)
                continue;
           // GameObject obj = (GameObject)Instantiate(m_floorItem);
            FloorItem _floorCom = obj.GetComponent<FloorItem>();
            obj.transform.position = new Vector3(m_floorDataMap[i]._posX, m_floorDataMap[i]._posY, 0);
            obj.name = "floor( " + m_floorDataMap[i]._posX + " , " + m_floorDataMap[i]._posY + " )";
            m_allFloorItem.Add(m_floorDataMap[i]._floorId, _floorCom);
        }

    }

    void SetMapData()
    {

    }

    public List<DataItem> finilyway = new List<DataItem>();
    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.B))
        {
            findway.CreateFindWay(m_floorDataMap, m_wight, m_hight);
            finilyway = findway.FindNextWay(0, 0, 9, 9);
            if(finilyway.Count <= 0)
            {
                Debug.LogError("error findnextway way is null");
                return;
            }

            if (finilyway.Count > 0)
            {
                List<Vector3> _points = new List<Vector3>();
                GameObject line = (GameObject)Instantiate(m_lineObj);
                line.transform.parent = lineManager.transform;
                
                for (int i = 0; i < finilyway.Count; i++)
                {
                    //if (i + i < finilyway.Count)
                    //{
                        Vector3 world1 = new Vector3(finilyway[i].x, finilyway[i].y, 0);
                     //   Vector3 world2 = new Vector3(finilyway[i + 1].x, finilyway[i + 1].y, 0);
                        _points.Add(world1);
                        
                   // }

                    Debug.Log("x = " + finilyway[i].x + " y = " + finilyway[i].y);
                }
                Line lin = line.GetComponent<Line>();
                lin.DrawLine(_points);
                m_master.DoRun(_points);
            }

        }

   

    }

    //public Material mat;
    public GameObject m_lineObj;
    public GameObject lineManager;



    private void CrateMaster()
    {
        GameObject obj = (GameObject)Instantiate(m_masterObj);
        if(obj != null)
        {
            m_master = obj.GetComponent<Master>();
            m_master.gameObject.transform.position = new Vector3(m_startPosX, m_startPosY);
        }
        
    }

   
}
