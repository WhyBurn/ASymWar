using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapSpaceInfo
{
    public SpaceData space;
    public int xPos;
    public int yPos;
    public int country;
    public int recruitType;
    public string name;

    public MapSpaceInfo(int x, int y)
    {
        space = Resources.Load<SpaceData>("DefaultSpace");
        xPos = x;
        yPos = y;
        country = -1;
        recruitType = 0;
        name = "";
    }
}
