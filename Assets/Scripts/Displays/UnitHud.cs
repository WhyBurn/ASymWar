using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHud : MonoBehaviour
{
    private Unit displayedUnit;

    public Text textDisplay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(Unit unit)
    {
        displayedUnit = unit;
        if (unit.Id == Data.localId)
        {
            textDisplay.text = unit.GetDescription();
        }
        else
        {
            textDisplay.text = "Unknown Enemy Unit";
        }
    }
    public void Deactivate()
    {
        textDisplay.text = "";
    }
}
