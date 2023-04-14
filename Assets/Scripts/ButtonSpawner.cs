using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour
{
    public Transform[] buttonParents;
    public Transform[] colorButtonParent;
    public GameObject hairButtonPreFab;
    public GameObject colorButtonPreFab;

    private readonly Vector2 _originPanel = new(-245, 0);
    private readonly Vector2 _origin = new(-220, 300);
    private Controls _controls;
    private const float VerticalSpacing = 150;
    private const float HorizontalSpacing = 70;

    public Color[] hairColor = new Color[8];
    public Color[] eyebrowColor = new Color[8];
    public Color[] eyeColor = new Color[5];
    public Color[] noseColor = new Color[8];

    public void SpawnButtons()
    {
        SetUpParentArrays();
        
        _controls = GameObject.Find("Controls").GetComponent<Controls>();

        _controls.hairIcons = MakeSelectButtons(0, _controls.hairIcons.Length, "Hair Type Changed Index: ",
            "faceIndex", "hairTypeIndex");

        _controls.eyeBrowIcons = MakeSelectButtons(1, _controls.eyeBrowIcons.Length, "Eye Brow Tint Changed Index: ",
            "faceIndex", "eyeBrowTypeIndex");
        _controls.eyeIcons = MakeSelectButtons(2, _controls.eyeIcons.Length, "Eye Tint Changed Index: ",
            "faceIndex", "eyeTypeIndex");
        _controls.noseIcons = MakeSelectButtons(3, _controls.noseIcons.Length, "Nose Tint Changed Index: ",
            "faceIndex", "noseTypeIndex");
        

        MakeColorButtons(0, hairColor, "Hair Color Changed Index: ",
            "faceIndex", "hairColorIndex");
        MakeColorButtons(1, eyebrowColor, "Eye Brow Color Changed Index: ",
            "faceIndex", "eyeBrowColorIndex");
        MakeColorButtons(2, eyeColor, "Eye Color Changed Index: ",
            "faceIndex", "eyeColorIndex");
        MakeColorButtons(3, noseColor, "Nose Color Changed Index: ",
            "faceIndex", "noseColorIndex");
    }

    private void SetUpParentArrays()
    {
        var tabParent = GameObject.Find("Canvas").transform.Find("Big_Tabs").transform.Find("Face_Tabs").gameObject;
        var childCount = tabParent.transform.childCount;
        buttonParents = new Transform[childCount];
        colorButtonParent = new Transform[childCount];
 
        for (var i = 0; i < tabParent.transform.childCount; i++)
        {
            buttonParents[i] = tabParent.transform.GetChild(i).transform.GetChild(0).transform;
            colorButtonParent[i] = tabParent.transform.GetChild(i).transform.GetChild(1).transform;
        }
    }

    private void MakeColorButtons(int colorButtonParentIndex, Color[] colorArray, string logMessage, string subTabName,
        string indexName)
    {
        for (var i = 0; i < colorArray.Length; i++)
        {
            SpawnColorButton(colorButtonParentIndex, colorArray, logMessage,
                subTabName, indexName, i);
        }
    }

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
        
        //Links the button to be parented and placed in correct spot on board  
        newButton.transform.SetParent(buttonParents[buttonParentIndex]);
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(_origin.x + VerticalSpacing * (i%4), _origin.y - VerticalSpacing * ((i/4)%4));

        // Connect the On Click () event to the buttonScript component's OnButtonClick() function
        newButton.GetComponent<Button>().onClick.AddListener(() => _controls.UpdateType(
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
        
        //Links the button to be parented and placed in correct spot on board  
        newButton.transform.SetParent(colorButtonParent[colorButtonParentIndex]);
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(_originPanel.x + HorizontalSpacing * i, _originPanel.y);
        
        //Grabs the icon 
        newButton.transform.GetChild(0).GetComponent<Image>().color = colorArray[i];

        // Connect the On Click () event to the buttonScript component's OnButtonClick() function
        newButton.GetComponent<Button>().onClick.AddListener(() => _controls.UpdateColor(
            logMessage, subTabName, indexName, i));
    }

}
