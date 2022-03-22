using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country
{
    private CountryData data;
    private int money;
    private List<Card> deck;
    private List<Card> discard;
    public List<Card> hand;
    public List<Unit> units;
    public Data.PlayerType playerType;
    public Card plannedCard;
    public bool purchasedCards;
    public bool purchasedTech;
    public int industry;

    public Country(CountryData d, Data.PlayerType p)
    {
        data = d;
        money = d.startingMoney;
        units = new List<Unit>();
        for(int i = 0; i < d.startPositions.Length; ++i)
        {
            units.Add(new Unit(d.startPositions[i].unit, new Vector2Int(d.startPositions[i].xPos, d.startPositions[i].yPos), d.id));
        }
        deck = new List<Card>();
        discard = new List<Card>();
        hand = new List<Card>();
        plannedCard = null;
        for(int i = 0; i < d.deck.Length; ++i)
        {
            deck.Add(new Card(d.deck[i]));
        }
        playerType = p;
        DrawCards(data.numStartingCards);
        purchasedCards = false;
        purchasedTech = false;
        industry = data.startingIndustry;
    }

    public int Id
    {
        get { return (data.id); }
    }
    public int InitiativeTiebreaker
    {
        get { return (data.initiativeTiebreaker); }
    }
    public string CountryName
    {
        get { return (data.countryName); }
    }
    public Sprite ControlSprite
    {
        get { return (data.controlSprite);}
    }
    public int HandSize
    {
        get { return (data.handSize); }
    }
    public UnitData[] AvailableUnits
    {
        get { return (data.availableUnits); }
    }
    public int IndustryUpgradeCost
    {
        get { return (data.industryUpgradeCost); }
    }
    public int LandStackingLimit
    {
        get { return (data.landStackingLimit); }
    }
    public int SeaStackingLimit
    {
        get { return (data.seaStackingLimit); }
    }

    public int Money
    {
        get { return (money); }
        set { money = value; }
    }

    public void Shuffle()
    {
        Card[] cards = new Card[deck.Count + discard.Count];
        for (int i = 0; i < deck.Count; ++i)
        {
            cards[i] = deck[i];
        }
        for (int i = 0; i < discard.Count; ++i)
        {
            cards[i + deck.Count] = discard[i];
        }
        List<int> positions = new List<int>();
        for(int i = 0; i < cards.Length; ++i)
        {
            positions.Add(i);
        }
        List<Card> newDeck = new List<Card>();
        while(positions.Count > 0)
        {
            int posIndex = Random.Range(0, positions.Count);
            newDeck.Add(cards[positions[posIndex]]);
            positions.RemoveAt(posIndex);
        }
        deck = newDeck;
        discard = new List<Card>();
    }

    public void DrawCards(int num)
    {
        for(int i = 0; i < num; ++i)
        {
            if(deck.Count <= 0)
            {
                if(discard.Count <= 0)
                {
                    return;
                }
                Shuffle();
            }
            hand.Add(deck[0]);
            deck.RemoveAt(0);
        }
    }

    public int NumberOfMovedUnits
    {
        get
        {
            int count = 0;
            for(int i = 0; i < units.Count; ++i)
            {
                if(units[i].path.Count > 0)
                {
                    ++count;
                }
            }
            return (count);
        }
    }

    public void PlayCard(Card card)
    {
        plannedCard = card;
        hand.Remove(card);
        discard.Add(card);
    }

    public bool CanRecruit(int recruitType)
    {
        for(int i = 0; i < data.possibleRecruits.Length; ++i)
        {
            if(data.possibleRecruits[i] == recruitType)
            {
                return (true);
            }
        }
        return (false);
    }
}
