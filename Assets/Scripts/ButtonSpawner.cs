using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject hairButtonPreFab;
    public GameObject colorButtonPreFab;

    private readonly Vector2 _originPanel = new(-245, 0);
    private readonly Vector2 _origin = new(-220, 300);
    private const float VerticalSpacing = 150;
    private const float HorizontalSpacing = 70;
    
    [HideInInspector] public InspectorEntry.ButtonSpawnerData[] data = { };
    
    private Controls _controls;
    private InspectorEntry _inspectorEntry; 

    public void SpawnButtons()
    {
        _controls = GameObject.Find("Controls").GetComponent<Controls>();
        _inspectorEntry = GetComponent<InspectorEntry>();

        data = new InspectorEntry.ButtonSpawnerData[_inspectorEntry.ButtonSetUps.Length];
        for (int i = 0; i < _inspectorEntry.ButtonSetUps.Length; i++)
        {
            data[i] = _inspectorEntry.ButtonSetUps[i].ButtonSpawnerDatas;
        }
        
        CreateAllButtons();
    }

    private void CreateAllButtons()
    {
        for (var i = 0; i < data.Length; i++)
        {
            MakeColorButtons(i, data[i].color, data[i].indexName, data[i].subTabName, 
                data[i].indexName);
            _controls.buttonLinks[i].icons = MakeSelectButtons(i, _controls.buttonLinks[i].icons.Length, data[i].indexName, 
                data[i].subTabName, data[i].indexName);
        }
    }

    private void MakeColorButtons(int colorButtonParentIndex, Color[] colorArray, string logMessage, string subTabName,
        string indexName)
    {
        for (var i = 0; i < colorArray.Length; i++)
        {
            SpawnColorButton(colorButtonParentIndex, colorArray, logMessage + " Tint Changed Index: ",
                subTabName, indexName, i);
        }
    }

    /// <summary>
    /// This will create all of the buttons given the amount of colors provided
    /// </summary>
    /// <param name="colorButtonParentIndex"></param> What object will be it's parent 
    /// <param name="length"></param> How many buttons will we create 
    /// <param name="logMessage"></param> What will the debug message hold 
    /// <param name="subTabName"></param> What tab it's connected to 
    /// <param name="indexName"></param> What index keeps track of it's position 
    /// <returns></returns>
    private Image[] MakeSelectButtons(int colorButtonParentIndex, int length, string logMessage, string subTabName, string indexName)
    {
        var array = new Image[length];
        for (var i = 0; i < length; i++)
        {
            array[i] = SpawnSelectButton(colorButtonParentIndex, logMessage, subTabName, indexName, i);
        }

        return array;
    }

    //==================================================================================================================
    // Button Spawning 
    //==================================================================================================================

    /// <summary>
    /// This will create button that will allow you to switch the type of item the person has
    /// </summary>
    /// <param name="buttonParentIndex"></param> Which transform should we create the buttons under 
    /// <param name="logMessage"></param> Message that will be shown in the Debug.Log
    /// <param name="subTabName"></param> Which sub tab should be update 
    /// <param name="indexName"></param> which 
    /// <param name="i"></param> index of currently created button 
    /// <returns></returns>
    private Image SpawnSelectButton(int buttonParentIndex, string logMessage, string subTabName, string indexName, int i)
    {
        // Instantiate the button prefab
        var newButton = Instantiate(hairButtonPreFab, transform);
        
        //Links the button to be parented and placed in correct spot on board, the false keeps it to scale properly  
        newButton.transform.SetParent(data[buttonParentIndex].buttonParentObject, false);
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(_origin.x + VerticalSpacing * (i%4), _origin.y - VerticalSpacing * ((i/4)%4));

        // Connect the On Click () event to the buttonScript component's OnButtonClick() function
        newButton.GetComponent<Button>().onClick.AddListener(() => _controls.UpdateType(buttonParentIndex,
            logMessage, subTabName, indexName, i));
        
        return newButton.transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }
    
    /// <summary>
    /// This will create buttons on the top of the screen given that there are any colors provided. Those buttons
    /// then will be attached and will use the UpdateColor() method from Controls. 
    /// </summary>
    /// <param name="colorButtonParentIndex"></param> tells us which transform to refer to when spawning the buttons 
    /// <param name="colorArray"></param> tells us what colors assign to the button image 
    /// <param name="logMessage"></param> what message should Debug.Log show
    /// <param name="subTabName"></param> which tab should we refer to when update the game state 
    /// <param name="indexName"></param> which button should we refer to when updating the game state 
    /// <param name="i"></param> index of currently created button 
    private void SpawnColorButton(int colorButtonParentIndex, Color[] colorArray, string logMessage, string subTabName, string indexName, int i)
    {
        // Instantiate the button prefab
        var newButton = Instantiate(colorButtonPreFab, transform);
        
        //Links the button to be parented and placed in correct spot on board, the false keeps it to scale properly 
        newButton.transform.SetParent(data[colorButtonParentIndex].colorButtonParentObject, false);
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(_originPanel.x + HorizontalSpacing * i, _originPanel.y);
        
        //Grabs the icon 
        newButton.transform.GetChild(0).GetComponent<Image>().color = colorArray[i];

        // Connect the On Click () event to the buttonScript component's OnButtonClick() function
        newButton.GetComponent<Button>().onClick.AddListener(() => _controls.UpdateColor(colorButtonParentIndex, 
            logMessage, subTabName, indexName, i));
    }

}
