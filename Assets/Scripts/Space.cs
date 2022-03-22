using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space
{
    private SpaceData data;
    private int xPos;
    private int yPos;
    private int controllerId;
    private int recruitType;

    public DistanceSpace availableMove;

    public Space(SpaceData d, int x, int y, int c, int r)
    {
        data = d;
        xPos = x;
        yPos = y;
        controllerId = c;
        recruitType = r;
        availableMove = null;
    }

    public float Entrance
    {
        get { return (data.entrance); }
    }
    public float Exit
    {
        get { return (data.exit); }
    }
    public int Income
    {
        get { return (data.income); }
    }
    public int Defense
    {
        get { return (data.terrainDefense); }
    }
    public Vector2Int Position
    {
        get { return (new Vector2Int(xPos, yPos)); }
    }
    public int Controller
    {
        get { return (controllerId); }
        set { controllerId = value; }
    }
    public Sprite Sprite
    {
        get { return (data.sprite); }
    }
    public bool Recruit
    {
        get { return (data.recruit); }
    }
    public int RecruitType
    {
        get { return (recruitType); }
    }
    public int StackingModifier
    {
        get { return (data.stackModifier); }
    }
    public bool Land
    {
        get { return (data.land); }
    }

    public string GetDescription()
    {
        string description = data.terrainName;
        description += "\nEntrance Move Cost: " + data.entrance;
        description += "\nExit Move Cost: " + data.exit;
        description += "\nTerrain Defense Bonus: " + data.terrainDefense;
        if(data.recruit)
        {
            description += "\n" + Game.GetGame().GetRecruitTitle(recruitType) + " can recruit.";
        }
        if(data.points > 0)
        {
            description += "\nSpace worth " + data.points + " points.";
        }
        if(data.income > 0)
        {
            description += "\nSpace provides " + data.income + " income.";
        }
        return (description);
    }
}
