using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private static Game game;
    public static Game GetGame()
    {
        if(game == null)
        {
            game = new Game();
        }
        return (game);
    }
    public static Game CreateGame(ScenarioData scenario)
    {
        game = new Game(scenario);
        return (game);
    }

    private ScenarioData scenario;
    private Map map;
    private Country[] countries;
    private Dictionary<int, int> countryIndexs;
    private List<Unit>[] units;
    private List<Unit>[][] unitsInSpace;

    private List<int> doneCountries;
    private List<int> actingCountries;
    public int currentCountry;
    public Data.GameType gameType;
    public Data.GamePhase currentPhase;

    public Game()
    {
        map = new Map();
        countries = new Country[0];
        countryIndexs = new Dictionary<int, int>();
        units = new List<Unit>[0];
        currentPhase = Data.GamePhase.springPlanning;
        gameType = Data.GameType.hotseat;
        doneCountries = new List<int>();
        actingCountries = new List<int>();
    }
    public Game(ScenarioData data)
    {
        scenario = data;
        countryIndexs = new Dictionary<int, int>();
        map = new Map(data.map);
        countries = new Country[data.countries.Length];
        units = new List<Unit>[data.countries.Length];
        unitsInSpace = new List<Unit>[map.Height][];
        for(int i = 0; i < unitsInSpace.Length; ++i)
        {
            unitsInSpace[i] = new List<Unit>[map.Width];
            for(int j = 0; j < unitsInSpace[i].Length; ++j)
            {
                unitsInSpace[i][j] = new List<Unit>();
            }
        }
        for(int i = 0; i < countries.Length; ++i)
        {
            countries[i] = new Country(data.countries[i], Data.PlayerType.player);
            countryIndexs.Add(countries[i].Id, i);
            units[i] = countries[i].units;
            for(int u = 0; u < units[i].Count; ++u)
            {
                Vector2Int space = units[i][u].Space;
                unitsInSpace[space.y][space.x].Add(units[i][u]);
            }
        }
        currentPhase = Data.GamePhase.production;
        gameType = Data.GameType.hotseat;
        doneCountries = new List<int>();
        actingCountries = new List<int>();
        EndTurn();
    }

    public Map Map
    {
        get { return (map); }
    }
    public bool IsMovePhase
    {
        get { return ((currentPhase == Data.GamePhase.spring || currentPhase == Data.GamePhase.summer || currentPhase == Data.GamePhase.autumn)); }
    }

    public Country GetCountry()
    {
        return (countries[countryIndexs[currentCountry]]);
    }
    public Country GetCountry(int countryId)
    {
        return (countries[countryIndexs[countryId]]);
    }

    public List<Unit> GetCountryUnits(int countryId)
    {
        return (units[countryIndexs[countryId]]);
    }
    public List<Unit> GetUnitsInSpace(int x, int y)
    {
        /*List<Unit> u = new List<Unit>();
        for(int i = 0; i < units.Length; ++i)
        {
            for(int j = 0; j < units[i].Count; ++j)
            {
                if(units[i][j].Space.x == x && units[i][j].Space.y == y)
                {
                    u.Add(units[i][j]);
                }
            }
        }
        return (u);*/
        return (unitsInSpace[y][x]);
    }
    public List<Unit> GetUnitsInSpace(Vector2Int position)
    {
        return (GetUnitsInSpace(position.x, position.y));
    }

    public void MoveUnit(Unit unit, DistanceSpace distanceSpace)
    {
        unitsInSpace[unit.Space.y][unit.Space.x].Remove(unit);
        unit.Space = distanceSpace.path[distanceSpace.path.Length - 1];
        for(int i = 0; i < distanceSpace.path.Length - 1; ++i)
        {
            unit.path.Push(distanceSpace.path[i]);
        }
        unit.Movement -= distanceSpace.distance;
        map.UpdateBorders(distanceSpace.path);
        unitsInSpace[unit.Space.y][unit.Space.x].Add(unit);
    }

    public void UndoLastMove(Unit unit)
    {
        if(unit.path.Count > 0)
        {
            Vector2Int current = unit.Space;
            unit.Space = unit.path.Pop();
            map.UndoMove(unit.Space, current);
        }
    }

    public void EndPhase()
    {
        actingCountries = new List<int>();
        for(int i = 0; i < countries.Length; ++i)
        {
            actingCountries.Add(countries[i].Id);
        }
        doneCountries = new List<int>();
        currentPhase = (Data.GamePhase)(((int)currentPhase + 1) % 9);
        if(IsMovePhase)
        {
            SortCountries();
        }
        else
        {
            ResetCardPlayed();
        }
    }

    public void SortCountries()
    {
        for(int i = 0; i < actingCountries.Count; ++i)
        {
            if(countries[countryIndexs[actingCountries[i]]].plannedCard == null)
            {
                doneCountries.Add(actingCountries[i]);
                actingCountries.RemoveAt(i);
                --i;
            }
        }
        SortHelper(0, actingCountries.Count);
    }
    private void SortHelper(int min, int max)
    {
        if(max <= min + 1)
        {
            return;
        }
        int pivot = min;
        for (int i = min + 1; i < max; ++i)
        {
            if (countries[countryIndexs[actingCountries[i]]].plannedCard.Initiative < countries[countryIndexs[actingCountries[pivot]]].plannedCard.Initiative
                || (countries[countryIndexs[actingCountries[i]]].plannedCard.Initiative == countries[countryIndexs[actingCountries[pivot]]].plannedCard.Initiative 
                && countries[countryIndexs[actingCountries[i]]].InitiativeTiebreaker < countries[countryIndexs[actingCountries[pivot]]].InitiativeTiebreaker))
            {
                int temp = actingCountries[pivot];
                actingCountries[pivot] = actingCountries[pivot + 1];
                actingCountries[pivot + 1] = actingCountries[i];
                actingCountries[i] = temp;
                ++pivot;
            }
        }
        SortHelper(min, pivot);
        SortHelper(pivot + 1, max);
    }

    public void EndTurn()
    {
        Data.selectedCard = null;
        Data.selectedSpace = null;
        Data.selectedUnit = null;
        Data.rightClicked = null;
        if (IsMovePhase)
        {
            ResolveBattles();
            CaptureSpaces();
            ResetMovement();
        }
        doneCountries.Add(currentCountry);
        while (actingCountries.Count <= 0)
        {
            EndPhase();
        }
        currentCountry = actingCountries[0];
        actingCountries.RemoveAt(0);
        if(countries[countryIndexs[currentCountry]].playerType == Data.PlayerType.player && gameType == Data.GameType.hotseat)
        {
            Data.localId = currentCountry;
        }
        if(currentPhase == Data.GamePhase.production)
        {
            Country country = countries[countryIndexs[currentCountry]];
            country.Money += map.TotalIncome(currentCountry);
            country.Money += country.industry;
            country.purchasedCards = false;
            country.purchasedTech = false;
        }
    }

    public void ResolveBattles()
    {
        for (int y = 0; y < unitsInSpace.Length; ++y)
        {
            for (int x = 0; x < unitsInSpace[y].Length; ++x)
            {
                if (unitsInSpace[y][x].Count > 0)
                {
                    if(unitsInSpace[y][x][0].Id != unitsInSpace[y][x][unitsInSpace[y][x].Count - 1].Id)
                    {
                        ResolveBattle(x, y);
                    }
                }
            }
        }
    }

    public void ResolveBattle(int x, int y)
    {
        List<Unit> defenders = new List<Unit>();
        List<Unit> attackers = new List<Unit>();
        defenders.Add(unitsInSpace[y][x][0]);
        for(int i = 1; i < unitsInSpace[y][x].Count; ++i)
        {
            if(unitsInSpace[y][x][i].Id == defenders[0].Id)
            {
                defenders.Add(unitsInSpace[y][x][i]);
            }
            else
            {
                attackers.Add(unitsInSpace[y][x][i]);
            }
        }
        for (int a = 0; a < attackers.Count; ++a)
        {
            List<int> index = new List<int>();
            for (int d = 0; d < defenders.Count; ++d)
            {
                if (defenders[d].PlannedDamage < defenders[d].Health)
                {
                    index.Add(d);
                }
            }
            Unit target;
            if (index.Count == 0)
            {
                target = defenders[0];
            }
            else
            {
                target = defenders[index[Random.Range(0, index.Count)]];
            }
            float damage = CalculateDamage(attackers[a], target, true);
            target.PlannedDamage += (int)(damage * (1 - attackers[a].InstantDamagePercentage));
            target.Health -= (int)(damage * attackers[a].InstantDamagePercentage);
        }
        for (int d = 0; d < defenders.Count; ++d)
        {
            List<int> index = new List<int>();
            for (int a = 0; a < attackers.Count; ++a)
            {
                if (attackers[a].PlannedDamage < attackers[a].Health)
                {
                    index.Add(a);
                }
            }
            Unit target;
            if (index.Count == 0)
            {
                target = attackers[0];
            }
            else
            {
                target = attackers[index[Random.Range(0, index.Count)]];
            }
            float damage = CalculateDamage(defenders[d], target, false);
            int instantDamage = Mathf.CeilToInt(damage * defenders[d].InstantDamagePercentage);
            if(instantDamage > 0)
            {
                target.Health -= instantDamage;
                target.PlannedDamage += Mathf.FloorToInt(damage * (1 - defenders[d].InstantDamagePercentage));
            }
            else
            {
                target.PlannedDamage += Mathf.CeilToInt(damage * (1 - defenders[d].InstantDamagePercentage));
            }
        }
        TakeDamage(x, y);
        Retreat(x, y);
    }

    public float CalculateDamage(Unit attacker, Unit defender, bool isAttacker)
    {
        int attack = attacker.Attack - defender.Defense;
        if (isAttacker)
        {
            Vector2Int previous = attacker.path.Peek();
            int direction = Data.GetDirection(previous, attacker.Space);
            attack += Map.GetBorder(previous.x, previous.y, direction).data.attackModifier;
            attack -= Map.GetSpace(defender.Space.x, defender.Space.y).Defense;
        }
        float damage = attack * Random.Range(0.9f, 1f);
        if(attacker.InstantDamagePercentage > 0)
        {
            damage *= ((float)attacker.Health) / attacker.MaxHealth;
        }
        else
        {
            damage = (((float)attacker.PlannedDamage) / attacker.MaxHealth * attacker.UsedDamagePercentage * damage) + ((float)(attacker.Health - attacker.PlannedDamage) / attacker.MaxHealth * damage);
        }
        if(damage < 1)
        {
            damage = 1;
        }
        Debug.Log(attacker.Name + " dealt " + damage + " damage to " + defender.Name);
        return (damage);
    }

    public void TakeDamage(int x, int y)
    {
        for(int i = 0; i < unitsInSpace[y][x].Count; ++i)
        {
            Unit unit = unitsInSpace[y][x][i];
            unit.Health -= unit.PlannedDamage;
            if (unit.Health <= 0)
            {
                unitsInSpace[y][x].RemoveAt(i);
                --i;
                units[countryIndexs[unit.Id]].Remove(unit);
                countries[countryIndexs[unit.Id]].units.Remove(unit);
            }
        }
    }

    public void Retreat(int x, int y)
    {
        if(unitsInSpace[y][x].Count <= 0)
        {
            return;
        }
        int defenderId = unitsInSpace[y][x][0].Id;
        for(int i = 1; i < unitsInSpace[y][x].Count; ++i)
        {
            if(unitsInSpace[y][x][i].Id != defenderId)
            {
                Vector2Int space = unitsInSpace[y][x][i].path.Pop();
                unitsInSpace[y][x][i].Space = space;
                unitsInSpace[space.y][space.x].Add(unitsInSpace[y][x][i]);
                unitsInSpace[y][x].RemoveAt(i);
                --i;
            }
        }
    }

    public void CaptureSpaces()
    {
        for (int y = 0; y < unitsInSpace.Length; ++y)
        {
            for (int x = 0; x < unitsInSpace[y].Length; ++x)
            {
                if (unitsInSpace[y][x].Count > 0)
                {
                    Map.GetSpace(x, y).Controller = unitsInSpace[y][x][0].Id;
                }
            }
        }
    }

    public void ResetMovement()
    {
        for(int i = 0; i < units.Length; ++i)
        {
            foreach(Unit unit in units[i])
            {
                unit.ResetMovement();
            }
        }
    }

    public void ResetCardPlayed()
    {
        for(int i = 0; i < countries.Length; ++i)
        {
            countries[i].plannedCard = null;
        }
    }

    public void AddUnit(Unit unit)
    {
        GetCountry().units.Add(unit);
        unitsInSpace[unit.Space.y][unit.Space.x].Add(unit);
        units[countryIndexs[unit.Id]].Add(unit);
    }

    public int StackingModifier(Vector2Int space)
    {
        return (map.GetSpace(space.x, space.y).StackingModifier);
    }

    public int StackingLimit(Vector2Int space)
    {
        int countryLimt = countries[countryIndexs[currentCountry]].SeaStackingLimit;
        if(map.GetSpace(space.x, space.y).Land)
        {
            countryLimt = countries[countryIndexs[currentCountry]].LandStackingLimit;
        }
        return (Mathf.Max(1, map.GetSpace(space.x, space.y).StackingModifier + countryLimt));
    }

    public bool HasSeaUnit(Vector2Int space)
    {
        List<Unit> units = unitsInSpace[space.y][space.x];
        foreach(Unit unit in units)
        {
            if(unit.MoveType == Data.MoveType.sea)
            {
                return (true);
            }
        }
        return (false);
    }

    public string GetRecruitTitle(int index)
    {
        return (scenario.recruitNames[index]);
    }
}
