using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IPointerClickHandler
{
    public Card card;
    public Text description;
    public Image background;

    // Update is called once per frame
    void Update()
    {
        if (card == null)
        {
            description.text = "";
        }
        else
        {
            description.text = card.GetDescription();
        }
        if(Data.selectedCard == null || Data.selectedCard != card)
        {
            background.color = new Color(190 / 255f, 190 / 255f, 190 / 255f);
        }
        else
        {
            background.color = new Color(250 / 255f, 250 / 255f, 0);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Data.rightClicked = null;
            Data.selectedCard = card;
        }
    }
}
