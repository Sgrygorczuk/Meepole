using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    
    [Tooltip("0 = Head,\n 1 = Neck,\n 2 = L_Arm,\n 3 = L_Hand,\n 4 = R_Arm,\n 5 = R_Hand,\n 6 = L_Leg,\n" +
             "7 = R_Leg")]
    public SpriteRenderer[] bodyPart = new SpriteRenderer[8];
    
    [Tooltip("0 = Hair,\n 1 = Nose,\n 2 = L_Eyes,\n 3 = R_Eye,\n 4 = Mouth")]
    public SpriteRenderer[] facePart = new SpriteRenderer[5];

    private int _tintIndex = 0;
    private int _noseIndex = 0;
    private int _hairColorIndex = 0;
    private int _hairTypeIndex = 0;
    
    private Vector2 _hairOrigin = Vector2.zero;
    private const int PixelPerUnit = 200;
    
    public SkinTintData[] skinTintData = new SkinTintData[8];

    public GameObject hairButtonsParent;
    public GameObject hairColorButtonParent;
    
    public HairData[] hairData = new HairData[8];
    public Vector2[] hairCoordinates = new Vector2[14];
    public Image[] hairIcons = new Image[15];

    private ButtonSpawner _buttonSpawner;

    // Start is called before the first frame update
    void Start()
    {
        _buttonSpawner = GetComponent<ButtonSpawner>();
        
        SetUpSkinTint();
        
        SetUpHairTint();
        SetUpHairCoordinates();
        
        _buttonSpawner.SpawnHairButtons();
        
        SetUpButtons();
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
            hairData[i] = hairTintObject.transform.GetChild(i).GetComponent<HairData>();
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
        UpdateSkinTint(0);
        UpdateHairType(0);
        UpdateHairColor(0);
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

    //==================================================================================================================
    // Skin Tint Methods  
    //==================================================================================================================
    
    public void UpdateSkinTint(int index)
    {
        Debug.Log("Skin Tint Changed Index: " + index);
        _tintIndex = index;
        ChangeSkinTintOnModel();
        ChangeSkinTintOnModel();
    }
    
    private void ChangeSkinTintOnModel()
    {
        //Head Update 
        bodyPart[0].sprite = skinTintData[_tintIndex].spriteTint[0];
        //Neck Update 
        bodyPart[1].sprite = skinTintData[_tintIndex].spriteTint[1];
        //Arms 
        bodyPart[2].sprite = skinTintData[_tintIndex].spriteTint[2];
        bodyPart[4].sprite = skinTintData[_tintIndex].spriteTint[2];
        //Hands
        bodyPart[3].sprite = skinTintData[_tintIndex].spriteTint[3];
        bodyPart[5].sprite = skinTintData[_tintIndex].spriteTint[3];
        //Legs 
        bodyPart[6].sprite = skinTintData[_tintIndex].spriteTint[4];
        bodyPart[7].sprite = skinTintData[_tintIndex].spriteTint[4];
        
        ChangeNoseTint();
    }

    private void ChangeNoseTint()
    {
        //Updates Nose 
        facePart[1].sprite = skinTintData[_tintIndex].noseTints[_noseIndex];
    }

    private void ChangeSkinTintInMenu()
    {
        
    }
    
    //==================================================================================================================
    // Hair Methods 
    //==================================================================================================================

    /// <summary>
    ///  Used by button to prompt the change of a hair type
    /// </summary>
    /// <param name="i"></param>
    public void UpdateHairType(int i)
    {
        Debug.Log("Hair Type Changed Index: " + i);
        _hairTypeIndex = i;
        ChangeHairModel();
        ChangeHairCoordinates();
        ChangeButtonActive(hairButtonsParent, _hairTypeIndex);
    }

    /// <summary>
    /// Updates the coordinates of the hair on the model to account for the different sprite sizes  
    /// </summary>
    private void ChangeHairCoordinates()
    {
        switch (_hairTypeIndex)
        {
            //If Bold Don't Adjust 
            case 0:
                return;
            //If Short Hair Adjust by adding 
            case < 9:
                facePart[0].transform.position = _hairOrigin + hairCoordinates[_hairTypeIndex - 1];
                break;
            //If Long hair adjust x by adding and y by subbing 
            default:
                facePart[0].transform.position = new Vector3(_hairOrigin.x,
                    _hairOrigin.y - hairCoordinates[_hairTypeIndex - 1].y, 0);
                break;
        }
    }
    
    /// <summary>
    /// Used by button to prompt change to a new hair color. i is set as the new index for the color selected. 
    /// </summary>
    /// <param name="i"></param>
    public void UpdateHairColor(int i)
    {
        Debug.Log("Hair Color Changed Index: " + i);
        _hairColorIndex = i;
        ChangeHairModel();
        ChangeHairColorMenu();
        ChangeButtonActive(hairColorButtonParent, _hairColorIndex);
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
            hairIcons[i].sprite =  hairData[_hairColorIndex].hair[i - 1];
        }
    }
    
    /// <summary>
    /// Updates the hair that's displayed on the model  
    /// </summary>
    private void ChangeHairModel()
    {
        //If bold turn hair off
        if (_hairTypeIndex == 0) { facePart[0].enabled = false; }
        //Else turn hair on and change to the desired style 
        else
        {
            facePart[0].enabled = true;
            facePart[0].sprite = hairData[_hairColorIndex].hair[_hairTypeIndex - 1];   
        }
    }

}
