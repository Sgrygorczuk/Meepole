using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// This script is the central script that updates the all of the visual elements, grabs data and pass it on to installation 
/// </summary>
public class Controls : MonoBehaviour
{
    //Initial way of connecting different body parts got replaced a bit but some core functions need it without being reworked 
    [Tooltip("0 = Head,\n 1 = Neck,\n 2 = L_Arm,\n 3 = L_Hand,\n 4 = R_Arm,\n 5 = R_Hand,\n 6 = L_Leg,\n" +
             "7 = R_Leg,\n 8 = L_Sleeve,\n 9 = R_Sleeve,,\n 10 = L_Pants,\n 11 = R_Pants")]
    public SpriteRenderer[] bodyPart = new SpriteRenderer[12];
    
    [Tooltip("0 = Hair,\n 1 = Nose,\n 2 = L_Eyes,\n 3 = R_Eye,\n 4 = Mouth, \n 5 = L_EyeBrow,\n 6 = R_EyeBrow,")]
    [HideInInspector] public SpriteRenderer[] facePart = new SpriteRenderer[7];
    
    /// <summary>
    /// Index that are used to keep track of stuff, should have been dynamically created with each new data set 
    /// </summary>
    private readonly Dictionary<string, int> FaceBigTabKeys = new Dictionary<string, int>
    {   
        {"noseTypeIndex", 0},
        {"noseColorIndex", 0},
        {"hairTypeIndex", 0},
        {"hairColorIndex", 0},
        {"eyeBrowTypeIndex", 0},
        {"eyeBrowColorIndex", 0},
        {"eyeTypeIndex", 0},
        {"eyeColorIndex", 0},
        {"mouthTypeIndex", 0},
        {"mouthColorIndex", 0},
        {"shirtTypeIndex", 0},
        {"shirtColorIndex", 0},
        {"sleeveTypeIndex", 0},
        {"sleeveColorIndex", 0},
        {"beltTypeIndex", 0},
        {"beltColorIndex", 0},
        {"pantsTypeIndex", 0},
        {"pantsColorIndex", 0},
        {"shoesTypeIndex", 0},
        {"shoesColorIndex", 0},
    };
    
    
    [HideInInspector] public InspectorEntry.ButtonLinks[] buttonLinks = { };

    private Vector2 _hairOrigin = Vector2.zero;
    private const int PixelPerUnit = 200;

    [HideInInspector] public Vector2[] hairCoordinates = new Vector2[14];
    
    //Placement of the sleeve sprites and the adjustments need for each type 
    private Vector2 _leftSleeveOrigin = Vector2.zero;
    private Vector2 _rightSleeveOrigin = Vector2.zero;
    [HideInInspector] public Vector2[] leftSleeveCoordinates = new Vector2[3];
    [HideInInspector] public Vector2[] rightSleeveCoordinates = new Vector2[3];
    
    //Placement of the legs sprites and the adjustments need for each type 
    private Vector2 _leftPantsOrigin = Vector2.zero;
    private Vector2 _rightPantsOrigin = Vector2.zero;
    [HideInInspector] public Vector2[] leftPantsCoordinates = new Vector2[3];
    [HideInInspector] public Vector2[] rightPantsCoordinates = new Vector2[3];
    
    //Where we hold reference to the skin data that's formatted differently 
    [HideInInspector] public SpriteData[] armData = new SpriteData[8];
    [HideInInspector] public SpriteData[] handData = new SpriteData[8];
    [HideInInspector] public SpriteData[] legData = new SpriteData[8];
    [HideInInspector]  public SpriteData[] headData = new SpriteData[8];
    [HideInInspector] public SpriteData[] neckData = new SpriteData[8];

    //External compoentss 
    private ButtonSpawner _buttonSpawner;
    private SpawnData _spawnData;
    private InspectorEntry _inspectorEntry; 

