using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitDataDisplay : MonoBehaviour, IPointerClickHandler
{
    public UnitData unit;
    public Image unitSprite;
    public Text description;
    public Image background;

    // Update is called once per frame
    void Update()
    {
        if (unit == null)
        {
            unitSprite.sprite = null;
            description.text = "";
        }
        else
        {
            unitSprite.sprite = unit.sprite;
            description.text = unit.GetDescription();
        }
        if(Data.selectedUnitData == null || Data.selectedUnitData != unit)
        {
            background.color = new Color(158 / 255f, 158 / 255f, 158 / 255f);
        }
        else
        {
            background.color = new Color(1, 1, 1);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Data.rightClicked = null;
            Data.selectedUnitData = unit;
        }
    }
}
