using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space
{
    private SpaceData data;
    private int spaceNum;
    private int xPos;
    private int yPos;
    private int controllerId;
    private int recruitType;
    private string spaceName;
    private Border[] adjacentSpaces;

    public DistanceSpace availableMove;

    public Space(SpaceData d, int sN, int x, int y, int c, int r, string n)
    {
        data = d;
        spaceNum = sN;
        xPos = x;
        yPos = y;
        controllerId = c;
        recruitType = r;
        availableMove = null;
        spaceName = n;
        adjacentSpaces = new Border[0];
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
    public int NumAdjacent
    {
        get { return (adjacentSpaces.Length); }
    }
    public int SpaceID
    {
        get { return (spaceNum); }
    }

    public Border GetAdjacent(int pos)
    {
        return (adjacentSpaces[pos]);
    }

    public string GetDescription()
    {
        string description = spaceName;
        description += "\n" + data.terrainName;
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

    public void AddAdjacent(BorderData border, Space destination)
    {
        Border[] newBorders = new Border[adjacentSpaces.Length + 1];
        for (int i = 0; i < adjacentSpaces.Length; ++i)
        {
            newBorders[i] = adjacentSpaces[i];
        }
        newBorders[adjacentSpaces.Length] = new Border(border, this, destination);
        adjacentSpaces = newBorders;
    }
}
