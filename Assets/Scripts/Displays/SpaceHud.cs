using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceHud : MonoBehaviour
{
    private Space displayedSpace;

    public Text spaceDescription;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(Space space)
    {
        displayedSpace = space;
        spaceDescription.text = space.GetDescription();
    }
    public void Deactivate()
    {
        spaceDescription.text = "";
    }
}
