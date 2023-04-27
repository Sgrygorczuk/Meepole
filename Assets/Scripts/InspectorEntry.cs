using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class InspectorEntry : MonoBehaviour
{
    [Serializable] public class ButtonSpawnerData
    {
        public Transform buttonParentObject;
        public Transform colorButtonParentObject;
        public Color[] color = { };
        public string subTabName;
        public string indexName;
    }
    
    [Serializable] public class ButtonLinks
    {
        public SpriteData[] spriteData = new SpriteData[8];
        public string spriteDataPath;
        public Image[] icons = new Image[15];
        public SpriteRenderer[] spriteBodyPart;
        public bool canBeEmpty = true;
    }

    [Serializable]
    public class ButtonSetUp
    {
        public ButtonSpawnerData ButtonSpawnerDatas;
        public ButtonLinks ControlsData;
    }
    
    [SerializeField] public ButtonSetUp[] ButtonSetUps = { };

}
