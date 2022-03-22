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
    private List<Unit>[] unitsInSpace;

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
        unitsInSpace = new List<Unit>[map.NumSpaces];
        for(int i = 0; i < unitsInSpace.Length; ++i)
        {
            unitsInSpace[i] = new List<Unit>();
        }
        for(int i = 0; i < countries.Length; ++i)
        {
            countries[i] = new Country(data.countries[i], Data.PlayerType.player);
            countryIndexs.Add(countries[i].Id, i);
            units[i] = countries[i].units;
            for(int u = 0; u < units[i].Count; ++u)
            {
                int space = units[i][u].Space;
                unitsInSpace[space].Add(units[i][u]);
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
    public List<Unit> GetUnitsInSpace(int id)
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
        return (unitsInSpace[id]);
    }

    public void MoveUnit(Unit unit, DistanceSpace distanceSpace)
    {
        unitsInSpace[unit.Space].Remove(unit);
        unit.Space = distanceSpace.path[distanceSpace.path.Length - 1];
        for(int i = 0; i < distanceSpace.path.Length - 1; ++i)
        {
            unit.path.Push(distanceSpace.path[i]);
        }
        unit.Movement -= distanceSpace.distance;
        map.UpdateBorders(distanceSpace.path);
        unitsInSpace[unit.Space].Add(unit);
    }

    public void UndoLastMove(Unit unit)
    {
        if(unit.path.Count > 0)
        {
            int current = unit.Space;
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
            if (unitsInSpace[y].Count > 0)
            {
                if (unitsInSpace[y][0].Id != unitsInSpace[y][unitsInSpace[y].Count - 1].Id)
                {
                    ResolveBattle(y);
                }
            }

        }
    }

    public void ResolveBattle(int id)
    {
        List<Unit> defenders = new List<Unit>();
        List<Unit> attackers = new List<Unit>();
        defenders.Add(unitsInSpace[id][0]);
        for(int i = 1; i < unitsInSpace[id].Count; ++i)
        {
            if(unitsInSpace[id][i].Id == defenders[0].Id)
            {
                defenders.Add(unitsInSpace[id][i]);
            }
            else
            {
                attackers.Add(unitsInSpace[id][i]);
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
        TakeDamage(id);
        Retreat(id);
    }

    public float CalculateDamage(Unit attacker, Unit defender, bool isAttacker)
    {
        int attack = attacker.Attack - defender.Defense;
        if (isAttacker)
        {
            int previous = attacker.path.Peek();
            attack += Map.GetBorder(previous, attacker.Space).data.attackModifier;
            attack -= Map.GetSpace(defender.Space).Defense;
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

    public void TakeDamage(int id)
    {
        for(int i = 0; i < unitsInSpace[id].Count; ++i)
        {
            Unit unit = unitsInSpace[id][i];
            unit.Health -= unit.PlannedDamage;
            if (unit.Health <= 0)
            {
                unitsInSpace[id].RemoveAt(i);
                --i;
                units[countryIndexs[unit.Id]].Remove(unit);
                countries[countryIndexs[unit.Id]].units.Remove(unit);
            }
        }
    }

    public void Retreat(int id)
    {
        if(unitsInSpace[id].Count <= 0)
        {
            return;
        }
        int defenderId = unitsInSpace[id][0].Id;
        for(int i = 1; i < unitsInSpace[id].Count; ++i)
        {
            if(unitsInSpace[id][i].Id != defenderId)
            {
                int space = unitsInSpace[id][i].path.Pop();
                unitsInSpace[id][i].Space = space;
                unitsInSpace[space].Add(unitsInSpace[id][i]);
                unitsInSpace[id].RemoveAt(i);
                --i;
            }
        }
    }

    public void CaptureSpaces()
    {
        for (int y = 0; y < unitsInSpace.Length; ++y)
        {
            if (unitsInSpace[y].Count > 0)
            {
                Map.GetSpace(y).Controller = unitsInSpace[y][0].Id;
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
        unitsInSpace[unit.Space].Add(unit);
        units[countryIndexs[unit.Id]].Add(unit);
    }

    public int StackingModifier(int space)
    {
        return (map.GetSpace(space).StackingModifier);
    }

    public int StackingLimit(int space)
    {
        int countryLimt = countries[countryIndexs[currentCountry]].SeaStackingLimit;
        if(map.GetSpace(space).Land)
        {
            countryLimt = countries[countryIndexs[currentCountry]].LandStackingLimit;
        }
        return (Mathf.Max(1, map.GetSpace(space).StackingModifier + countryLimt));
    }

    public bool HasSeaUnit(int space)
    {
        List<Unit> units = unitsInSpace[space];
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
