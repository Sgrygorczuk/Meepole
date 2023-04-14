using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    
    [Tooltip("0 = Head,\n 1 = Neck,\n 2 = L_Arm,\n 3 = L_Hand,\n 4 = R_Arm,\n 5 = R_Hand,\n 6 = L_Leg,\n" +
             "7 = R_Leg")]
    public SpriteRenderer[] bodyPart = new SpriteRenderer[8];
    
    [Tooltip("0 = Hair,\n 1 = Nose,\n 2 = L_Eyes,\n 3 = R_Eye,\n 4 = Mouth, \n 5 = L_EyeBrow,\n 6 = R_EyeBrow,")]
    public SpriteRenderer[] facePart = new SpriteRenderer[7];
    
    
    private readonly Dictionary<string, int> _subTabKeys = new Dictionary<string, int>
    {   
        {"faceIndex", 0},
        {"bodyIndex", 0},
        {"bottomIndex", 0}
    };

    private readonly Dictionary<string, int> _faceBigTabKeys = new Dictionary<string, int>
    {   
        {"noseTypeIndex", 0},
        {"noseColorIndex", 0},
        {"hairTypeIndex", 0},
        {"hairColorIndex", 0},
        {"eyeBrowTypeIndex", 0},
        {"eyeBrowColorIndex", 0},
        {"eyeTypeIndex", 0},
        {"eyeColorIndex", 0},
        {"mouthIndex", 0},
    };

    private Vector2 _hairOrigin = Vector2.zero;
    private const int PixelPerUnit = 200;

    public GameObject faceSubTabButtonsParent;

    public SpriteData[] hairData = new SpriteData[8];
    public Vector2[] hairCoordinates = new Vector2[14];
    public Image[] hairIcons = new Image[15];
    
    public SpriteData[] eyeBrowData = new SpriteData[8]; 
    public Image[] eyeBrowIcons = new Image[4];
    
    public SpriteData[] eyeData = new SpriteData[5]; 
    public Image[] eyeIcons = new Image[3];
    
    public SpriteData[] noseData = new SpriteData[8]; 
    public Image[] noseIcons = new Image[4];
    public SpriteData[] armData = new SpriteData[8];
    public SpriteData[] handData = new SpriteData[8];
    public SpriteData[] legData = new SpriteData[8];
    public SpriteData[] headData = new SpriteData[8];
    public SpriteData[] neckData = new SpriteData[8];

    private ButtonSpawner _buttonSpawner;
    private SpawnData _spawnData;

    // Start is called before the first frame update
    void Start()
    {
        _buttonSpawner = GetComponent<ButtonSpawner>();
        _spawnData = GetComponent<SpawnData>();
        
        _spawnData.StartSpawnData();

        hairData = SetUpTintData(hairData.Length, "HairTintData");
        eyeBrowData = SetUpTintData(eyeBrowData.Length, "EyeBrowTintData");
        eyeData = SetUpTintData(eyeData.Length, "EyeTintData");
        noseData = SetUpTintData(noseData.Length, "NoseTintData");
        armData = SetUpTintData(1, "ArmTintData");
        handData = SetUpTintData(1, "HandTintData");
        legData = SetUpTintData(1, "LegTintData");
        headData = SetUpTintData(1, "HeadTintData");
        neckData = SetUpTintData(1, "NeckTintData");
        SetUpHairCoordinates();
        
        _buttonSpawner.SpawnButtons();
        
        SetUpButtons();
        //_buttonSpawner.buttonParents[0].GetChild(0).GetComponent<Button>().onClick.Invoke();
    }

    //==================================================================================================================
    // Set Up Data & UI Methods 
    //==================================================================================================================

    private static SpriteData[] SetUpTintData(int size, string path)
    {
        Debug.Log("Path: " + path +" with size of: " + size);
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

    private void SetUpButtons()
    {
        UpdateType("Hair Type Changed Index: ", "faceIndex", "hairTypeIndex", 1);
        UpdateType("Eye Brow Tint Changed Index: ", "faceIndex", "eyeBrowTypeIndex", 1);
        UpdateType("Eye Tint Changed Index: ", "faceIndex", "eyeTypeIndex", 1);
        UpdateType("Skin Tint Changed Index: ", "faceIndex", "noseTypeIndex", 1);
        
        UpdateColor("Hair Color Changed Index: ", "faceIndex", "hairColorIndex", 0);
        UpdateColor("Eye Brow Color Changed Index: ", "faceIndex", "eyeBrowColorIndex", 0);
        UpdateColor("Eye Color Changed Index: ", "faceIndex", "eyeColorIndex", 0);
        UpdateColor("Skin Color Changed Index: ", "faceIndex", "noseColorIndex", 0);
        
        ChangeTab(0);
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
        Debug.Log("Button Deactivated: " + parent.transform.name);
        parent.transform.GetChild(index).GetComponent<Button>().interactable = false;
    }
    
    //=
    
    public void UpdateColor(string logMessage, string subTabName, string indexName, int i)
    {
        //"Eye Brow Color Changed Index: " 
        Debug.Log(logMessage + i);
        //"eyeBrowColorIndex"
        _faceBigTabKeys[indexName]  = i;
        UpdateColorSwitch(indexName);
        ChangeButtonActive(_buttonSpawner.colorButtonParent[_subTabKeys[subTabName]], _faceBigTabKeys[indexName]);
    }

    private void UpdateColorSwitch(string indexName)
    {
        switch (indexName)
        {
            case "hairColorIndex":
            {
                hairIcons[0].enabled = false;
                ChangeHairModel();
                hairIcons = ChangeColorMenu(hairIcons, hairData, "hairColorIndex");
                break;
            }
            case "eyeBrowColorIndex":
            {
                eyeBrowIcons[0].enabled = false;
                ChangeEyeBrowModel();
                eyeBrowIcons = ChangeColorMenu(eyeBrowIcons, eyeBrowData, "eyeBrowColorIndex");
                break;
            }
            case "eyeColorIndex":
            {
                eyeIcons[0].enabled = false;
                ChangeEyeModel();
                eyeIcons = ChangeColorMenu(eyeIcons, eyeData, "eyeColorIndex");
                break;
            }
            case "noseColorIndex":
            {
                noseIcons[0].enabled = false;
                ChangeSkinModel();
                noseIcons = ChangeColorMenu(noseIcons, noseData, "noseColorIndex");
                ChangeSkinColorOnModel();
                break;
            }
            default:
            {
                Debug.Log("Case Not Found");
                break;
            }
        }
    }
    
    private Image[] ChangeColorMenu(Image[] icons, SpriteData[] data, string path)
    {
        //Updates all the other hair to the new color 
        for (var i = 1; i < icons.Length; i++)
        {
            icons[i].sprite =  data[_faceBigTabKeys[path]].spriteData[i - 1];
        }
        return icons;
    }
    
    private void ChangeSkinColorOnModel()
    {
        //Head Update 
        bodyPart[0].sprite = headData[0].spriteData[_faceBigTabKeys["noseColorIndex"]];
        
        //Neck Update 
        bodyPart[1].sprite = neckData[0].spriteData[_faceBigTabKeys["noseColorIndex"]];
        //Arms 
        bodyPart[2].sprite = armData[0].spriteData[_faceBigTabKeys["noseColorIndex"]];
        bodyPart[4].sprite = armData[0].spriteData[_faceBigTabKeys["noseColorIndex"]];
        //Hands
        bodyPart[3].sprite = handData[0].spriteData[_faceBigTabKeys["noseColorIndex"]];
        bodyPart[5].sprite = handData[0].spriteData[_faceBigTabKeys["noseColorIndex"]];
        //Legs 
        bodyPart[6].sprite = legData[0].spriteData[_faceBigTabKeys["noseColorIndex"]];
        bodyPart[7].sprite = legData[0].spriteData[_faceBigTabKeys["noseColorIndex"]];
    }
    
    public void UpdateType(string logMessage, string subTabName, string indexName, int i)
    {
        Debug.Log(logMessage + i);
        _faceBigTabKeys[indexName] = i;
        UpdateTypeSwitch(indexName);
        Debug.Log(_subTabKeys[subTabName]);
        ChangeButtonActive(_buttonSpawner.buttonParents[_subTabKeys[subTabName]], _faceBigTabKeys[indexName]);
    }
    
    private void UpdateTypeSwitch(string indexName)
    {
        switch (indexName)
        {
            case "hairTypeIndex":
            {
                ChangeHairModel();
                ChangeHairCoordinates();
                break;
            }
            case "eyeBrowTypeIndex":
            {
                ChangeEyeBrowModel();
                break;
            }
            case "eyeTypeIndex":
            {
                ChangeEyeModel();
                break;
            }
            case "noseTypeIndex":
            {
                ChangeSkinModel();
                break;
            }
            default:
            {
                Debug.Log("Case Not Found");
                break;
            }
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
        switch (_faceBigTabKeys["hairTypeIndex"] )
        {
            //If Bold Don't Adjust 
            case 0:
                return;
            //If Short Hair Adjust by adding 
            case < 9:
                facePart[0].transform.position = _hairOrigin + hairCoordinates[_faceBigTabKeys["hairTypeIndex"]  - 1];
                break;
            //If Long hair adjust x by adding and y by subbing 
            default:
                facePart[0].transform.position = new Vector3(_hairOrigin.x,
                    _hairOrigin.y - hairCoordinates[_faceBigTabKeys["hairTypeIndex"]  - 1].y, 0);
                break;
        }
    }

    /// <summary>
    /// Updates the hair that's displayed on the model  
    /// </summary>
    private void ChangeHairModel()
    {
        //If bold turn hair off
        if (_faceBigTabKeys["hairTypeIndex"] == 0) { facePart[0].enabled = false; }
        //Else turn hair on and change to the desired style 
        else
        {
            facePart[0].enabled = true;
            facePart[0].sprite = hairData[_faceBigTabKeys["hairColorIndex"]].spriteData[_faceBigTabKeys["hairTypeIndex"] - 1];   
        }
    }
    
    //==================================================================================================================
    // Eye Brow Methods 
    //==================================================================================================================

    /// <summary>
    /// Updates the eye brows that's displayed on the model  
    /// </summary>
    private void ChangeEyeBrowModel()
    {
        //If bold turn hair off
        if (_faceBigTabKeys["eyeBrowTypeIndex"] == 0)
        {
            facePart[5].enabled = false;
            facePart[6].enabled = false; 
        }
        //Else turn hair on and change to the desired style 
        else
        {
            facePart[5].enabled = true;
            facePart[5].sprite = eyeBrowData[_faceBigTabKeys["eyeBrowColorIndex"]].spriteData[_faceBigTabKeys["eyeBrowTypeIndex"] - 1];   
            
            facePart[6].enabled = true;
            facePart[6].sprite = eyeBrowData[_faceBigTabKeys["eyeBrowColorIndex"]].spriteData[_faceBigTabKeys["eyeBrowTypeIndex"] - 1];   

        }
    }
    
    //==================================================================================================================
    // Eye Methods 
    //==================================================================================================================

    /// <summary>
    /// Updates the hair that's displayed on the model  
    /// </summary>
    private void ChangeEyeModel()
    {
        //If bold turn hair off
        if (_faceBigTabKeys["eyeTypeIndex"] == 0)
        {
            facePart[2].enabled = false;
            facePart[3].enabled = false;
        }
        //Else turn hair on and change to the desired style 
        else
        {
            facePart[2].enabled = true;
            facePart[2].sprite = eyeData[_faceBigTabKeys["eyeColorIndex"]].spriteData[_faceBigTabKeys["eyeTypeIndex"] - 1]; 
            facePart[3].enabled = true;
            facePart[3].sprite = eyeData[_faceBigTabKeys["eyeColorIndex"]].spriteData[_faceBigTabKeys["eyeTypeIndex"] - 1]; 
        }
    }
    
    //==================================================================================================================
    // Skin Tint Methods  
    //==================================================================================================================
    
    /// <summary>
    /// Updates the hair that's displayed on the model  
    /// </summary>
    private void ChangeSkinModel()
    {
        //If bold turn hair off
        if (_faceBigTabKeys["noseTypeIndex"] == 0) { facePart[1].enabled = false; }
        //Else turn hair on and change to the desired style 
        else
        {
            facePart[1].enabled = true;
            facePart[1].sprite = noseData[_faceBigTabKeys["noseColorIndex"]].spriteData[_faceBigTabKeys["noseTypeIndex"] - 1];
        }
    }

    public void ChangeTab(int newIndex)
    {
        for (int i = 0; i < 5; i++)
        {
            faceSubTabButtonsParent.transform.GetChild(i).gameObject.SetActive(false);
        }
        faceSubTabButtonsParent.transform.GetChild(newIndex).gameObject.SetActive(true);
    }


}
