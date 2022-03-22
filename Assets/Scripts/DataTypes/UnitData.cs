using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 5)]
public class UnitData : ScriptableObject
{
    public string unitName;
    public int minHealth;
    public int maxHealth;
    public int minAttack;
    public int maxAttack;
    public int minDefense;
    public int maxDefense;
    public int movement;
    public int cost;
    public Sprite sprite;
    public float percentageOfDamageUsed;
    public float percentageOfInstantDamage;
    public Data.MoveType moveType;

    public int RandomHealth()
    {
        return (Random.Range(minHealth, maxHealth + 1));
    }

    public int RandomAttack()
    {
        return (Random.Range(minAttack, maxAttack + 1));
    }

    public int RandomDefense()
    {
        return (Random.Range(minDefense, maxDefense + 1));
    }

    public string GetDescription()
    {
        string description = unitName;
        description += "\nCost: " + cost;
        description += "\nHealth: " + minHealth + "-" + maxHealth;
        description += "\nAttack: " + minAttack + "-" + maxAttack;
        description += "\nDefense: " + minDefense + "-" + maxDefense;
        description += "\nMovement: " + movement;
        return (description);
    }
}
