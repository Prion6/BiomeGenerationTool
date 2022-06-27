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
    public List<Requirement> requirements;
    [Range(1,10)]
    public float scaleRandomRange;
    public Vector3 rotationRandomRange;
    public Vector3 positionRandomRange;
}
