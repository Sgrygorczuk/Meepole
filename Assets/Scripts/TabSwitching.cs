using UnityEngine;
using UnityEngine.UI;

public class TabSwitching : MonoBehaviour
{
    public GameObject subTabButtonsParent;
    public Button[] subTabButtons;
    public GameObject faceSubTabButtonsParent;
    public Button[] faceTabButtons;
    public GameObject bodySubTabButtonsParent;
    public Button[] bodyTabButtons;
    public GameObject legsSubTabButtonsParent;
    public Button[] legsTabButtons;
    private Controls _controls;
    
    private void Start()
    {
        ChangeFaceTab(0);
        ChangeSubTabs(0);
    }

    public void ChangeSubTabs(int newIndex)
    {
        for (var i = 0; i < 3; i++)
        {
            subTabButtonsParent.transform.GetChild(i).gameObject.SetActive(false);
            subTabButtons[i].interactable = true;
        }
        
        subTabButtonsParent.transform.GetChild(newIndex).gameObject.SetActive(true);
        subTabButtons[newIndex].interactable = false;

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
    
    public void ChangeFaceTab(int newIndex)
    {
        TurnAllOff();
        faceTabButtons[newIndex].interactable = false;
        faceSubTabButtonsParent.transform.GetChild(newIndex).gameObject.SetActive(true);
    }
    
    public void ChangeBodyTab(int newIndex)
    {
        TurnAllOff();
        bodyTabButtons[newIndex].interactable = false;
        bodySubTabButtonsParent.transform.GetChild(newIndex).gameObject.SetActive(true);
    }
    
    public void ChangeBottomTab(int newIndex)
    {
        TurnAllOff();
        legsTabButtons[newIndex].interactable = false;
        legsSubTabButtonsParent.transform.GetChild(newIndex).gameObject.SetActive(true);
    }

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
