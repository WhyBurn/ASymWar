using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public enum PlayerType { player = 0, ai = 1 };
    public enum GameType { hotseat = 0, online = 1 };
    public enum GamePhase { production = 0, springPlanning = 1, spring = 2, summerPlanning = 3, summer = 4, autumnPlanning = 5, autumn = 6, attrition = 7, victory = 8};
    public enum MoveType { land = 0, sea = 1, air = 2};

    public static float spaceSize = 1.28f;
    public static Vector2Int defaultRes = new Vector2Int(800, 600);

    public static ScenarioData selectedScenario;
    public static int localId;
    public static GameType gameType;
    public static Space selectedSpace;
    public static Unit selectedUnit;
    public static Space rightClicked;
    public static Card selectedCard;
    public static UnitData selectedUnitData;

    /*public static Vector2Int GetSpace(Vector2Int space, int direction)
    {
        if(direction == 0)
        {
            space.y += 1;
        }
        else if(direction == 1)
        {
            if(space.x % 2 == 1)
            {
                space.y += 1;
            }
            space.x += 1;
        }
        else if(direction == 2)
        {
            if(space.x % 2 == 0)
            {
                space.y -= 1;
            }
            space.x += 1;
        }
        else if(direction == 3)
        {
            space.y -= 1;
        }
        else if(direction == 4)
        {
            if(space.x % 2 == 0)
            {
                --space.y;
            }
            --space.x;
        }
        else if(direction == 5)
        {
            if(space.x % 2 == 1)
            {
                space.y++;
            }
            --space.x;
        }
        return (space);
    }

    public static int GetDirection(Vector2Int start, Vector2Int end)
    {
        if(start.x == end.x)
        {
            if (start.y > end.y)
            {
                return (3);
            }
            else if (start.y < end.y)
            {
                return (0);
            }
        }
        else if(start.x > end.x)
        {
            if(start.y > end.y)
            {
                return (4);
            }
            else if(start.y < end.y)
            {
                return (5);
            }
            else
            {
                if(start.x % 2 == 1)
                {
                    return (4);
                }
                else
                {
                    return (5);
                }
            }
        }
        else
        {
            if (start.y > end.y)
            {
                return (2);
            }
            else if (start.y < end.y)
            {
                return (1);
            }
            else
            {
                if (start.x % 2 == 1)
                {
                    return (2);
                }
                else
                {
                    return (1);
                }
            }
        }
        return (-1);
    }*/

    public static Vector2 GetPosition(Vector2Int spacePos)
    {
        float x = spaceSize / 2;
        float y = spaceSize * Mathf.Sin(60 * Mathf.Deg2Rad) / (1 + (2 * Mathf.Cos(60 * Mathf.Deg2Rad)));
        float dy = y * 2;
        float dx = spaceSize / 2 + (spaceSize * Mathf.Cos(60 * Mathf.Deg2Rad) / (1 + (2 * Mathf.Cos(60 * Mathf.Deg2Rad))));
        if(spacePos.x % 2 == 1)
        {
            y *= 2f;
        }
        return (new Vector2(x + (dx * spacePos.x), y + (dy * spacePos.y)));
    }
    public static Vector2 GetPosition(int x, int y)
    {
        return (GetPosition(new Vector2Int(x, y)));
    }
}
