using UnityEngine;

public class SpawnData : MonoBehaviour
{
    public GameObject _preFabs;
    public GameObject[] _parentObjects = new GameObject[] { };
    public int[] _preFabsCount = new int[]{ };
    public int[] _arraySize= new int[]{ };
    private int currentIndex; 
    
    // Start is called before the first frame update
    public void StartSpawnData()
    {
        for (var i = 0; i < _parentObjects.Length; i++)
        {
            currentIndex = i;
            Spawn();
        }
    }

    private void Spawn()
    {
        for (var i = 0; i < _preFabsCount[currentIndex]; i++)
        {
            var item = Instantiate(_preFabs, Vector3.zero, Quaternion.identity);
            item.GetComponent<SpriteData>().ConnectSelect(currentIndex, i, _arraySize[currentIndex]);
            item.transform.parent = _parentObjects[currentIndex].transform;
        }
    }
    
}
