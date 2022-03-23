using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapBorderInfo
{
    public BorderData border;
    public int source;
    public int destination;

    public MapBorderInfo()
    {
        source = 0;
        destination = 0;
    }
}
