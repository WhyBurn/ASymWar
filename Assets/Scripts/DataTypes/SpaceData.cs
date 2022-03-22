using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpaceData", menuName = "ScriptableObjects/SpaceData", order = 4)]
public class SpaceData : ScriptableObject
{
    public string terrainName;
    public float entrance;
    public float exit;
    public bool recruit;
    public int terrainDefense;
    public int points;
    public int income;
    public bool land;
    public int stackModifier;
    public Sprite sprite;

    public SpaceData()
    {
        entrance = 1;
        exit = 0;
        recruit = false;
        terrainDefense = 0;
        points = 0;
        income = 0;
        land = true;
        stackModifier = 0;
    }
}
