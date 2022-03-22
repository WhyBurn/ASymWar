using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSpace : IComparable
{
    public int position;
    public float distance;
    public int[] path;

    public DistanceSpace(int p, float d, int[] previous)
    {
        position = p;
        distance = d;
        path = new int[previous.Length + 1];
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
