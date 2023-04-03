using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBrowData : MonoBehaviour
{
    public Sprite[] eyeBrows = new Sprite[3];
    public int index = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < eyeBrows.Length; i++)
        {
            eyeBrows[i] =  Resources.Load<Sprite>("Sprites/EyesBrows/tint" + (index + 1) + "EyesBrows" + (i+1));   
        }
    }
}
