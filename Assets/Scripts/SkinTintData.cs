using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinTintData : MonoBehaviour
{
    [Tooltip("0 = Head,\n 1 = Neck,\n 2 = Arm,\n 3 = Hand,\n 4 = Leg")]
    public Sprite[] spriteTint = new Sprite[5];
    public Sprite[] noseTints = new Sprite[3];
    public int index = 0;

    public void Start()
    {
        spriteTint[0] =  Resources.Load<Sprite>("Sprites/Head/tint" +  (index + 1) + "_head");
        spriteTint[1] =  Resources.Load<Sprite>("Sprites/Neck/tint" +  (index + 1) + "_neck");
        spriteTint[2] =  Resources.Load<Sprite>("Sprites/Arm/tint" +  (index + 1) + "_arm");
        spriteTint[3] =  Resources.Load<Sprite>("Sprites/Hand/tint" +  (index + 1) + "_hand");
        spriteTint[4] =  Resources.Load<Sprite>("Sprites/Leg/tint" +  (index + 1) + "_leg");
        
        noseTints[0] =  Resources.Load<Sprite>("Sprites/Nose/tint" +  (index + 1) + "Nose1");
        noseTints[1] =  Resources.Load<Sprite>("Sprites/Nose/tint" +  (index + 1) + "Nose2");
        noseTints[2] =  Resources.Load<Sprite>("Sprites/Nose/tint" +  (index + 1) + "Nose3");
    }
}
