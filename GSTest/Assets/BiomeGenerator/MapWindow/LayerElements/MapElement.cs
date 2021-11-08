using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class MapElement : MonoBehaviour
{
    public GameObject prefab;
    public int priority;
    public List<BaseStat> stats;
    public List<BaseStat> requirements;
    public float scaleRandomRange;
    public Vector3 rotationRandomRange;
    public Vector3 positionRandomRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }
     
    // Update is called once per frame
    void Update()
    {
        
    }
}
