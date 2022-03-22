using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BorderData", menuName ="ScriptableObjects/BorderData", order = 0)]
public class BorderData : ScriptableObject
{
    public float moveCost;
    public int attackModifier;
    public int maxUnits;
    public Sprite sprite;

    public BorderData()
    {
        moveCost = 0;
        attackModifier = 0;
        maxUnits = -1;
        sprite = null;
    }
}
