using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public ScenarioData testScenario;
    public GameObject spaceHud;
    public GameObject unitHud;
    public GameObject endTurnButton;
    public GameObject phaseDisplay;
    private Text phaseDisplayText;
    public GameObject CardListButton;
    private Text remainingMovementText;
    public GameObject remainingMovement;
    // Start is called before the first frame update
    void Start()
    {
        Game game = Game.CreateGame(testScenario);
        GameObject spacePrefab = Resources.Load<GameObject>("Space");
        GameObject borderPrefab = Resources.Load<GameObject>("Border");
        GameObject linePrefab = Resources.Load<GameObject>("Line");
        for (int y = 0; y < game.Map.NumSpaces; ++y)
        {
            Space spaceData = game.Map.GetSpace(y);
            GameObject space = Instantiate(spacePrefab, Data.GetPosition(spaceData.Position), new Quaternion());
            space.GetComponent<SpaceDisplay>().space = spaceData;
            space.GetComponent<SpaceDisplay>().spaceBackground.sprite = spaceData.Sprite;
            for (int i = 0; i < spaceData.NumAdjacent; ++i)
            {
                Vector2 startPos = Data.GetPosition(spaceData.Position);
                Vector2 endPos = Data.GetPosition(spaceData.GetAdjacent(i).destination.Position);
                Vector2 midPos = new Vector2((startPos.x + endPos.x) / 2, (startPos.y + endPos.y) / 2);
                GameObject line = Instantiate(linePrefab);
                line.GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(startPos.x, startPos.y, 0), new Vector3(endPos.x, endPos.y, 0) });
                GameObject border = Instantiate(borderPrefab, midPos, new Quaternion());
                border.GetComponent<SpriteRenderer>().sprite = spaceData.GetAdjacent(i).data.sprite;
            }
        }
        phaseDisplayText = phaseDisplay.GetComponentInChildren<Text>();
        remainingMovementText = remainingMovement.GetComponentInChildren<Text>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 defaultRes = Data.defaultRes;
        float heightRatio = defaultRes.x * Screen.height / defaultRes.y / Screen.width;
        spaceHud.transform.localPosition = new Vector3(defaultRes.x / -2, defaultRes.y * heightRatio / 2, 0);
        unitHud.transform.localPosition = new Vector3(defaultRes.x / 2, defaultRes.y * heightRatio / 2, 0);
        endTurnButton.transform.localPosition = new Vector3(defaultRes.x / 2, defaultRes.y * heightRatio / -2, 0);
        endTurnButton.SetActive(Game.GetGame().currentCountry == Data.localId);
        phaseDisplay.transform.localPosition = new Vector3(0, defaultRes.y * heightRatio / 2, 0);
        phaseDisplayText.text = "" + Game.GetGame().currentPhase + "\n" + Game.GetGame().GetCountry().CountryName;
        CardListButton.transform.localPosition = new Vector3(defaultRes.x / -2, defaultRes.y * heightRatio / -2, 0);
        CardListButton.SetActive(Game.GetGame().currentCountry == Data.localId && !Game.GetGame().IsMovePhase);
        remainingMovement.transform.localPosition = new Vector3(0, defaultRes.y * heightRatio / -2, 0);
        if (Game.GetGame().GetCountry().plannedCard != null)
        {
            remainingMovement.SetActive(true);
            remainingMovementText.text = "" + (Game.GetGame().GetCountry().plannedCard.NumUnits - Game.GetGame().GetCountry().NumberOfMovedUnits);
        }
        else if(Game.GetGame().currentPhase == Data.GamePhase.production)
        {
            remainingMovement.SetActive(true);
            remainingMovementText.text = "Money: " + Game.GetGame().GetCountry().Money;
        }
        else
        {
            remainingMovement.SetActive(false);
        }
    }
}
