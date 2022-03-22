using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Victory", menuName = "ScriptableObjects/VictoryCondition", order = 7)]
public class VictoryCondition : ScriptableObject
{
    public int numTurns;
    public int countryId;
    public int numPoints;
    public string victoryMessageOverride;
}
