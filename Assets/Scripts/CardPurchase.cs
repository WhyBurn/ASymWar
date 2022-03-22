using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPurchase : MonoBehaviour
{
    public Text text;
    public int numCards;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "" + numCards;
    }

    public void Increase()
    {
        Country country = Game.GetGame().GetCountry();
        if(country.hand.Count + numCards < country.HandSize && country.Money > numCards)
        {
            ++numCards;
        }
    }
    public void Decrease()
    {
        if(numCards > 0)
        {
            --numCards;
        }
    }
}
