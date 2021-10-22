using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTesting : MonoBehaviour
{
    System.Random r = new System.Random();
    Vector2 seedX;
    Vector2 seedY;
    Vector2 seedA;

    public float amplitude;

    // Start is called before the first frame update
    void Start()
    {
        Test();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Test()
    {
        for(int i = 0; i < 100; i++)
        {
            Debug.Log( i + " - " +MathTools.Q_sqrt(i));
        }
    }
}
