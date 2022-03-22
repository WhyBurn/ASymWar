using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private CardData data;

    public Card(CardData d)
    {
        data = d;
    }

    public int Initiative
    {
        get { return (data.initiative); }
    }
    public int NumUnits
    {
        get { return (data.numUnits); }
    }

    public string GetDescription()
    {
        string description = (char)(data.initiative + 65) + " " + data.numUnits;
        return (description);
    }
}
