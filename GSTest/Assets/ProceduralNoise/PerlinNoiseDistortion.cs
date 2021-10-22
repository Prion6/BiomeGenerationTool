using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralGeneration/PerlinNoiseDistortion")]
public class PerlinNoiseDistortion : ScriptableObject, INoiseGenerator
{
    //public float baseScale = 4.7f;
    public int octaves = 4;
    public float amplitude = 5.0f;
    //public float lacunarity = 2.0f;
   // public float persistence = 0.5f;
    System.Random r = new System.Random();

    Vector2 seedX;
    Vector2 seedY;
    Vector2 seedA;

    // Arbitrary numbers to break up visible correlation between octaves / x & y
    //public Vector2 seed = new Vector2(-71, 37);

    /*public float[] GenerateNoiseMap(int width, int height, LayerChromosome map)
    {
        System.Random r = new System.Random();
        seed = new Vector2(r.Next(0,10000), r.Next(0, 10000));
        float[] turbulentMap = new float[width * height];
        for(int j = 0; j < height; j++)
        {
            for(int i = 0; i < width; i++)
            {
                turbulentMap[j * width + i] = (float)(double.Parse(map.GetGene(GetTurbulentPixel(new Vector2(i, j), width, height)).Value.ToString()));// + double.Parse(map.GetGene(j * width + i).Value.ToString()))/2;
            }
        }
        return turbulentMap;
    }

    int GetTurbulentPixel(Vector2 input, int width, int height)
    {

        input = input / baseScale + seed;
        float a = 2f * amplitude;

        float x = 0;
        float y = 0;


        for (int octave = 0; octave < octaveCount; octave++)
        {
            x += a * (Mathf.PerlinNoise(input.x, input.y) - 0.5f);
            y += a * (Mathf.PerlinNoise(input.x + seed.y, input.y + seed.y) - 0.5f);
            input = input * lacunarity + seed;
            a *= persistence;
        }
        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, height);

        return (int)(y*width + x);
    }*/

    public float[] GenerateNoiseMap(LayerChromosome map)
    {
        var genes = map.ToFloatingPoints();
        seedX = new Vector2(r.Next(0, 3000), r.Next(0, 3000));
        seedY = new Vector2(r.Next(3000, 6000), r.Next(3000, 6000));
        seedA = new Vector2(r.Next(6000, 9000), r.Next(6000, 9000));

        float[] turbulentMap = new float[ map.N * map.N];
        //Debug.Log(turbulentMap.Length);
        for (int j = 0; j < map.N; j++)
        {
            for (int i = 0; i < map.N; i++)
            {
                int index = Mathf.Clamp(DistortedIndex(i, j, map.N, map.N), 0, genes.Length-1);
                turbulentMap[j * map.N + i] = genes[index];
                turbulentMap[j * map.N + i] *= (Mathf.Abs(Mathf.PerlinNoise(i * 1.0f / map.N + seedA.x, j * 1.0f / map.N + seedA.y)) + 0.5f);
                
            }
        }
        
        return turbulentMap;
    }

    public int DistortedIndex(float x, float y, int width, int height)
    {
        float dx = (int)(x + amplitude * (Mathf.Abs(Mathf.PerlinNoise(x * 1.0f / width + seedX.x, y * 1.0f / height + seedX.y) - 0.5f)));
        float dy = (int)(y + amplitude * (Mathf.Abs(Mathf.PerlinNoise(x * 1.0f / width + seedY.x, y * 1.0f / height + seedY.y) - 0.5f)));

        return (int)(dy * width + dx);
    }
}
