using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSpace : IComparable
{
    public Vector2Int position;
    public float distance;
    public Vector2Int[] path;

    public DistanceSpace(Vector2Int p, float d, Vector2Int[] previous)
    {
        position = p;
        distance = d;
        path = new Vector2Int[previous.Length + 1];
        for(int i = 0; i < previous.Length; ++i)
        {
            path[i] = previous[i];
        }
        path[previous.Length] = position;
    }

    public int CompareTo(object obj)
    {
        DistanceSpace other;
        if((other = (DistanceSpace)obj) == null)
        {
            return (0);
        }
        if(distance > other.distance)
        {
            return (1);
        }
        else if(distance < other.distance)
        {
            return (-1);
        }
        else
        {
            return (0);
        }
    }
}
