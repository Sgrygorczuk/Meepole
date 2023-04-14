using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonIndexes : MonoBehaviour
{
    public readonly Dictionary<string, int> SubTabKeys = new Dictionary<string, int>
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
    };
}
