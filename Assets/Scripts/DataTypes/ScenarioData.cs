using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenarioData", menuName = "ScriptableObjects/ScenarioData", order = 3)]
public class ScenarioData : ScriptableObject
{
    public string scenarioName;
    public CountryData[] countries;
    public MapData map;
    public string[] recruitNames;
}