    // Start is called before the first frame update
    private void Start()
    {
        //Connects to external components 
        _buttonSpawner = GetComponent<ButtonSpawner>();
        _spawnData = GetComponent<SpawnData>();
        _inspectorEntry = GetComponent<InspectorEntry>();

        //Collects data from the desingers input 
        buttonLinks = new InspectorEntry.ButtonLinks[_inspectorEntry.ButtonSetUps.Length];
        for (var i = 0; i < _inspectorEntry.ButtonSetUps.Length; i++)
        {
            buttonLinks[i] = _inspectorEntry.ButtonSetUps[i].ControlsData;
        }

        //Generates all of the data from game assets 
        _spawnData.StartSpawnData();

        //Sets Up Sprite Data and Icons for the different body parts 
        foreach (var t in buttonLinks) { t.spriteData = SetUpTintData(t.spriteData.Length, t.spriteDataPath); }
        
        //Connects the skin body parts data 
        armData = SetUpTintData(1, "ArmTintData");
        handData = SetUpTintData(1, "HandTintData");
        legData = SetUpTintData(1, "LegTintData");
        headData = SetUpTintData(1, "HeadTintData");
        neckData = SetUpTintData(1, "NeckTintData");
        
        //Sets up potential moves for differently sized sprites 
        SetUpHairCoordinates();
        SetUpExtraCoordinates();
        
        //Spawns all of the buttons 
        _buttonSpawner.SpawnButtons();
        
        //Runs each button once to set up visuals 
        SetUpButtons();
    }

    //==================================================================================================================
    // Set Up Data & UI Methods 
    //==================================================================================================================

    /// <summary>
    /// Looks for game objects that hold sprite references and brings them to this script for use 
    /// </summary>
    /// <param name="size"></param> Tells us how many sets of sprites there are a
    /// <param name="path"></param> Where the sprite should would in the hierarchy 
    /// <returns></returns>
    private static SpriteData[] SetUpTintData(int size, string path)
    {
        Debug.Log("Path: " + path + " with size of: " + size);
        var data = new SpriteData[size];
        var dataObject = GameObject.Find(path).gameObject;
        for (var i = 0; i < size; i++)
        {
            data[i] = dataObject.transform.GetChild(i).GetComponent<SpriteData>();
        }
        return data;
    }
    
    /// <summary>
    /// Goes through all of the different hair sprites and gets the unit adjustments based on the different sizes of
    /// the sprites necessary to keep the game object in the same place even if the sprite changes. 
    /// </summary>
    private void SetUpHairCoordinates()
    {
        _hairOrigin = facePart[0].gameObject.transform.position;
        var baseWidth =  Resources.Load<Sprite>("Sprites/Hair/tint1Hair1").textureRect.width;
        var baseHeight =  Resources.Load<Sprite>("Sprites/Hair/tint1Hair1").textureRect.height;
        for (var i = 0; i < hairCoordinates.Length; i++){
            hairCoordinates[i][0] =  (Resources.Load<Sprite>("Sprites/Hair/tint1Hair" + (i+1)).textureRect.width - baseWidth)/PixelPerUnit;
            hairCoordinates[i][1] =  (Resources.Load<Sprite>("Sprites/Hair/tint1Hair" + (i+1)).textureRect.height - baseHeight)/PixelPerUnit;
        }
    }

    /// <summary>
    /// Connects the sprite that will be moved, sleeves and pants, to the adjustment needed for the different sizes of sprites
    /// to keep them in place. 
    /// </summary>
    private void SetUpExtraCoordinates()
    {
        _leftSleeveOrigin = bodyPart[8].gameObject.transform.position;
        _rightSleeveOrigin = bodyPart[9].gameObject.transform.position;
        _leftPantsOrigin = bodyPart[10].gameObject.transform.position;
        _rightPantsOrigin = bodyPart[11].gameObject.transform.position;
    }

