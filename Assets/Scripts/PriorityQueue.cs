using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IComparable
{
    List<T> values;
    int lowIndex;

    public PriorityQueue()
    {
        values = new List<T>();
        lowIndex = -1;
    }

    public int Count
    {
        get { return (values.Count); }
    }

    public void Add(T value)
    {
        values.Add(value);
        if(lowIndex < 0 || value.CompareTo(values[lowIndex]) < 0)
        {
            lowIndex = values.Count - 1;
        }
    }

    public T Peek()
    {
        if(lowIndex < 0)
        {
            return (default(T));
        }
        return (values[lowIndex]);
    }

    public T Remove()
    {
        if(lowIndex < 0)
        {
            return (default(T));
        }
        T value = values[lowIndex];
        values.RemoveAt(lowIndex);
        lowIndex = -1;
        for(int i = 0; i < values.Count; ++i)
        {
            if(lowIndex < 0 || values[i].CompareTo(values[lowIndex]) < 0)
            {
                lowIndex = i;
            }
        }
        return (value);
    }
}
