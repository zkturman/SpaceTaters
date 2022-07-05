using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeonicToolSetBehaviour : MonoBehaviour
{
    private MonsterMaker monsterMaker;
    [SerializeField]
    private InputField xCoord;
    [SerializeField]
    private InputField yCoord;
    [SerializeField]
    private Toggle randomLocation;
    [SerializeField]
    private InputField shapeIdInput;

    private void Awake()
    {
        monsterMaker = FindObjectOfType<MonsterMaker>();   
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        configureRandomLocation();
        configureCoords();
        configureGeonicId();
    }

    private void configureRandomLocation()
    {
        monsterMaker.RandomiseLocation = randomLocation.isOn;
        if (monsterMaker.RandomiseLocation)
        {
            setCoordEnableFlag(false);
        }
        else
        {
            setCoordEnableFlag(true);
        }
    }

    private void setCoordEnableFlag(bool isEnabled)
    {
        xCoord.enabled = isEnabled;
        yCoord.enabled = isEnabled;
    }

    private void configureCoords()
    {
        if (!randomLocation.isOn)
        {
            monsterMaker.XCoord = getIntFromInput(xCoord);
            monsterMaker.YCoord = getIntFromInput(yCoord);
        }
    }

    private void configureGeonicId()
    {
        int inputId = getIntFromInput(shapeIdInput);
        monsterMaker.SetGeonicPrefab(inputId);
    }

    private int getIntFromInput(InputField sourceInput)
    {
        int inputValue = 0;
        if (sourceInput.text != "")
        {
            inputValue = int.Parse(sourceInput.text);
        }
        return inputValue;
    }
}
