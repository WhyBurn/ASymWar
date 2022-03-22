using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitDisplay : MonoBehaviour, IPointerClickHandler
{
    public Unit unit;
    public Image unitSprite;
    public Text description;

    // Update is called once per frame
    void Update()
    {
        if(unit == null)
        {
            unitSprite.sprite = null;
            description.text = "";
        }
        else
        {
            if (unit.Id == Data.localId)
            {
                unitSprite.sprite = unit.Sprite;
                description.text = unit.GetDescription();
            }
            else
            {
                unitSprite.sprite = Game.GetGame().GetCountry(unit.Id).ControlSprite;
                description.text = "Unknown Enemy Unit";
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Data.rightClicked = null;
            Data.selectedUnit = unit;
        }
    }
}
