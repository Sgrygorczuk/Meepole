using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Script is used for the Designer to interface with the Controls and ButtonSpawner scripts to create the buttons 
/// </summary>
public class InspectorEntry : MonoBehaviour
{
    /// <summary>
    ///This class is used in the button spawner to indicate the parents game objects where the button will be created,
    /// </summary>
    [Serializable] public class ButtonSpawnerData
    {
        [Tooltip("Used to tell where the button that select type will be parented")]
        public Transform buttonParentObject;
        [Tooltip("Used to tell where the button that select color will be parented")]
        public Transform colorButtonParentObject;
        [Tooltip("Define the colors that will be used when creating the color buttons")]
        public Color[] color = { };
        [Tooltip("What is the index name that keeps track of this variable, examples: hair,eyebrows")]
        public string indexName;
    }
    
    /// <summary>
    /// This class is used in the controls script to manage the button activity, such as  pulling data, updating the
    /// sprites, and ability to not have the sprite present 
    /// </summary>
    [Serializable] public class ButtonLinks
    {
        [Tooltip("Used to tell us how many color types of each item there is")]
        public SpriteData[] spriteData = new SpriteData[8];
        [Tooltip("Tells us what is the name of the Game Object in the scene that will pull the data from")]
        public string spriteDataPath;
        [Tooltip("Used to hold the types of items, if canBeEmpty is true add +1, example eyebrows: 4, eyes 2")]
        public Image[] icons = new Image[15];
        [Tooltip("Which body parts are affected by the changes")]
        public SpriteRenderer[] spriteBodyPart;
        [Tooltip("Can this type not be drawn")]
        public bool canBeEmpty = true;
    }

    /// <summary>
    /// Holds the ButtonSpawnerData and ButtonLinks so that they can be seen as a unit in the inspector 
    /// </summary>
    [Serializable] public class ButtonSetUp
    {
        public ButtonSpawnerData ButtonSpawnerDatas;
        public ButtonLinks ControlsData;
    }
    
    /// <summary>
    /// An array of Button Set Up so that the designer can input multiple sets of buttons for different tabs 
    /// </summary>
    [SerializeField] public ButtonSetUp[] ButtonSetUps = { };

}