    /// <summary>
    /// Used to run all of the buttons once's so that the UI updates, but also used as the randomizer for the characters 
    /// </summary>
    public void SetUpButtons()
    {
        UpdateType(0, "Hair Type Changed Index: ", "hair", Random.Range(0, buttonLinks[0].icons.Length));
        UpdateType(1,"Eye Brow Tint Changed Index: ", "eyeBrow", Random.Range(0, buttonLinks[1].icons.Length));
        UpdateType(2, "Eye Tint Changed Index: ", "eye", Random.Range(0, buttonLinks[2].icons.Length));
        UpdateType(3, "Skin Tint Changed Index: ", "nose", Random.Range(0, buttonLinks[3].icons.Length));
        UpdateType(4, "Mouth Tint Changed Index: ", "mouth", Random.Range(0, buttonLinks[4].icons.Length));
        UpdateType(5, "Shirt Tint Changed Index: ", "shirt", Random.Range(0, buttonLinks[5].icons.Length));
        UpdateType(6, "Sleeve Tint Changed Index: ", "sleeve", Random.Range(0, buttonLinks[6].icons.Length));
        UpdateType(7, "Belt Tint Changed Index: ", "belt", Random.Range(0, buttonLinks[7].icons.Length));
        UpdateType(8, "Pants Tint Changed Index: ", "pants", Random.Range(0, buttonLinks[8].icons.Length));
        UpdateType(9, "Shoes Tint Changed Index: ", "shoes", Random.Range(0, buttonLinks[9].icons.Length));
        
        UpdateColor(0, "Hair Color Changed Index: ", "hair", Random.Range(0, buttonLinks[0].spriteData.Length));
        UpdateColor(1,"Eye Brow Color Changed Index: ", "eyeBrow", Random.Range(0, buttonLinks[1].spriteData.Length));
        UpdateColor(2,"Eye Color Changed Index: ", "eye", Random.Range(0, buttonLinks[2].spriteData.Length));
        UpdateColor(3, "Skin Color Changed Index: ", "nose", Random.Range(0, buttonLinks[3].spriteData.Length));
        UpdateColor(4, "Mouth Color Changed Index: ", "mouth", Random.Range(0, buttonLinks[4].spriteData.Length));
        UpdateColor(5, "Shirt Color Changed Index: ", "shirt", Random.Range(0, buttonLinks[5].spriteData.Length));
        UpdateColor(6, "Sleeve Color Changed Index: ", "sleeve", Random.Range(0, buttonLinks[6].spriteData.Length));
        UpdateColor(7, "Belt Color Changed Index: ", "belt", Random.Range(0, buttonLinks[7].spriteData.Length));
        UpdateColor(8, "Pants Color Changed Index: ", "pants", Random.Range(0, buttonLinks[8].spriteData.Length));
        UpdateColor(9, "Shoes Color Changed Index: ", "shoes", Random.Range(0, buttonLinks[9].spriteData.Length));
        
    }
    
    //==================================================================================================================
    // General Methods 
    //==================================================================================================================

