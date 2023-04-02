using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour
{
    public Transform hairButtonParent;
    public Transform hairColorButtonParent;
    public GameObject hairButtonPreFab;
    public GameObject colorButtonPreFab;

    private Vector2 _oringPanel = new (-245, 0);
    private Vector2 _origin = new(-220, 300);
    private Controls _controls; 
    private float _spacing = 150;
    private float _spacing2 = 70;

    public Color[] hairColor = new Color[8];
    
    // Start is called before the first frame update
    public void SpawnHairButtons()
    {
        _controls = GameObject.Find("Controls").GetComponent<Controls>();
        for (int i = 0; i < _controls.hairIcons.Length; i++)
        {
            SpawnHairButton(i);   
        }

        for (int i = 0; i < hairColor.Length; i++)
        {
            SpawnHairColorButton(i);
        }
    }

    private void SpawnHairButton(int i)
    {
        // Instantiate the button prefab
        var newButton = Instantiate(hairButtonPreFab, transform);
        
        //Links the button to be parented and placed in correct spot on board  
        newButton.transform.SetParent(hairButtonParent);
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(_origin.x + _spacing * (i%4), _origin.y - _spacing * ((i/4)%4));

        //Grabs the icon 
        _controls.hairIcons[i] = newButton.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        
        // Connect the On Click () event to the buttonScript component's OnButtonClick() function
        newButton.GetComponent<Button>().onClick.AddListener(() => _controls.UpdateHairType(i));
    }

    private void SpawnHairColorButton(int i)
    {
        // Instantiate the button prefab
        var newButton = Instantiate(colorButtonPreFab, transform);
        
        //Links the button to be parented and placed in correct spot on board  
        newButton.transform.SetParent(hairColorButtonParent);
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(_oringPanel.x + _spacing2 * i, _oringPanel.y);
        
        //Grabs the icon 
        newButton.transform.GetChild(0).GetComponent<Image>().color = hairColor[i];

        // Connect the On Click () event to the buttonScript component's OnButtonClick() function
        newButton.GetComponent<Button>().onClick.AddListener(() => _controls.UpdateHairColor(i));
    }
}
