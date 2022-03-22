using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CountryData", menuName = "ScriptableObjects/CountryData", order = 1)]
public class CountryData : ScriptableObject
{
    public string countryName;
    public UnitData[] availableUnits;
    public CountryStartUnit[] startPositions;
    public CardData[] deck;
    public int startingMoney;
    public int handSize;
    public int id;
    public Sprite controlSprite;
    public int[] possibleRecruits;
    public int initiativeTiebreaker;
    public int numStartingCards;
    public int startingIndustry;
    public int landStackingLimit;
    public int seaStackingLimit;
    public int industryUpgradeCost;
}
