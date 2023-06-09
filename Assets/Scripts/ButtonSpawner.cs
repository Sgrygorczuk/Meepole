using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Script is used to create the buttons
/// </summary>
public class ButtonSpawner : MonoBehaviour
{
    // The Button PreFabs 
    public GameObject hairButtonPreFab;
    public GameObject colorButtonPreFab;
    
    // Location of the type and color prefabs
    private readonly Vector2 _originPanel = new(-245, 0);
    private readonly Vector2 _origin = new(-220, 300);
    //Spacing between buttons 
    private const float VerticalSpacing = 150;
    private const float HorizontalSpacing = 70;
    
    //Data holder 
    [HideInInspector] public InspectorEntry.ButtonSpawnerData[] data = { };
    
    //External components 
    private Controls _controls;
    private InspectorEntry _inspectorEntry;
    private PlaySFX _playSfx;

    /// <summary>
    /// Used in Controls to bring in the information and create all of the buttons used to switch type and color of sprite 
    /// </summary>
    public void SpawnButtons()
    {
        //Connects to external components 
        _controls = GameObject.Find("Controls").GetComponent<Controls>();
        _inspectorEntry = GetComponent<InspectorEntry>();
        _playSfx = GameObject.Find("ClickSFX").GetComponent<PlaySFX>();

        //Copies size of the array 
        data = new InspectorEntry.ButtonSpawnerData[_inspectorEntry.ButtonSetUps.Length];
        //Copies all the data from the inspector entry         
        for (var i = 0; i < _inspectorEntry.ButtonSetUps.Length; i++)
        {
            data[i] = _inspectorEntry.ButtonSetUps[i].ButtonSpawnerDatas;
        }
        
        //Creates all of the buttons 
        CreateAllButtons();
    }

    private void CreateAllButtons()
    {
        for (var i = 0; i < data.Length; i++)
        {
            MakeColorButtons(i, data[i].color, data[i].indexName, data[i].indexName);
            _controls.buttonLinks[i].icons = MakeSelectButtons(i, _controls.buttonLinks[i].icons.Length, data[i].indexName, data[i].indexName);
        }
    }

    /// <summary>
    /// Goes through each color button tab and creates the buttons for it 
    /// </summary>
    /// <param name="colorButtonParentIndex"></param> Where the buttons will be parented 
    /// <param name="colorArray"></param> How many buttons there will be 
    /// <param name="logMessage"></param> What is the message that will be sent to Debug
    /// <param name="indexName"></param> What is the index prefix that will keep track of the data 
    private void MakeColorButtons(int colorButtonParentIndex, Color[] colorArray, string logMessage,
        string indexName)
    {
        for (var i = 0; i < colorArray.Length; i++)
        {
            SpawnColorButton(colorButtonParentIndex, colorArray, logMessage + " Tint Changed Index: ", 
                indexName, i);
        }
    }

    /// <summary>
    /// This will create all of the buttons given the amount of colors provided
    /// </summary>
    /// <param name="colorButtonParentIndex"></param> What object will be it's parent 
    /// <param name="length"></param> How many buttons will we create 
    /// <param name="logMessage"></param> What will the debug message hold 
    /// <param name="indexName"></param> What index keeps track of it's position 
    /// <returns></returns>
    private Image[] MakeSelectButtons(int colorButtonParentIndex, int length, string logMessage, string indexName)
    {
        var array = new Image[length];
        for (var i = 0; i < length; i++)
        {
            array[i] = SpawnSelectButton(colorButtonParentIndex, logMessage, indexName, i);
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
    /// <param name="indexName"></param> which 
    /// <param name="i"></param> index of currently created button 
    /// <returns></returns>
    private Image SpawnSelectButton(int buttonParentIndex, string logMessage, string indexName, int i)
    {
        // Instantiate the button prefab
        var newButton = Instantiate(hairButtonPreFab, transform);
        
        //Links the button to be parented and placed in correct spot on board, the false keeps it to scale properly  
        newButton.transform.SetParent(data[buttonParentIndex].buttonParentObject, false);
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(_origin.x + VerticalSpacing * (i%4), _origin.y - VerticalSpacing * ((i/4)%4));

        // Connect the On Click () event to the buttonScript component's OnButtonClick() function
        newButton.GetComponent<Button>().onClick.AddListener(() => _controls.UpdateType(buttonParentIndex,
            logMessage, indexName, i));
        //Adds a Play SFX function that will make a noise when button is clicked 
        newButton.GetComponent<Button>().onClick.AddListener(() => _playSfx.PlaySfx());
        
        return newButton.transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }
    
    /// <summary>
    /// This will create buttons on the top of the screen given that there are any colors provided. Those buttons
    /// then will be attached and will use the UpdateColor() method from Controls. 
    /// </summary>
    /// <param name="colorButtonParentIndex"></param> tells us which transform to refer to when spawning the buttons 
    /// <param name="colorArray"></param> tells us what colors assign to the button image 
    /// <param name="logMessage"></param> what message should Debug.Log show
    /// <param name="indexName"></param> which button should we refer to when updating the game state 
    /// <param name="i"></param> index of currently created button 
    private void SpawnColorButton(int colorButtonParentIndex, Color[] colorArray, string logMessage, string indexName, int i)
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
            logMessage, indexName, i));
        //Adds a Play SFX function that will make a noise when button is clicked 
        newButton.GetComponent<Button>().onClick.AddListener(() => _playSfx.PlaySfx());
    }

}
