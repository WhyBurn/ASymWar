using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit
{
    private UnitData data;
    private int maxHealth;
    private int health;
    private int attack;
    private int defense;
    private int maxMovement;
    private float movement;
    private int space;
    public Stack<int> path;
    private int countryId;
    private int plannedDamage;

    public Unit(UnitData d, int s, int country)
    {
        data = d;
        maxHealth = d.RandomHealth();
        health = maxHealth;
        attack = d.RandomAttack();
        defense = d.RandomDefense();
        maxMovement = d.movement;
        movement = maxMovement;
        space = s;
        path = new Stack<int>();
        countryId = country;
    }

    public string Name
    {
        get { return (data.unitName); }
    }

    public int Health
    {
        get { return (health); }
        set { health = value; }
    }
    public int MaxHealth
    {
        get { return (maxHealth); }
    }
    public int Attack
    {
        get { return (attack); }
    }
    public int Defense
    {
        get { return (defense); }
    }
    public float Movement
    {
        get { return (movement); }
        set { movement = value; }
    }
    public int MaxMovement
    {
        get { return (maxMovement); }
    }
    public int Space
    {
        get { return (space); }
        set { space = value; }
    }
    public int Id
    {
        get { return (countryId); }
    }
    public Sprite Sprite
    {
        get { return (data.sprite); }
    }
    public float UsedDamagePercentage
    {
        get { return (data.percentageOfDamageUsed); }
    }
    public float InstantDamagePercentage
    {
        get { return (data.percentageOfInstantDamage); }
    }
    public int PlannedDamage
    {
        get { return (plannedDamage); }
        set { plannedDamage = value; }
    }
    public Data.MoveType MoveType
    {
        get { return (data.moveType); }
    }

    public string GetHealthString()
    {
        return (health + "/" + maxHealth);
    }
    public string GetMovementString()
    {
        return (movement + "/" + maxMovement);
    }

    public string GetDescription()
    {
        string description = data.unitName;
        description += "\nHealth: " + GetHealthString();
        description += "\nMovement: " + GetMovementString();
        description += "\nAttack: " + attack;
        description += "\nDefense: " + defense;
        return (description);
    }

    public void ResetMovement()
    {
        path = new Stack<int>();
        movement = maxMovement;
    }
}
