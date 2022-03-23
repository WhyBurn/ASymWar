using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/MapData", order = 2)]
public class MapData : ScriptableObject
{
    public MapSpaceInfo[] mapSpaces;
    public MapBorderInfo[] borders;

    public void FixArrays()
    {
        if (mapSpaces == null)
        {
            mapSpaces = new MapSpaceInfo[0];
            borders = new MapBorderInfo[0];
        }
        /*MapSpaceInfo[] s = new MapSpaceInfo[width * height];
        MapBorderInfo[] b = new MapBorderInfo[width * height * Data.spaceBorders];
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                int index = y * width + x;
                s[index] = new MapSpaceInfo(x, y);
                for (int j = 0; j < Data.spaceBorders; ++j)
                {
                    b[(index) * Data.spaceBorders + j] = new MapBorderInfo(x, y, j);
                }
            }
        }
        mapSpaces = s;*/
    }
}
