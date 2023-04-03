using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeData : MonoBehaviour
{
    public Sprite[] eye = new Sprite[2];
    public int index = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < eye.Length; i++)
        {
            eye[i] =  Resources.Load<Sprite>("Sprites/Eyes/tint" + (index + 1) + "Eye" + (i+1));   
        }
    }
    
}
