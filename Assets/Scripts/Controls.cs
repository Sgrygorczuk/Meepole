using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Controls : MonoBehaviour
{
    
    [Tooltip("0 = Head,\n 1 = Neck,\n 2 = L_Arm,\n 3 = L_Hand,\n 4 = R_Arm,\n 5 = R_Hand,\n 6 = L_Leg,\n" +
             "7 = R_Leg,\n 8 = L_Sleeve,\n 9 = R_Sleeve,,\n 10 = L_Pants,\n 11 = R_Pants")]
    public SpriteRenderer[] bodyPart = new SpriteRenderer[12];
    
    [Tooltip("0 = Hair,\n 1 = Nose,\n 2 = L_Eyes,\n 3 = R_Eye,\n 4 = Mouth, \n 5 = L_EyeBrow,\n 6 = R_EyeBrow,")]
    [HideInInspector] public SpriteRenderer[] facePart = new SpriteRenderer[7];
    
    
    private readonly Dictionary<string, int> _subTabKeys = new Dictionary<string, int>
    {   
        {"faceIndex", 0},
        {"bodyIndex", 0},
        {"bottomIndex", 0}
    };

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
    
    private Vector2 _leftSleeveOrigin = Vector2.zero;
    private Vector2 _rightSleeveOrigin = Vector2.zero;
    [HideInInspector] public Vector2[] leftSleeveCoordinates = new Vector2[3];
    [HideInInspector] public Vector2[] rightSleeveCoordinates = new Vector2[3];
    
    private Vector2 _leftPantsOrigin = Vector2.zero;
    private Vector2 _rightPantsOrigin = Vector2.zero;
    [HideInInspector] public Vector2[] leftPantsCoordinates = new Vector2[3];
    [HideInInspector] public Vector2[] rightPantsCoordinates = new Vector2[3];
    
    [HideInInspector] public SpriteData[] armData = new SpriteData[8];
    [HideInInspector] public SpriteData[] handData = new SpriteData[8];
    [HideInInspector] public SpriteData[] legData = new SpriteData[8];
    [HideInInspector]  public SpriteData[] headData = new SpriteData[8];
    [HideInInspector] public SpriteData[] neckData = new SpriteData[8];

    private ButtonSpawner _buttonSpawner;
    private SpawnData _spawnData;
    private InspectorEntry _inspectorEntry; 

    // Start is called before the first frame update
    void Start()
    {
        _buttonSpawner = GetComponent<ButtonSpawner>();
        _spawnData = GetComponent<SpawnData>();
        
        _inspectorEntry = GetComponent<InspectorEntry>();

        buttonLinks = new InspectorEntry.ButtonLinks[_inspectorEntry.ButtonSetUps.Length];
        for (var i = 0; i < _inspectorEntry.ButtonSetUps.Length; i++)
        {
            buttonLinks[i] = _inspectorEntry.ButtonSetUps[i].ControlsData;
        }

        
        _spawnData.StartSpawnData();

        //Sets Up Sprite Data and Icons for the different body parts 
        foreach (var t in buttonLinks)
        {
            t.spriteData = SetUpTintData(t.spriteData.Length, t.spriteDataPath);
        }
        
        armData = SetUpTintData(1, "ArmTintData");
        handData = SetUpTintData(1, "HandTintData");
        legData = SetUpTintData(1, "LegTintData");
        headData = SetUpTintData(1, "HeadTintData");
        neckData = SetUpTintData(1, "NeckTintData");
        SetUpHairCoordinates();
        SetUpExtraCoordinates();
        
        _buttonSpawner.SpawnButtons();
        
        SetUpButtons();
    }

    //==================================================================================================================
    // Set Up Data & UI Methods 
    //==================================================================================================================

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

    private void SetUpExtraCoordinates()
    {
        _leftSleeveOrigin = bodyPart[8].gameObject.transform.position;
        _rightSleeveOrigin = bodyPart[9].gameObject.transform.position;
        _leftPantsOrigin = bodyPart[10].gameObject.transform.position;
        _rightPantsOrigin = bodyPart[11].gameObject.transform.position;
    }

    public void SetUpButtons()
    {
        UpdateType(0, "Hair Type Changed Index: ", "faceIndex", "hair", Random.Range(0, buttonLinks[0].icons.Length));
        UpdateType(1,"Eye Brow Tint Changed Index: ", "faceIndex", "eyeBrow", Random.Range(0, buttonLinks[1].icons.Length));
        UpdateType(2, "Eye Tint Changed Index: ", "faceIndex", "eye", Random.Range(1, buttonLinks[2].icons.Length));
        UpdateType(3, "Skin Tint Changed Index: ", "faceIndex", "nose", Random.Range(0, buttonLinks[3].icons.Length));
        UpdateType(4, "Mouth Tint Changed Index: ", "faceIndex", "mouth", Random.Range(0, buttonLinks[4].icons.Length));
        UpdateType(5, "Shirt Tint Changed Index: ", "bodyIndex", "shirt", Random.Range(1, buttonLinks[5].icons.Length));
        UpdateType(6, "Sleeve Tint Changed Index: ", "bodyIndex", "sleeve", Random.Range(0, buttonLinks[6].icons.Length));
        UpdateType(7, "Belt Tint Changed Index: ", "bottomIndex", "belt", Random.Range(1, buttonLinks[7].icons.Length));
        UpdateType(8, "Pants Tint Changed Index: ", "bottomIndex", "pants", Random.Range(0, buttonLinks[8].icons.Length));
        UpdateType(9, "Shoes Tint Changed Index: ", "bottomIndex", "shoes", Random.Range(1, buttonLinks[9].icons.Length));
        
        UpdateColor(0, "Hair Color Changed Index: ", "faceIndex", "hair", Random.Range(0, buttonLinks[0].spriteData.Length));
        UpdateColor(1,"Eye Brow Color Changed Index: ", "faceIndex", "eyeBrow", Random.Range(0, buttonLinks[1].spriteData.Length));
        UpdateColor(2,"Eye Color Changed Index: ", "faceIndex", "eye", Random.Range(0, buttonLinks[2].spriteData.Length));
        UpdateColor(3, "Skin Color Changed Index: ", "faceIndex", "nose", Random.Range(0, buttonLinks[3].spriteData.Length));
        UpdateColor(4, "Mouth Color Changed Index: ", "faceIndex", "mouth", Random.Range(0, buttonLinks[4].spriteData.Length));
        UpdateColor(5, "Shirt Color Changed Index: ", "bodyIndex", "shirt", Random.Range(0, buttonLinks[5].spriteData.Length));
        UpdateColor(6, "Sleeve Color Changed Index: ", "bodyIndex", "sleeve", Random.Range(0, buttonLinks[6].spriteData.Length));
        UpdateColor(7, "Belt Color Changed Index: ", "bottomIndex", "belt", Random.Range(0, buttonLinks[7].spriteData.Length));
        UpdateColor(8, "Pants Color Changed Index: ", "bottomIndex", "pants", Random.Range(0, buttonLinks[8].spriteData.Length));
        UpdateColor(9, "Shoes Color Changed Index: ", "bottomIndex", "shoes", Random.Range(0, buttonLinks[9].spriteData.Length));
        
    }
    
    //==================================================================================================================
    // General Methods 
    //==================================================================================================================

    private static void ChangeButtonActive(Component parent, int index)
    {
        for (var i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
        parent.transform.GetChild(index).GetComponent<Button>().interactable = false;
    }
    
    
    public void UpdateColor(int parentIndex, string logMessage, string subTabName, string indexName, int i)
    {
        Debug.Log(logMessage + i);
        FaceBigTabKeys[indexName + "ColorIndex"]  = i;
        UpdateColorSwitch(indexName, parentIndex);
        ChangeButtonActive(_inspectorEntry.ButtonSetUps[parentIndex].ButtonSpawnerDatas.colorButtonParentObject, FaceBigTabKeys[indexName + "ColorIndex"]);
    }

    private void UpdateColorSwitch(string indexName, int parentIndex)
    {
        switch (indexName)
        {
            case "nose":
            {
                ChangeSkinColorOnModel();
                break;
            }
        }
        
        ChangePartModel(indexName + "ColorIndex", indexName + "TypeIndex", parentIndex);
        
        if (buttonLinks[parentIndex].canBeEmpty)
        {
            buttonLinks[parentIndex].icons[0].enabled = false;
            buttonLinks[parentIndex].icons = ChangeColorMenu(buttonLinks[parentIndex].icons, buttonLinks[parentIndex].spriteData, indexName + "ColorIndex");   
        }
        else
        {
            buttonLinks[parentIndex].icons = ChangeColorMenuNoEmpty(buttonLinks[parentIndex].icons, buttonLinks[parentIndex].spriteData, indexName + "ColorIndex");
        }
    }
    
    private Image[] ChangeColorMenu(Image[] icons, SpriteData[] data, string path)
    {
        //Updates all the other hair to the new color 
        for (var i = 1; i < icons.Length; i++)
        {
            icons[i].sprite =  data[FaceBigTabKeys[path]].spriteData[i - 1];
        }
        return icons;
    }
    
    private Image[] ChangeColorMenuNoEmpty(Image[] icons, SpriteData[] data, string path)
    {
        //Updates all the other hair to the new color 
        for (var i = 0; i < icons.Length; i++)
        {
            icons[i].sprite =  data[FaceBigTabKeys[path]].spriteData[i];
        }
        return icons;
    }
    
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
    
    public void UpdateType(int parentIndex, string logMessage, string subTabName, string indexName, int i)
    {
        Debug.Log(logMessage + i);
        FaceBigTabKeys[indexName + "TypeIndex"] = i;
        UpdateTypeSwitch(parentIndex, indexName);
        ChangeButtonActive(_inspectorEntry.ButtonSetUps[parentIndex].ButtonSpawnerDatas.buttonParentObject, FaceBigTabKeys[indexName + "TypeIndex"]);
    }
    
    private void UpdateTypeSwitch(int parentIndex, string indexName)
    {
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

        if (buttonLinks[parentIndex].canBeEmpty)
        {
            ChangePartModel(indexName + "ColorIndex", indexName + "TypeIndex", parentIndex);   
        }
        else
        {
            ChangePartModelNoEmpty(indexName + "ColorIndex", indexName + "TypeIndex", parentIndex);   
        }

    }

    //==================================================================================================================
    // Hair Methods 
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

    private void ChangePartModel(string colorIndex, string typeIndex, int parentIndex)
    {
        //Checks if the given part(s) is set to 0, if so turn off the Sprite Renderer(s)
        if (FaceBigTabKeys[typeIndex] == 0)
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
                part.sprite = buttonLinks[parentIndex].spriteData[FaceBigTabKeys[colorIndex]].spriteData[FaceBigTabKeys[typeIndex] - 1];   
            }
        }
    }
    
    private void ChangePartModelNoEmpty(string colorIndex, string typeIndex, int parentIndex)
    {
        foreach (var part in buttonLinks[parentIndex].spriteBodyPart)
        {
            part.enabled = true;
            part.sprite = buttonLinks[parentIndex].spriteData[FaceBigTabKeys[colorIndex]]
                .spriteData[FaceBigTabKeys[typeIndex]];
        }
    }
}
