using UnityEngine;

/// <summary>
/// This script is a interface for the designer to dictate how many data prefabs should be created per type 
/// </summary>
public class SpawnData : MonoBehaviour
{
    //The Data Type PReFab
    public GameObject _preFabs;
    [Tooltip("Where the object in scene where the data will be spawned at")]
    public GameObject[] _parentObjects = new GameObject[] { };
    [Tooltip("How many colors variants there are")]
    public int[] _preFabsCount = new int[]{ };
    [Tooltip("How many unique sprites there are")]
    public int[] _arraySize= new int[]{ };
    private int currentIndex;

    /// <summary>
    /// Is called in Controls to start spawning all of the data prefabs 
    /// </summary>
    public void StartSpawnData()
    {
        for (var i = 0; i < _parentObjects.Length; i++)
        {
            currentIndex = i;
            Spawn();
        }
    }

    /// <summary>
    /// Spawns each prefab 
    /// </summary>
    private void Spawn()
    {
        for (var i = 0; i < _preFabsCount[currentIndex]; i++)
        {
            //Creates the prefab 
            var item = Instantiate(_preFabs, Vector3.zero, Quaternion.identity);
            //Tells it what data to fill it with 
            item.GetComponent<SpriteData>().ConnectSelect(currentIndex, i, _arraySize[currentIndex]);
            //Connects it to parent 
            item.transform.parent = _parentObjects[currentIndex].transform;
        }
    }
    
}
