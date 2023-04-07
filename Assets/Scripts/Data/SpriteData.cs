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
                ConnectHair();
                break;
            }
            case 1:
            {
                ConnectEyeBrow();
                break;
            }
            case 2:
            {
                ConnectEyes();
                break;
            }
        }
    }

    private void ConnectHair()
    {
        for (var i = 0; i < spriteData.Length; i++)
        {
            spriteData[i] =  Resources.Load<Sprite>("Sprites/Hair/tint" + (index + 1) + "Hair" + (i+1));   
        }
    }
    
    private void ConnectEyeBrow()
    {
        for (var i = 0; i < spriteData.Length; i++)
        {
            spriteData[i] =  Resources.Load<Sprite>("Sprites/EyeBrows/tint" + (index + 1) + "EyeBrow" + (i+1));   
        }
    }

    private void ConnectEyes()
    {
        for (var i = 0; i < spriteData.Length; i++)
        {
            spriteData[i] =  Resources.Load<Sprite>("Sprites/Eyes/tint" + (index + 1) + "Eye" + (i+1));   
        }
    }

    private void ConnectSkinTint()
    {
        
    }
}