    /// <summary>
    /// Goes through all of the buttons, activates all of them and then disables the one that was clicked on last
    /// </summary>
    /// <param name="parent"></param> Parent that holds all of the buttons
    /// <param name="index"></param> Index of the button that will be turned off 
    private static void ChangeButtonActive(Component parent, int index)
    {
        for (var i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
        parent.transform.GetChild(index).GetComponent<Button>().interactable = false;
    }
    
    //==================================================================================================================
    // Color Methods 
    //==================================================================================================================
    
    /// <summary>
    /// General Update color function, updates index of chosen color, changes the color of sprite on the model and in the UI,
    /// and updates button clickbaitlity   
    /// </summary>
    /// <param name="parentIndex"></param> What is the parent that this button belongs to 
    /// <param name="logMessage"></param> What will pop up in the Debug
    /// <param name="indexName"></param> The name that the index is connected to 
    /// <param name="i"></param> The index of the sprite in respect to indexName
    public void UpdateColor(int parentIndex, string logMessage, string indexName, int i)
    {
        Debug.Log(logMessage + i);
        FaceBigTabKeys[indexName + "ColorIndex"]  = i;
        UpdateColorSwitch(indexName, parentIndex);
        ChangeButtonActive(_inspectorEntry.ButtonSetUps[parentIndex].ButtonSpawnerDatas.colorButtonParentObject, FaceBigTabKeys[indexName + "ColorIndex"]);
    }

    /// <summary>
    /// Actually changes the color of the items 
    /// </summary>
    /// <param name="indexName"></param>
    /// <param name="parentIndex"></param>
    private void UpdateColorSwitch(string indexName, int parentIndex)
    {
        //Specific case for the nose, changes the skin color as well for other body parts 
        if(indexName ==  "nose"){ ChangeSkinColorOnModel(); }
        
        //Updates the color on the model, updates the color choice on the model 
        ChangePartModel(indexName + "ColorIndex", indexName + "TypeIndex", parentIndex,
            buttonLinks[parentIndex].canBeEmpty ? 1 : 0);
        
        //Updates the color choice in the UI 
        if (buttonLinks[parentIndex].canBeEmpty)
        {
            buttonLinks[parentIndex].icons[0].enabled = false;
            buttonLinks[parentIndex].icons = ChangeColorMenu(buttonLinks[parentIndex].icons, buttonLinks[parentIndex].spriteData, indexName + "ColorIndex", 1);   
        }
        else
        {
            buttonLinks[parentIndex].icons = ChangeColorMenu(buttonLinks[parentIndex].icons, buttonLinks[parentIndex].spriteData, indexName + "ColorIndex", 0);
        }
    }
    
    /// <summary>
    /// Goes through all of the sprites and changes them to show it with the newly selected color 
    /// </summary>
    /// <param name="icons"></param> Array of all of the images affected by the change 
    /// <param name="data"></param> Array of all of the spites
    /// <param name="path"></param> Index
    /// <param name="start"></param> if we start at 0 or 1, depending on if we want an empty space 
    /// <returns></returns>
    private Image[] ChangeColorMenu(Image[] icons, IReadOnlyList<SpriteData> data, string path, int start)
    {
        //Updates all the other hair to the new color 
        for (var i = start; i < icons.Length; i++)
        {
            icons[i].sprite =  data[FaceBigTabKeys[path]].spriteData[i - start];
        }
        return icons;
    }
    
    
    /// <summary>
    /// Changes the head, neck, arms, hands and legs to the new color chosen by the nose 
    /// </summary>
    private void ChangeSkinColorOnModel()
    {
        //Head Update 
        bodyPart[0].sprite = headData[0].spriteData[FaceBigTabKeys["noseColorIndex"]];
        
        //Neck Update 
        bodyPart[1].sprite = neckData[0].spriteData[FaceBigTabKeys["noseColorIndex"]];
        //Arms 
        bodyPart[2].sprite = armData[0].spriteData[FaceBigTabKeys["noseColorIndex"]];
        bodyPart[4].sprite = armData[0].spriteData[FaceBigTabKeys["noseColorIndex"]];
        //Hands
        bodyPart[3].sprite = handData[0].spriteData[FaceBigTabKeys["noseColorIndex"]];
        bodyPart[5].sprite = handData[0].spriteData[FaceBigTabKeys["noseColorIndex"]];
        //Legs 
        bodyPart[6].sprite = legData[0].spriteData[FaceBigTabKeys["noseColorIndex"]];
        bodyPart[7].sprite = legData[0].spriteData[FaceBigTabKeys["noseColorIndex"]];
    }
    
    //==================================================================================================================
    // Type Methods 
    //==================================================================================================================
    
    /// <summary>
    /// General Update type function, updates index of chosen type, changes the sprite to a different type on the model,
    /// and updates button clickbaitlity   
    /// </summary>
    /// <param name="parentIndex"></param> What is the parent that this button belongs to 
    /// <param name="logMessage"></param> What will pop up in the Debug
    /// <param name="indexName"></param> The name that the index is connected to 
    /// <param name="i"></param> The index of the sprite in respect to indexName
    public void UpdateType(int parentIndex, string logMessage, string indexName, int i)
    {
        Debug.Log(logMessage + i);
        FaceBigTabKeys[indexName + "TypeIndex"] = i;
        UpdateTypeSwitch(parentIndex, indexName);
        ChangeButtonActive(_inspectorEntry.ButtonSetUps[parentIndex].ButtonSpawnerDatas.buttonParentObject, FaceBigTabKeys[indexName + "TypeIndex"]);
    }
    
    /// <summary>
    /// Actually changes the type 
    /// </summary>
    /// <param name="parentIndex"></param> 
    /// <param name="indexName"></param>
    private void UpdateTypeSwitch(int parentIndex, string indexName)
    {
        //Updates coordinates in three cases 
        switch (indexName)
        {
            case "hair":
            {
                ChangeHairCoordinates();
                break;
            }
            case "sleeve":
            {
                ChangeSleeveCoordinates();
                break;
            }
            case "pants":
            {
                ChangePantsCoordinates();
                break;
            }
        }

        //Updates the color on the model, updates the color choice on the model 
        ChangePartModel(indexName + "ColorIndex", indexName + "TypeIndex", parentIndex,
            buttonLinks[parentIndex].canBeEmpty ? 1 : 0);
    }
    
    //==================================================================================================================
    // Coordinates Methods 
    //==================================================================================================================
    
    /// <summary>
    /// Updates the coordinates of the hair on the model to account for the different sprite sizes  
    /// </summary>
    private void ChangeHairCoordinates()
    {
        switch (FaceBigTabKeys["hairTypeIndex"] )
        {
            //If Bold Don't Adjust 
            case 0:
                return;
            //If Short Hair Adjust by adding 
            case < 9:
                facePart[0].transform.position = _hairOrigin + hairCoordinates[FaceBigTabKeys["hairTypeIndex"]  - 1];
                break;
            //If Long hair adjust x by adding and y by subbing 
            default:
                facePart[0].transform.position = new Vector3(_hairOrigin.x,
                    _hairOrigin.y - hairCoordinates[FaceBigTabKeys["hairTypeIndex"]  - 1].y, 0);
                break;
        }
    }
    
    /// <summary>
    /// Adjust the placment of the sleeve based on the chosen type 
    /// </summary>
    private void ChangeSleeveCoordinates()
    {
        switch (FaceBigTabKeys["sleeveTypeIndex"] )
        {
            case 0:
                return;
            default:
                bodyPart[8].transform.position = new Vector3(_leftSleeveOrigin.x + leftSleeveCoordinates[FaceBigTabKeys["sleeveTypeIndex"]  - 1].x,
                    _leftSleeveOrigin.y + leftSleeveCoordinates[FaceBigTabKeys["sleeveTypeIndex"]  - 1].y, 0);
                bodyPart[9].transform.position = new Vector3(_rightSleeveOrigin.x + rightSleeveCoordinates[FaceBigTabKeys["sleeveTypeIndex"]  - 1].x,
                    _rightSleeveOrigin.y + rightSleeveCoordinates[FaceBigTabKeys["sleeveTypeIndex"]  - 1].y, 0);
                break;
        }
    }
    
    /// <summary>
    /// Adjust the placment of the pants based on the chosen type 
    /// </summary>
    private void ChangePantsCoordinates()
    {
        switch (FaceBigTabKeys["pantsTypeIndex"] )
        {
            case 0:
                return;
            default:
                bodyPart[10].transform.position = new Vector3(_leftPantsOrigin.x + leftPantsCoordinates[FaceBigTabKeys["pantsTypeIndex"]  - 1].x,
                    _leftPantsOrigin.y + leftPantsCoordinates[FaceBigTabKeys["pantsTypeIndex"]  - 1].y, 0);
                bodyPart[11].transform.position = new Vector3(_rightPantsOrigin.x + rightPantsCoordinates[FaceBigTabKeys["pantsTypeIndex"]  - 1].x,
                    _rightPantsOrigin.y + rightPantsCoordinates[FaceBigTabKeys["pantsTypeIndex"]  - 1].y, 0);
                break;
        }
    }

    

    //==================================================================================================================
    // Model Part Change Methods 
    //==================================================================================================================

    /// <summary>
    /// Changes the chosen sprite 
    /// </summary>
    /// <param name="colorIndex"></param> Looks at the currently chosen color
    /// <param name="typeIndex"></param> Looks at the new index type 
    /// <param name="parentIndex"></param> Looks at which body parts will be done 
    /// <param name="start"></param> Does it start at 0 or 1, determine if it has an empty space 
    private void ChangePartModel(string colorIndex, string typeIndex, int parentIndex, int start)
    {
        //Checks if the given part(s) is set to 0, if so turn off the Sprite Renderer(s)
        if (FaceBigTabKeys[typeIndex] == 0 && start == 1)
        {
            foreach (var part in buttonLinks[parentIndex].spriteBodyPart)
            {
                part.enabled = false;
            }
        }
        //If not go through the part(s) and turn them on, while updating them to the new type and color 
        else
        {
            foreach (var part in buttonLinks[parentIndex].spriteBodyPart)
            {
                part.enabled = true;
                part.sprite = buttonLinks[parentIndex].spriteData[FaceBigTabKeys[colorIndex]].spriteData[FaceBigTabKeys[typeIndex] - start];   
            }
        }
    }

}
