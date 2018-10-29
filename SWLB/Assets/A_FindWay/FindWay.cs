using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataItem
{
    public int x;
    public int y;
    public double weight;
    public List<DataItem> childs;
    public int wId;
    public DataItem parent;
}


public class FindWay {
    private List<FloorData> _mapData = new List<FloorData>();
    private int _mapX = 0;
    private int _mapY = 0;

    private List<int[]> WAY_OFFSET = new List<int[]>();
    public FloorData[,] map_info = new FloorData[20, 20];
    public int m_wayId = 0;


    public void CreateFindWay(List<FloorData> FloorDataInfo, int mapX, int mapY)
    {
        _mapData = FloorDataInfo;
        _mapX = mapX;
        _mapY = mapY;
        WAY_OFFSET = SetWay();
        SetMapInfo();
    }

    public List<DataItem> FindNextWay(int beginX, int beginY, int endX, int endY)
    {
        if(beginX == endX && beginY == endY)
        {
            Debug.LogError("findnextway beginX == endX && beginY == endY");
            return null;
        }

        double cost = GetCost(beginX, beginY, endX, endY);
        int wayId = SetWayId();
        List<DataItem> childs = new List<DataItem>();
        DataItem rootNode = setWayDataInfo(beginX, beginY, cost, childs, wayId, null);
        List<DataItem> openNodes = new List<DataItem>();
        openNodes.Add(rootNode);

        for(int i = 0; i < 5000; ++i)
        {
            DataItem currentNode = this.getBastNode(openNodes);
            if (currentNode == null)
                return null;

            int findWayCount = this.GetFindWayCount(endX, endY, currentNode.x, currentNode.y, currentNode, openNodes);
            if(findWayCount > 0)
            {
                List<DataItem> bestWay = this.getBestWay(openNodes, findWayCount, endX, endY);
                if(bestWay != null)
                {
                    return bestWay;
                }
            }
            else
            {
                this.RemoveNode(currentNode, openNodes);
            }
        }
        return null;
    }

    private DataItem setWayDataInfo(int x, int y, double we, List<DataItem> childs, int id, DataItem parent)
    {
        DataItem wayData = new DataItem();
        wayData.x = x;
        wayData.y = y;
        wayData.weight = we;
        wayData.childs = childs;
        wayData.wId = id;
        wayData.parent = parent;
        return wayData;
    }

    public List<int[]> SetWay()
    {
        List<int[]> way = new List<int[]>();
        int[] way1 = new int[] { 0, 1 };
        int[] way2 = new int[] { 0, -1 };
        int[] way3 = new int[] { -1, 0 };
        int[] way4 = new int[] { 1, 0 };
        way.Add(way1);
        way.Add(way2);
        way.Add(way3);
        way.Add(way4);
        return way;
        Debug.Log("aaaaaaaa= " + way.Count);
    }

    private void SetMapInfo()
    {
        foreach(var a in _mapData)
        {
            map_info[a._posX, a._posY] = a;
        }
    }

    public bool IsEnableWalk(int x, int y)
    {

        return true;
    }

    public double GetCost(int sx, int sy, int tx, int ty)
    {
        int x = Math.Abs(sx - tx);
        int y = Math.Abs(sy - ty);
        return Math.Sqrt(x * x + y * y);
    }

    public int SetWayId()
    {
        return m_wayId++;
    }

    public DataItem getBastNode(List<DataItem> allNodes)
    {
        if (allNodes.Count == 0)
            return null;

        double minWeight = allNodes[0].weight;
        int minIndex = 0;
        for(int i = 1; i < allNodes.Count; i++)
        {
            DataItem node = allNodes[i];
            if(node.weight < minWeight)
            {
                minWeight = node.weight;
                minIndex = i;
            }
        }

        return allNodes[minIndex];
    }

    public int GetFindWayCount(int tx, int ty, int cx, int cy, DataItem curNode, List<DataItem> allNodes)
    {
        int count = 0; 
        for(int i = 0; i < WAY_OFFSET.Count; ++i)
        {
            int nx = cx + WAY_OFFSET[i][0];
            int ny = cy + WAY_OFFSET[i][1];
            if (nx < _mapX && ny < _mapY && nx >= 0 && ny >= 0)
            {
                double cost = this.GetCost( nx, ny, tx, ty);
                List<DataItem> childs = new List<DataItem>();
                int id = this.SetWayId();
                DataItem node = this.setWayDataInfo(nx, ny, cost, childs, id, curNode);
                curNode.childs.Add(node);
                allNodes.Add(node);
                ++count;
            }
        }
        return count ;
    }

    private List<DataItem> getBestWay(List<DataItem> allNodes, int wayCount, int tx, int ty)
    {
        for(int i = allNodes.Count - wayCount; i < allNodes.Count; ++i)
        {
            if(allNodes[i].x == tx && allNodes[i].y == ty)
            {
                List<DataItem> way = new List<DataItem>();
                DataItem cur = allNodes[i];
                while(cur != null)
                {
                    if(cur.parent != null)
                    {
                        way.Add(cur);
                    }
                    cur = cur.parent;
                }
                way.Reverse();
                return way;
            }
        }
        return null;
    }

    private void RemoveNode(DataItem removeNode, List<DataItem> allNodes)
    {
        if(removeNode.parent != null)
        {
            List<DataItem> childs = removeNode.parent.childs;
            foreach(var item in childs)
            {
                if(item.wId == removeNode.wId)
                {
                    childs.Remove(item);
                    break;
                }
            }

            foreach(var item in allNodes)
            {
                if(item.wId == removeNode.wId)
                {
                    allNodes.Remove(item);
                    break;
                }
            }
        }
    }
}
