using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceDisplay : MonoBehaviour, IPointerClickHandler
{
    public SpriteRenderer spaceBackground;
    public SpriteRenderer control;
    public SpriteRenderer defenders;
    public SpriteRenderer attackers;
    public SpriteRenderer numDefenders;
    public SpriteRenderer numAttackers;
    public GameObject moveSelect;

    public Space space;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(space.Controller >= 0)
        {
            Sprite sprite = Game.GetGame().GetCountry(space.Controller).ControlSprite;
            control.sprite = sprite;
        }
        else
        {
            control.sprite = null;
        }
        List<Unit> units = Game.GetGame().GetUnitsInSpace(space.Position);
        if(units.Count > 0)
        {
            defenders.sprite = Game.GetGame().GetCountry(units[0].Id).ControlSprite;
            int num = 0;
            for(int i = 1; i < units.Count; ++i)
            {
                if(units[i].Id != units[0].Id)
                {
                    attackers.sprite = Game.GetGame().GetCountry(units[i].Id).ControlSprite;
                    ++num;
                }
            }
            numDefenders.sprite = Resources.Load<Sprite>("Number" + Mathf.Min(units.Count - num, 9));
            if (num > 0)
            {
                numAttackers.sprite = Resources.Load<Sprite>("Number" + Mathf.Min(num, 9));
            }
            else
            {
                attackers.sprite = null;
                numAttackers.sprite = null;
            }
        }
        else
        {
            defenders.sprite = null;
            attackers.sprite = null;
            numAttackers.sprite = null;
            numDefenders.sprite = null;
        }
        moveSelect.SetActive(space.availableMove != null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Data.selectedSpace = space;
            Data.rightClicked = null;
            Data.selectedUnit = null;
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            Data.rightClicked = space;
        }
    }
}
