using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapBorderInfo
{
    public BorderData border;
    public int xPos;
    public int yPos;
    public int direction;

    public MapBorderInfo(int x, int y, int d)
    {
        border = Resources.Load<BorderData>("DefaultBorder");
        xPos = x;
        yPos = y;
        direction = d;
    }
}
