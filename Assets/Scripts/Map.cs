using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private Space[] spaces;

    public Map()
    {
        spaces = new Space[0];
    }
    public Map(MapData data)
    {
        spaces = new Space[data.mapSpaces.Length];
        for (int y = 0; y < spaces.Length; ++y)
        {
            spaces[y] = new Space(data.mapSpaces[y].space, y, data.mapSpaces[y].xPos, data.mapSpaces[y].yPos
                , data.mapSpaces[y].country, data.mapSpaces[y].recruitType, data.mapSpaces[y].name);
        }
        for(int i = 0; i < data.borders.Length; ++i)
        {
            MapBorderInfo border = data.borders[i];
            spaces[border.source].AddAdjacent(border.border, spaces[border.destination]);
        }
    }

    public int NumSpaces
    {
        get { return (spaces.Length); }
    }

    public void ResetSpaceMovement()
    {
        for (int i = 0; i < spaces.Length; ++i)
        {
            spaces[i].availableMove = null;
            for(int b = 0; b < spaces[i].NumAdjacent; ++b)
            {
                spaces[i].GetAdjacent(b).numMoves = 0;
            }
        }
    }

    public void SetMovement(int startingPosition, float movement, int team, Data.MoveType moveType)
    {
        ResetSpaceMovement();
        List<int> visited = new List<int>();
        PriorityQueue<DistanceSpace> distanceSpaces = new PriorityQueue<DistanceSpace>();
        distanceSpaces.Add(new DistanceSpace(startingPosition, 0, new int[0]));
        while(distanceSpaces.Count > 0)
        {
            DistanceSpace space = distanceSpaces.Remove();
            if(space.distance <= movement)
            {
                int position = space.position;
                bool done = false;
                for(int i = 0; i < visited.Count; ++i)
                {
                    if(position == visited[i])
                    {
                        done = true;
                        break;
                    }
                }
                if(done)
                {
                    continue;
                }
                Space consideredSpace = GetSpace(position);
                consideredSpace.availableMove = space;
                visited.Add(position);
                List<Unit> unitsInSpace = Game.GetGame().GetUnitsInSpace(space.position);
                bool noEnemies = true;
                for(int u = 0; u < unitsInSpace.Count; ++u)
                {
                    if(unitsInSpace[u].Id != team)
                    {
                        noEnemies = false;
                        break;
                    }
                }
                if (noEnemies)
                {
                    for (int i = 0; i < consideredSpace.NumAdjacent; ++i)
                    {
                        if (consideredSpace.GetAdjacent(i).data.maxUnits < 0
                            || consideredSpace.GetAdjacent(i).numMoves < consideredSpace.GetAdjacent(i).data.maxUnits)
                        {
                            float moveCost = consideredSpace.Exit;
                            Space targetSpace = consideredSpace.GetAdjacent(i).destination;
                            if (moveType == Data.MoveType.air || targetSpace.Recruit || ((targetSpace.Land && moveType == Data.MoveType.land)
                                || (!targetSpace.Land && (moveType == Data.MoveType.sea || Game.GetGame().HasSeaUnit(targetSpace.SpaceID)))))
                            {
                                moveCost += targetSpace.Entrance;
                                moveCost += consideredSpace.GetAdjacent(i).data.moveCost;
                                if (moveType == Data.MoveType.air)
                                {
                                    moveCost = 1;
                                }
                                distanceSpaces.Add(new DistanceSpace(targetSpace.SpaceID, space.distance + moveCost, space.path));
                            }
                        }
                    }
                }
            }
        }
    }

    public void UpdateBorders(int[] path)
    {
        Space origin = GetSpace(path[0]);
        for (int i = 0; i < path.Length - 1; ++i)
        {
            Space destination = GetSpace(path[i + 1]);
            for (int a = 0; a < origin.NumAdjacent; ++a)
            {
                if (origin.GetAdjacent(a).destination == destination)
                {
                    origin.GetAdjacent(i).numMoves++;
                    break;
                }
            }
            origin = destination;
        }
    }

    public void UndoMove(Space resetPosition, Space undonePosition)
    {
        for(int i = 0; i< resetPosition.NumAdjacent; ++i)
        {
            if(resetPosition.GetAdjacent(i).destination == undonePosition)
            {
                resetPosition.GetAdjacent(i).numMoves--;
                return;
            }
        }
    }

    public void UndoMove(int resetPosition, int undonePosition)
    {
        Space r = GetSpace(resetPosition);
        Space u = GetSpace(undonePosition);
        UndoMove(r, u);
    }

    public Space GetSpace(int id)
    {
        return (spaces[id]);
    }

    public Border GetBorder(int source, int destination)
    {
        Space s = GetSpace(source);
        Space d = GetSpace(destination);
        for(int i = 0; i < s.NumAdjacent; ++i)
        {
            if(s.GetAdjacent(i).destination == d)
            {
                return (s.GetAdjacent(i));
            }
        }
        return (null);
    }

    public int TotalIncome(int country)
    {
        int income = 0;
        for (int y = 0; y < spaces.Length; ++y)
        {
            if (spaces[y].Controller == country)
            {
                income += spaces[y].Income;
            }
        }
        return (income);
    }
}
