using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public SpaceHud spaceHud;
    public UnitHud unitHud;
    public GameObject unitContent;
    public GameObject cardContent;
    public GameObject unitDataContent;
    public GameObject handDisplay;
    public GameObject selectCardButtons;
    public GameObject purchaseCardButtons;
    public CardPurchase cardPurchasePanel;
    public GameObject recruitUnitButton;
    public GameObject recruitUnitPanel;
    public GameObject upgradeIndustryButton;
    public GameObject purchaseTechnologyButton;

    private bool spaceUI;
    private bool unitUI;
    private Space currentSpace;
    private Unit currentUnit;
    // Start is called before the first frame update
    void Start()
    {
        handDisplay.SetActive(false);
        recruitUnitPanel.SetActive(false);
        cardPurchasePanel.gameObject.SetActive(false);
        spaceHud.gameObject.SetActive(false);
        unitHud.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Game game = Game.GetGame();
        if(!spaceUI && Data.selectedSpace != null)
        {
            spaceHud.gameObject.SetActive(true);
            spaceHud.Activate(Data.selectedSpace);
            spaceUI = true;
            currentSpace = Data.selectedSpace;
            ResetUnitContent();
        }
        if(!unitUI && Data.selectedUnit != null)
        {
            unitHud.gameObject.SetActive(true);
            unitHud.Activate(Data.selectedUnit);
            unitUI = true;
            currentUnit = Data.selectedUnit;
            if (game.currentCountry == Data.localId && game.IsMovePhase)
            {
                game.Map.SetMovement(Data.selectedUnit.Space, Data.selectedUnit.Movement, Data.selectedUnit.Id, Data.selectedUnit.MoveType);
            }
        }
        if (Data.rightClicked != null)
        {
            if (Data.selectedUnit != null && Data.rightClicked.availableMove != null && game.currentCountry == Data.localId && game.IsMovePhase
                && game.GetUnitsInSpace(Data.rightClicked.SpaceID).Count < game.StackingLimit(Data.rightClicked.SpaceID) 
                && (Data.selectedUnit.path.Count > 0 || game.GetCountry().NumberOfMovedUnits < game.GetCountry().plannedCard.NumUnits))
            {
                Data.selectedSpace = Data.rightClicked;
                game.MoveUnit(Data.selectedUnit, Data.rightClicked.availableMove);
                unitHud.Activate(Data.selectedUnit);
                ResetUnitContent();
                game.Map.SetMovement(Data.rightClicked.SpaceID, Data.selectedUnit.Movement, Data.selectedUnit.Id, Data.selectedUnit.MoveType);
            }
            Data.rightClicked = null;
        }
        if(Data.selectedUnit == null || currentUnit != Data.selectedUnit)
        {
            unitUI = false;
            unitHud.Deactivate();
            game.Map.ResetSpaceMovement();
            unitHud.gameObject.SetActive(false);
        }
        if(Data.selectedSpace == null || Data.selectedSpace != currentSpace)
        {
            spaceUI = false;
            spaceHud.Deactivate();
            ClearUnitContent();
            spaceHud.gameObject.SetActive(false);
        }
        if (Data.selectedSpace != null && game.currentPhase == Data.GamePhase.production 
            && currentSpace.Recruit && game.GetCountry(Data.localId).CanRecruit(currentSpace.RecruitType))
        {
            recruitUnitButton.SetActive(true);
        }
        else
        {
            recruitUnitButton.SetActive(false);
        }
        if(game.currentPhase == Data.GamePhase.production && game.currentCountry == Data.localId && Data.selectedSpace == null)
        {
            if(!game.GetCountry().purchasedTech)
            {
                purchaseTechnologyButton.SetActive(true);
            }
            else
            {
                purchaseTechnologyButton.SetActive(false);
            }
            if (game.GetCountry().IndustryUpgradeCost > 0)
            {
                upgradeIndustryButton.SetActive(true);
                upgradeIndustryButton.GetComponentInChildren<Text>().text = "Upgrade Industry Cost: " + game.GetCountry().IndustryUpgradeCost;
            }
            else
            {
                upgradeIndustryButton.SetActive(false);
            }
        }
        else
        {
            purchaseTechnologyButton.SetActive(false);
            upgradeIndustryButton.SetActive(false);
        }
    }

    public void ClearUnitContent()
    {
        for(int i = 0; i < unitContent.transform.childCount; ++i)
        {
            GameObject.Destroy(unitContent.transform.GetChild(i).gameObject);
        }
    }

    public void ResetUnitContent()
    {
        ClearUnitContent();
        List<Unit> units = Game.GetGame().GetUnitsInSpace(Data.selectedSpace.SpaceID);
        foreach(Unit unit in units)
        {
            GameObject display = Instantiate(Resources.Load<GameObject>("UnitDisplay"), unitContent.transform);
            display.GetComponent<UnitDisplay>().unit = unit;
        }
    }

    public void ClearCardContent()
    {
        for (int i = 0; i < cardContent.transform.childCount; ++i)
        {
            GameObject.Destroy(cardContent.transform.GetChild(i).gameObject);
        }
    }

    public void ResetCardContent()
    {
        ClearCardContent();
        List<Card> hand = Game.GetGame().GetCountry().hand;
        foreach(Card card in hand)
        {
            GameObject display = Instantiate(Resources.Load<GameObject>("CardDisplay"), cardContent.transform);
            display.GetComponent<CardDisplay>().card = card;
        }
    }

    public void ClearUnitDataContent()
    {
        for (int i = 0; i < unitDataContent.transform.childCount; ++i)
        {
            GameObject.Destroy(unitDataContent.transform.GetChild(i).gameObject);
        }
    }

    public void ResetUnitDataContent()
    {
        ClearUnitDataContent();
        UnitData[] units = Game.GetGame().GetCountry(Data.localId).AvailableUnits;
        foreach (UnitData unit in units)
        {
            GameObject display = Instantiate(Resources.Load<GameObject>("UnitDataDisplay"), unitDataContent.transform);
            display.GetComponent<UnitDataDisplay>().unit = unit;
        }
    }

    public void EndTurn()
    {
        handDisplay.SetActive(false);
        cardPurchasePanel.gameObject.SetActive(false);
        Game.GetGame().EndTurn();
    }

    public void SelectCard()
    {
        if(Data.selectedCard != null)
        {
            Game.GetGame().GetCountry().PlayCard(Data.selectedCard);
            Game.GetGame().EndTurn();
        }
    }

    public void ChangeHand()
    {
        handDisplay.SetActive(!handDisplay.activeSelf);
        if(Game.GetGame().currentPhase == Data.GamePhase.production)
        {
            if(!Game.GetGame().GetCountry().purchasedCards)
            {
                purchaseCardButtons.SetActive(true);
            }
            else
            {
                purchaseCardButtons.SetActive(false);
            }
            selectCardButtons.SetActive(false);
        }
        else
        {
            selectCardButtons.SetActive(true);
            purchaseCardButtons.SetActive(false);
        }
    }

    public void ChangeCardPurchasePanel()
    {
        cardPurchasePanel.gameObject.SetActive(!cardPurchasePanel.gameObject.activeSelf);
        cardPurchasePanel.numCards = 0;
    }

    public void ChangeRecruitPanel()
    {
        recruitUnitPanel.SetActive(!recruitUnitPanel.activeSelf);
    }

    public void PurchaseCards()
    {
        if(cardPurchasePanel.numCards > 0)
        {
            Country country = Game.GetGame().GetCountry();
            country.DrawCards(cardPurchasePanel.numCards);
            country.Money -= cardPurchasePanel.numCards;
            country.purchasedCards = true;
            purchaseCardButtons.SetActive(false);
        }
        cardPurchasePanel.gameObject.SetActive(false);
        ResetCardContent();
    }

    public void RecruitSelected()
    {
        if(Data.selectedUnitData != null && Data.selectedUnitData.cost <= Game.GetGame().GetCountry().Money 
            && Game.GetGame().StackingLimit(Data.selectedSpace.SpaceID) > Game.GetGame().GetUnitsInSpace(Data.selectedSpace.SpaceID).Count)
        {
            Game.GetGame().GetCountry().Money -= Data.selectedUnitData.cost;
            Unit newUnit = new Unit(Data.selectedUnitData, Data.selectedSpace.SpaceID, Game.GetGame().currentCountry);
            Game.GetGame().AddUnit(newUnit);
            recruitUnitPanel.SetActive(false);
            Data.selectedUnitData = null;
            ResetUnitContent();
        }
    }

    public void UpgradeIndustry()
    {
        if(Game.GetGame().GetCountry().IndustryUpgradeCost > 0 && Game.GetGame().GetCountry().Money > Game.GetGame().GetCountry().IndustryUpgradeCost)
        {
            Game.GetGame().GetCountry().industry++;
            Game.GetGame().GetCountry().Money -= Game.GetGame().GetCountry().IndustryUpgradeCost;
        }
    }
}
