﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border
{
    public BorderData data;
    public int numMoves;
    public Space source;
    public Space destination;

    public Border(BorderData d)
    {
        data = d;
        numMoves = 0;
    }
    public Border(BorderData d, Space s, Space sD)
    {
        data = d;
        source = s;
        destination = sD;
    }
}
