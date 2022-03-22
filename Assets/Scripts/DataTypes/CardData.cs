using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData", order = 7)]
public class CardData : ScriptableObject
{
    public int initiative;
    public int numUnits;
}
