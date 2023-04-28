using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Script is used to switch between the different tabs,
///     the 3 Sub Tabs - Face, Body, and Bottoms
///     Face Tabs - Hair, Eye Brows, Eyes, Nose, and Mouth
///     Body - Shirt, and Sleeves
///     Bottoms - Belt, Pants, and Shoes 
/// </summary>
public class TabSwitching : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //The parent which holds the different tabs with buttons 
    public GameObject subTabButtonsParent;
    public GameObject faceSubTabButtonsParent;
    public GameObject bodySubTabButtonsParent;
    public GameObject legsSubTabButtonsParent;
    
    //The buttons which will be turned on and off 
    public Button[] subTabButtons;
    public Button[] faceTabButtons;
    public Button[] bodyTabButtons;
    public Button[] legsTabButtons;

    /// <summary>
    /// Runs the functions once so it always start with both tab selection in the 0th postions 
    /// </summary>
    private void Start()
    {
        ChangeFaceTab(0);
        ChangeSubTabs(0);
    }
    
    //==================================================================================================================
    // Sub Swapping 
    //==================================================================================================================

    /// <summary>
    /// Used to switch the main tabs of Face, Body and Legs 
    /// </summary>
    /// <param name="newIndex"></param>
    public void ChangeSubTabs(int newIndex)
    {
        //Resets all of the buttons and tabs 
        for (var i = 0; i < 3; i++)
        {
            subTabButtonsParent.transform.GetChild(i).gameObject.SetActive(false);
            subTabButtons[i].interactable = true;
        }
        
        //Activates on only the the one that was clicked  
        subTabButtonsParent.transform.GetChild(newIndex).gameObject.SetActive(true);
        subTabButtons[newIndex].interactable = false;

        //Resets the sub tab to the 0th page, ex Face -> Hair, Body -> Shirt, Bottoms -> Belt
        switch (newIndex)
        {
            case 0:
            {
                ChangeFaceTab(0);
                break;
            }
            case 1:
            {
                ChangeBodyTab(0);
                break;
            }
            case 2:
            {
                ChangeBottomTab(0);
                break;
            }
        }
    }
    
    //==================================================================================================================
    // Sub Tab Spawning 
    //==================================================================================================================

    //Changes the Face Tab between, Hair, Eye Brows, Eyes, Nose and Mouth
    public void ChangeFaceTab(int newIndex)
    {
        TurnAllOff();
        faceTabButtons[newIndex].interactable = false;
        faceSubTabButtonsParent.transform.GetChild(newIndex).gameObject.SetActive(true);
    }
    
    //Changes Body Tab between Shirt and Sleeves
    public void ChangeBodyTab(int newIndex)
    {
        TurnAllOff();
        bodyTabButtons[newIndex].interactable = false;
        bodySubTabButtonsParent.transform.GetChild(newIndex).gameObject.SetActive(true);
    }
    
    //Changes Bottom tabs between Belt, Pants, and Shoes 
    public void ChangeBottomTab(int newIndex)
    {
        TurnAllOff();
        legsTabButtons[newIndex].interactable = false;
        legsSubTabButtonsParent.transform.GetChild(newIndex).gameObject.SetActive(true);
    }
    
    //==================================================================================================================
    // General Function Spawning 
    //==================================================================================================================

    /// <summary>
    /// Goes through all of the buttons and resets them to be off
    /// </summary>
    private void TurnAllOff()
    {
        for (var i = 0; i < 5; i++)
        {
            faceSubTabButtonsParent.transform.GetChild(i).gameObject.SetActive(false);
            faceTabButtons[i].interactable = true;
        }
        
        for (var i = 0; i < 2; i++)
        {
            bodySubTabButtonsParent.transform.GetChild(i).gameObject.SetActive(false);
            bodyTabButtons[i].interactable = true;
        }
        
        for (var i = 0; i < 3; i++)
        {
            legsSubTabButtonsParent.transform.GetChild(i).gameObject.SetActive(false); 
            legsTabButtons[i].interactable = true;
        }
        
    }
    
    
}
