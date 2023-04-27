using UnityEngine;

public class SpriteData : MonoBehaviour
{
    public Sprite[] spriteData;
    public int index = 0;

    public void ConnectSelect(int preFabIndex, int pathIndex, int arraySize)
    {
        index = pathIndex;
        spriteData = new Sprite[arraySize];
        
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

    private void Connect(string startPath, string endPath)
    {
        for (var i = 0; i < spriteData.Length; i++)
        {
            spriteData[i] =  Resources.Load<Sprite>(startPath + (index + 1) + endPath + (i+1));   
        }
    }
    
    private void ConnectBody(string startPath, string endPath)
    {
        for (var i = 0; i < spriteData.Length; i++)
        {
            spriteData[i] =  Resources.Load<Sprite>(startPath + (i + 1) + endPath);   
        }
    }
}
