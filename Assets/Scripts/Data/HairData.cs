using UnityEngine;

public class HairData : MonoBehaviour
{
    public Sprite[] hair = new Sprite[14];
    public int index = 0;
    
    public void Start()
    {
        for (var i = 0; i < hair.Length; i++)
        {
            hair[i] =  Resources.Load<Sprite>("Sprites/Hair/tint" + (index + 1) + "Hair" + (i+1));   
        }
    }

}
