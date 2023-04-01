using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [Tooltip("0 = Head,\n 1 = Neck,\n 2 = L_Arm,\n 3 = L_Hand,\n 4 = R_Arm,\n 5 = R_Hand,\n 6 = L_Leg,\n" +
             "7 = R_Leg")]
    public SpriteRenderer[] bodyPart = new SpriteRenderer[8];
    
    [Tooltip("0 = Hair,\n 1 = Nose,\n 2 = L_Eyes,\n 3 = R_Eye,\n 4 = Mouth")]
    public SpriteRenderer[] facePart = new SpriteRenderer[5];

    private int _tintIndex = 0;
    private int _noseIndex = 0;
    
    public SkinTintData[] skinTintData = new SkinTintData[8];
    public Vector2[] hairCoordinates = new Vector2[14];

    // Start is called before the first frame update
    void Start()
    {
        SetUpSkinTint();
        SetUpHairCoordinates();
    }

    private void SetUpSkinTint()
    {
        var skinTintObject = GameObject.Find("SkinTintData").gameObject;
        for (var i = 0; i < skinTintData.Length; i++)
        {
            skinTintData[i] = skinTintObject.transform.GetChild(i).GetComponent<SkinTintData>();
        }
    }

    private void SetUpHairCoordinates()
    {
        var baseWidth =  Resources.Load<Sprite>("Sprites/Hair/tint1Hair1").textureRect.width;
        var baseHeight =  Resources.Load<Sprite>("Sprites/Hair/tint1Hair1").textureRect.height;
        
        for (var i = 0; i < hairCoordinates.Length; i++){
            hairCoordinates[i][0] =  Resources.Load<Sprite>("Sprites/Hair/tint1Hair" + (i+1)).textureRect.width - baseWidth;
            hairCoordinates[i][1] =  Resources.Load<Sprite>("Sprites/Hair/tint1Hair" + (i+1)).textureRect.height - baseHeight;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    //==================================================================================================================
    // SKIN TINT 
    //==================================================================================================================
    
    public void ChangeSkinTint(int index)
    {
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
    // HAIR TYPE 
    //==================================================================================================================

    public void ChangeHairType(int i)
    {
        ChangeHair();
        ChangeHairCoordinates();
    }

    private void ChangeHair()
    {
        
    }

    private void ChangeHairCoordinates()
    {
        
    }
    
}
