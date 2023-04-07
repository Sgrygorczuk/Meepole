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
        {"skinTintIndex", 0},
        {"noseIndex", 0},
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
    
    public SkinTintData[] skinTintData = new SkinTintData[8];

    public GameObject faceSubTabButtonsParent; 
    public GameObject[] buttonsParents = new GameObject[]{};
    public GameObject[] colorButtonParents = new GameObject[]{};
    
    public SpriteData[] hairData = new SpriteData[8];
    public Vector2[] hairCoordinates = new Vector2[14];
    public Image[] hairIcons = new Image[15];
    
    public SpriteData[] eyeBrowData = new SpriteData[8]; 
    public Image[] eyeBrowIcons = new Image[4];

    private ButtonSpawner _buttonSpawner;
    private SpawnData _spawnData;

    // Start is called before the first frame update
    void Start()
    {
        _buttonSpawner = GetComponent<ButtonSpawner>();
        _spawnData = GetComponent<SpawnData>();
        
        _spawnData.StartSpawnData();
        
        SetUpSkinTint();
        
        SetUpHairTint();
        SetUpEyeBrowTint();
        SetUpHairCoordinates();
        
        _buttonSpawner.SpawnButtons();
        
        SetUpButtons();
        //_buttonSpawner.buttonParents[0].GetChild(0).GetComponent<Button>().onClick.Invoke();
    }

    //==================================================================================================================
    // Set Up Data & UI Methods 
    //==================================================================================================================

    private void SetUpSkinTint()
    {
        var skinTintObject = GameObject.Find("SkinTintData").gameObject;
        for (var i = 0; i < skinTintData.Length; i++)
        {
            skinTintData[i] = skinTintObject.transform.GetChild(i).GetComponent<SkinTintData>();
        }
    }
    
    private void SetUpHairTint()
    {
        var hairTintObject = GameObject.Find("HairTintData").gameObject;
        for (var i = 0; i < hairData.Length; i++)
        {
            hairData[i] = hairTintObject.transform.GetChild(i).GetComponent<SpriteData>();
        }
    }
    
    private void SetUpEyeBrowTint()
    {
        var eyeBrowTintObject = GameObject.Find("EyeBrowTintData").gameObject;
        for (var i = 0; i < eyeBrowData.Length; i++)
        {
            eyeBrowData[i] = eyeBrowTintObject.transform.GetChild(i).GetComponent<SpriteData>();
        }
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
        UpdateType("Hair Type Changed Index: ", "faceIndex", "hairTypeIndex", 0);
        UpdateType("Eye Brow Tint Changed Index: ", "faceIndex", "eyeBrowTypeIndex", 0);
        
        UpdateColor("Hair Color Changed Index: ", "faceIndex", "hairColorIndex", 0);
        UpdateColor("Eye Brow Color Changed Index: ", "faceIndex", "eyeBrowColorIndex", 0);
    }
    
    //==================================================================================================================
    // General Methods 
    //==================================================================================================================

    private void ChangeButtonActive(GameObject parent, int index)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
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
        ChangeButtonActive(colorButtonParents[_subTabKeys[subTabName]], _faceBigTabKeys[indexName]);
    }

    private void UpdateColorSwitch(string indexName)
    {
        switch (indexName)
        {
            case "hairColorIndex":
            {
                ChangeHairModel();
                ChangeHairColorMenu();
                break;
            }
            case "eyeBrowColorIndex":
            {
                ChangeEyeBrowModel();
                ChangeEyeBrowColorMenu();
                break;
            }
        }
    }
    
    public void UpdateType(string logMessage, string subTabName, string indexName, int i)
    {
        Debug.Log(logMessage + i);
        _faceBigTabKeys[indexName] = i;
        UpdateTypeSwitch(indexName);
        ChangeButtonActive(buttonsParents[_subTabKeys[subTabName]], _faceBigTabKeys[indexName]);
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
        }
    }

    //==================================================================================================================
    // Skin Tint Methods  
    //==================================================================================================================

    private void ChangeSkinTintOnModel()
    {
        //Head Update 
        bodyPart[0].sprite = skinTintData[_faceBigTabKeys["skinTintIndex"]].spriteTint[0];
        //Neck Update 
        bodyPart[1].sprite = skinTintData[_faceBigTabKeys["skinTintIndex"]].spriteTint[1];
        //Arms 
        bodyPart[2].sprite = skinTintData[_faceBigTabKeys["skinTintIndex"]].spriteTint[2];
        bodyPart[4].sprite = skinTintData[_faceBigTabKeys["skinTintIndex"]].spriteTint[2];
        //Hands
        bodyPart[3].sprite = skinTintData[_faceBigTabKeys["skinTintIndex"]].spriteTint[3];
        bodyPart[5].sprite = skinTintData[_faceBigTabKeys["skinTintIndex"]].spriteTint[3];
        //Legs 
        bodyPart[6].sprite = skinTintData[_faceBigTabKeys["skinTintIndex"]].spriteTint[4];
        bodyPart[7].sprite = skinTintData[_faceBigTabKeys["skinTintIndex"]].spriteTint[4];
        
        ChangeNoseTint();
    }

    private void ChangeNoseTint()
    {
        //Updates Nose 
        facePart[1].sprite = skinTintData[_faceBigTabKeys["skinTintIndex"]].noseTints[_faceBigTabKeys["noseIndex"]];
    }

    private void ChangeSkinTintInMenu()
    {
        
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
    /// Updates the hair that's seen on the icon, changing the color of each type 
    /// </summary>
    private void ChangeHairColorMenu()
    {
        //Sets the first icon to have no hair 
        hairIcons[0].enabled = false;
        //Updates all the other hair to the new color 
        for (var i = 1; i < hairIcons.Length; i++)
        {
            hairIcons[i].sprite =  hairData[_faceBigTabKeys["hairColorIndex"]].spriteData[i - 1];
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
    /// Updates the hair that's seen on the icon, changing the color of each type 
    /// </summary>
    private void ChangeEyeBrowColorMenu()
    {
        //Sets the first icon to have no hair 
        eyeBrowIcons[0].enabled = false;
        //Updates all the other hair to the new color 
        for (var i = 1; i < eyeBrowIcons.Length; i++)
        {
            eyeBrowIcons[i].sprite =  eyeBrowData[_faceBigTabKeys["eyeBrowColorIndex"]].spriteData[i - 1];
        }
    }

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
    
}
