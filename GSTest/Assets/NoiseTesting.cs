using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class NoiseTesting : MonoBehaviour
{
    public PerlinNoise n;
    System.Random r = new System.Random();
    Vector2 seedX;
    Vector2 seedY;
    Vector2 seedA;

    public float amplitude;
    Stopwatch clock;
    bool done;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        clock = new Stopwatch();
        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(!done)
        //{
        if(timer > 3)
        {
            Test();
            timer = 0;
        }
        timer += Time.deltaTime;
            //done = !done;
        //}
    }

    void Test()
    {
        var Perlin = n.GenerateNoiseMap(128, 20);
        Texture2D text = new Texture2D(2560, 2560);
        for (int j = 0; j < text.height; j++)
        {
            for (int i = 0; i < text.width; i++)
            {
                var val = Perlin[j * text.width + i];
                text.SetPixel(i, j, new Color(val, val, val));
            }
        }
        text.Apply();
        GetComponent<Renderer>().material.mainTexture = text;
    }
}
