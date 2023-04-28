using UnityEngine;

/// <summary>
/// Called in Spawn Data and used by the TintData prefab, used to fill out the sprite data, such as skin tones, shirts,
/// and all other types and their color variants. This could very well be refactored to worked better. 
/// </summary>
public class SpriteData : MonoBehaviour
{
    //This row of sprites 
    public Sprite[] spriteData;
    //??? 
    public int index ;

    /// <summary>
    /// Takes in the information to find the right sprite asset and connect them to the array that controls can connect to
    /// </summary> 
    /// <param name="preFabIndex"></param> Which item in the array is this 
    /// <param name="pathIndex"></param>  ???
    /// <param name="arraySize"></param> Tells us how many types of items there is 
    public void ConnectSelect(int preFabIndex, int pathIndex, int arraySize)
    {
        index = pathIndex;
        spriteData = new Sprite[arraySize];
        
        //Links to respective tab in the resource folder 
        switch (preFabIndex)
        {
            case 0:
            {
                Connect("Sprites/Hair/tint", "Hair");
                break;
            }
            case 1:
            {
                Connect("Sprites/EyeBrows/tint" , "EyeBrow");
                break;
            }
            case 2:
            {
                Connect("Sprites/Eyes/tint", "Eye");
                break;
            }
            case 3:
            {
                Connect("Sprites/Nose/tint", "Nose");
                break;
            }
            case 4:
            {
                ConnectBody("Sprites/Arm/tint", "_arm");
                break;
            }
            case 5:
            {
                ConnectBody("Sprites/Hand/tint", "_hand");
                break;
            }
            case 6:
            {
                ConnectBody("Sprites/Leg/tint", "_leg");
                break;
            }
            case 7:
            {
                ConnectBody("Sprites/Head/tint", "_head");
                break;
            }
            case 8:
            {
                ConnectBody("Sprites/Neck/tint", "_neck");
                break;
            }
            case 9:
            {
                Connect("Sprites/Mouth/tint", "Mouth");
                break;
            }
            case 10:
            {
                Connect("Sprites/Shirt/tint", "Shirt");
                break;
            }
            case 11:
            {
                Connect("Sprites/Sleeve/tint", "Sleeve");
                break;
            }
            case 12:
            {
                Connect("Sprites/Belt/tint", "Belt");
                break;
            }
            case 13:
            {
                Connect("Sprites/Pants/tint", "Pants");
                break;
            }
            case 14:
            {
                Connect("Sprites/Shoes/tint", "Shoes");
                break;
            }
        }
    }

    /// <summary>
    /// Properly connect the data 
    /// </summary>
    /// <param name="startPath"></param>
    /// <param name="endPath"></param>
    private void Connect(string startPath, string endPath)
    {
        for (var i = 0; i < spriteData.Length; i++)
        {
            spriteData[i] =  Resources.Load<Sprite>(startPath + (index + 1) + endPath + (i+1));   
        }
    }
    
    /// <summary>
    /// Dumb off shoot I made to suit one specfic type of data that makes this harder to clean up 
    /// </summary>
    /// <param name="startPath"></param>
    /// <param name="endPath"></param>
    private void ConnectBody(string startPath, string endPath)
    {
        for (var i = 0; i < spriteData.Length; i++)
        {
            spriteData[i] =  Resources.Load<Sprite>(startPath + (i + 1) + endPath);   
        }
    }
}
