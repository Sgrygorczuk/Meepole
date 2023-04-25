using System;
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

    public readonly Dictionary<string, int> FaceBigTabKeys = new Dictionary<string, int>
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
        {"mouthColorIndex", 0},
    };
    
    [Serializable] public class ButtonLinks
    {
        public SpriteData[] spriteData = new SpriteData[8];
        public string spriteDataPath;
        public Image[] icons = new Image[15];
    }
    
    [SerializeField] public ButtonLinks[] buttonLinks = { };

    private Vector2 _hairOrigin = Vector2.zero;
    private const int PixelPerUnit = 200;

    public GameObject faceSubTabButtonsParent;
    
    public Vector2[] hairCoordinates = new Vector2[14];
    
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
        
        _buttonSpawner.SpawnButtons();
        
        SetUpButtons();
        //_buttonSpawner.buttonParents[0].GetChild(0).GetComponent<Button>().onClick.Invoke();
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
        FaceBigTabKeys[indexName]  = i;
        UpdateColorSwitch(indexName);
        ChangeButtonActive(_buttonSpawner.data[_subTabKeys[subTabName]].colorButtonParentObject, FaceBigTabKeys[indexName]);
    }

    private void UpdateColorSwitch(string indexName)
    {
        var index = 0; 
        
        switch (indexName)
        {
            case "hairColorIndex":
            {
                index = 0;
                ChangeHairModel();
                break;
            }
            case "eyeBrowColorIndex":
            {
                index = 1;
                ChangeEyeBrowModel();
                break;
            }
            case "eyeColorIndex":
            {
                index = 2;
                ChangeEyeModel();
                break;
            }
            case "noseColorIndex":
            {
                index = 3;
                ChangeSkinModel();
                ChangeSkinColorOnModel();
                break;
            }
            case "mouthColorIndex":
            {
                break;
            }
            default:
            {
                Debug.Log("Case Not Found");
                break;
            }
        }
        
        buttonLinks[index].icons[0].enabled = false;
        buttonLinks[index].icons = ChangeColorMenu(buttonLinks[index].icons, buttonLinks[index].spriteData, indexName);
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
    
    public void UpdateType(string logMessage, string subTabName, string indexName, int i)
    {
        Debug.Log(logMessage + i);
        FaceBigTabKeys[indexName] = i;
        UpdateTypeSwitch(indexName);
        Debug.Log(_subTabKeys[subTabName]);
        ChangeButtonActive(_buttonSpawner.data[_subTabKeys[subTabName]].buttonParentObject, FaceBigTabKeys[indexName]);
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
    /// Updates the hair that's displayed on the model  
    /// </summary>
    private void ChangeHairModel()
    {
        //If bold turn hair off
        if (FaceBigTabKeys["hairTypeIndex"] == 0) { facePart[0].enabled = false; }
        //Else turn hair on and change to the desired style 
        else
        {
            facePart[0].enabled = true;
            facePart[0].sprite = buttonLinks[0].spriteData[FaceBigTabKeys["hairColorIndex"]].spriteData[FaceBigTabKeys["hairTypeIndex"] - 1];   
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
        if (FaceBigTabKeys["eyeBrowTypeIndex"] == 0)
        {
            facePart[5].enabled = false;
            facePart[6].enabled = false; 
        }
        //Else turn hair on and change to the desired style 
        else
        {
            facePart[5].enabled = true;
            facePart[5].sprite = buttonLinks[1].spriteData[FaceBigTabKeys["eyeBrowColorIndex"]].spriteData[FaceBigTabKeys["eyeBrowTypeIndex"] - 1];   
            
            facePart[6].enabled = true;
            facePart[6].sprite = buttonLinks[1].spriteData[FaceBigTabKeys["eyeBrowColorIndex"]].spriteData[FaceBigTabKeys["eyeBrowTypeIndex"] - 1];   

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
        if (FaceBigTabKeys["eyeTypeIndex"] == 0)
        {
            facePart[2].enabled = false;
            facePart[3].enabled = false;
        }
        //Else turn hair on and change to the desired style 
        else
        {
            facePart[2].enabled = true;
            facePart[2].sprite = buttonLinks[2].spriteData[FaceBigTabKeys["eyeColorIndex"]].spriteData[FaceBigTabKeys["eyeTypeIndex"] - 1]; 
            facePart[3].enabled = true;
            facePart[3].sprite = buttonLinks[2].spriteData[FaceBigTabKeys["eyeColorIndex"]].spriteData[FaceBigTabKeys["eyeTypeIndex"] - 1]; 
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
        if (FaceBigTabKeys["noseTypeIndex"] == 0) { facePart[1].enabled = false; }
        //Else turn hair on and change to the desired style 
        else
        {
            facePart[1].enabled = true;
            facePart[1].sprite = buttonLinks[3].spriteData[FaceBigTabKeys["noseColorIndex"]].spriteData[FaceBigTabKeys["noseTypeIndex"] - 1];
        }
    }
    
    //==================================================================================================================
    // Mouth Tint Methods  
    //==================================================================================================================

    //TODO Connect 
    /// <summary>
    /// Updates the hair that's displayed on the model  
    /// </summary>
    private void MouthSkinModel()
    {
        //If bold turn hair off
        if (FaceBigTabKeys["noseTypeIndex"] == 0) { facePart[1].enabled = false; }
        //Else turn hair on and change to the desired style 
        else
        {
            facePart[1].enabled = true;
            facePart[1].sprite = buttonLinks[3].spriteData[FaceBigTabKeys["noseColorIndex"]].spriteData[FaceBigTabKeys["noseTypeIndex"] - 1];
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
