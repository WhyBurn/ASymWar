using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private Space[][] spaces;
    private Border[][][] borders;

    public Map()
    {
        spaces = new Space[0][];
        borders = new Border[0][][];
    }
    public Map(MapData data)
    {
        spaces = new Space[data.height][];
        borders = new Border[data.height][][];
        for(int y = 0; y < data.height; ++y)
        {
            spaces[y] = new Space[data.width];
            borders[y] = new Border[data.width][];
            for(int x = 0; x < data.width; ++x)
            {
                int index = y * data.width + x;
                spaces[y][x] = new Space(data.mapSpaces[index].space, x, y, data.mapSpaces[index].country, data.mapSpaces[index].recruitType);
                borders[y][x] = new Border[Data.spaceBorders];
                for(int b = 0; b < Data.spaceBorders; ++b)
                {
                    borders[y][x][b] = new Border(data.borders[index * Data.spaceBorders + b].border);
                }
            }
        }
    }

    public int Height
    {
        get { return (spaces.Length); }
    }
    public int Width
    {
        get
        {
            if (spaces.Length <= 0)
            {
                return (0);
            }
            return (spaces[0].Length);
        }
    }

    public void ResetSpaceMovement()
    {
        for(int i = 0; i < spaces.Length; ++i)
        {
            for(int j = 0; j < spaces[i].Length; ++j)
            {
                spaces[i][j].availableMove = null;
            }
        }
    }

    public void ResetBorderMoves()
    {
        for(int i = 0; i < borders.Length; ++i)
        {
            for(int j = 0; j < borders[i].Length; ++j)
            {
                for(int k = 0; k < borders[i][j].Length; ++k)
                {
                    borders[i][j][k].numMoves = 0;
                }
            }
        }
    }

    public void SetMovement(Vector2Int startingPosition, float movement, int team, Data.MoveType moveType)
    {
        ResetSpaceMovement();
        List<Vector2Int> visited = new List<Vector2Int>();
        PriorityQueue<DistanceSpace> distanceSpaces = new PriorityQueue<DistanceSpace>();
        distanceSpaces.Add(new DistanceSpace(startingPosition, 0, new Vector2Int[0]));
        while(distanceSpaces.Count > 0)
        {
            DistanceSpace space = distanceSpaces.Remove();
            if(space.distance <= movement)
            {
                Vector2Int position = space.position;
                bool done = false;
                for(int i = 0; i < visited.Count; ++i)
                {
                    if(position.x == visited[i].x && position.y == visited[i].y)
                    {
                        done = true;
                        break;
                    }
                }
                if(done)
                {
                    continue;
                }
                spaces[position.y][position.x].availableMove = space;
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
                    for (int i = 0; i < Data.spaceBorders; ++i)
                    {
                        if (borders[position.y][position.x][i].data.maxUnits < 0 || borders[position.y][position.x][i].numMoves < borders[position.y][position.x][i].data.maxUnits)
                        {
                            float moveCost = spaces[position.y][position.x].Exit;
                            Vector2Int targetPos = Data.GetSpace(position, i);
                            if (targetPos.x >= 0 && targetPos.y >= 0 && targetPos.x < Width && targetPos.y < Height)
                            {
                                if (moveType == Data.MoveType.air || spaces[targetPos.y][targetPos.x].Recruit || ((spaces[targetPos.y][targetPos.x].Land && moveType == Data.MoveType.land)
                                    || (!spaces[targetPos.y][targetPos.x].Land && (moveType == Data.MoveType.sea || Game.GetGame().HasSeaUnit(targetPos)))))
                                {
                                    moveCost += spaces[targetPos.y][targetPos.x].Entrance;
                                    moveCost += borders[position.y][position.x][i].data.moveCost;
                                    if (moveType == Data.MoveType.air)
                                    {
                                        moveCost = 1;
                                    }
                                    distanceSpaces.Add(new DistanceSpace(targetPos, space.distance + moveCost, space.path));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void UpdateBorders(Vector2Int[] path)
    {
        for(int i = 0; i < path.Length - 1; ++i)
        {
            int dir = Data.GetDirection(path[i], path[i + 1]);
            if (dir >= 0)
            {
                borders[path[i].y][path[i].x][dir].numMoves++;
            }
        }
    }

    public void UndoMove(Vector2Int resetPosition, Vector2Int undonePosition)
    {
        int dir = Data.GetDirection(resetPosition, undonePosition);
        if (dir >= 0)
        {
            borders[resetPosition.y][resetPosition.x][dir].numMoves--;
        }
    }

    public Space GetSpace(int x, int y)
    {
        return (spaces[y][x]);
    }

    public Border GetBorder(int x, int y, int dir)
    {
        return (borders[y][x][dir]);
    }

    public int TotalIncome(int country)
    {
        int income = 0;
        for (int y = 0; y < spaces.Length; ++y)
        {
            for(int x = 0; x < spaces[y].Length; ++x)
            {
                if (spaces[y][x].Controller == country)
                {
                    income += spaces[y][x].Income;
                }
            }
        }
        return (income);
    }
}
