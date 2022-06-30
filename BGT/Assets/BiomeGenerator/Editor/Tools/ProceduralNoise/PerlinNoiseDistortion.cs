using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralGeneration/PerlinNoiseDistortion")]
public class PerlinNoiseDistortion : ScriptableObject, INoiseGenerator
{
    [Range(0.00001f, 2)]
    public float alphaAmplitude;
    [Range(1, 16)]
    public int octaves;
    [Range(1, 400)]
    public float amplitude;
    [Range(0, 128)]
    public int OffsetX;
    [Range(0, 128)]
    public int OffsetY;
    [Range(1, 100)]
    public int step;

    //public float lacunarity = 2.0f;
    // public float persistence = 0.5f;
    System.Random r = new System.Random();

    Vector2 seedX;
    Vector2 seedY;
    Vector2 seedA;

    public PerlinNoise perlin;
    float[] PerlinNoise;
    public int PerlinSize => NoiseData.PerlinSize;


    /*private void Awake()
    {
        PerlinNoise = perlin.GenerateNoiseMap(PerlinSize, 1);
        MapWindow.Perlin = PerlinNoise;
    }

    private void OnEnable()
    {
        PerlinNoise = perlin.GenerateNoiseMap(PerlinSize, 1);
        MapWindow.Perlin = PerlinNoise;
    }*/

    
    public float[] Init()
    {
        PerlinNoise = NoiseData.GetPerlinNoise();
        return PerlinNoise;
    }


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
        var genes = map.GetRawData();
        //Debug.Log(PerlinNoise.Length);
        
        return Smooth(DistortingNoise(genes, map.N, map.N),map.N,map.N);
    }

    public float[] GenerateNoiseMap(float[] map, int width, int height)
    {
        //Debug.Log(PerlinNoise.Length);

        return Smooth(DistortingNoise(map, width, height), width, height);
    }


    public float[] DistortingNoise(float[] inputData, int width, int height)
    {
        System.Random r = new System.Random();
        float[] outputData = new float[inputData.Length];

        //posición primer plot de perlinNoise
        int x1 = (int)(r.NextDouble()*(PerlinSize/2));
        int y1 = (int)(r.NextDouble() * (PerlinSize/2));

        //posición segundo plot de perlinNoise
        int x2 = (int)(r.NextDouble() * (PerlinSize - width));
        int y2 = (int)(r.NextDouble() * (PerlinSize - height));

        //tercerPlot
        int x3 = (int)(r.NextDouble() * (PerlinSize - width));
        int y3 = (int)(r.NextDouble() * (PerlinSize - height));

        //int c = 0;


        //
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < height; i++)
            {
                int dx = (int)(amplitude * (PerlinNoise[(y1 + j) * width + (x1 + i)] - 0.5f));
                int dy = (int)(amplitude * (PerlinNoise[(y2 + j) * width + (x2 + i)] - 0.5f));
                float da = Mathf.Clamp01(alphaAmplitude * (PerlinNoise[(y3 + j) * width + (x3 + i)] - 0.5f));

                dx = Mathf.Clamp(dx + i, 0, width - 1);
                dy = Mathf.Clamp(dy + j, 0, height - 1);
                //if (dy > 0) c++;
                //int index = Mathf.Clamp(((dy) * width + (dx)), 0, width * height);

                outputData[j * width + i] = inputData[dy*width + dx] + da;
            }
        }

        //Debug.Log(c);
        return outputData;
    }


    public float[] Smooth(float[] inputData, int width, int height)
    {
        

        float[] outputData = new float[inputData.Length];
        int[] dirs = { 1, width + 1, width, width - 1, -1, -width - 1, -width, -width + 1 };

        float inputMax = 0;
        float outputMax = 0;

        for (int l = 0; l < octaves; l++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    float val = inputData[j * width + i];
                    if (val > inputMax)
                    {
                        inputMax = val;
                    }
                    int dots = 1;
                    foreach (int k in dirs)
                    {
                        if ((j * width + i) + k < 0 || (j * width + i) + k > inputData.Length - 1)
                        {
                            continue;
                        }
                        dots++;
                        val += inputData[(j * width + i) + k];
                    }
                    outputData[j * width + i] = ((val / dots));
                    if (l == octaves - 1 && outputData[j * width + i] > outputMax)
                    {
                        outputMax = outputData[j * width + i];
                    }
                }
            }
            inputData = outputData;
        }

        float scale = inputMax / outputMax;

        for(int i = 0; i < outputData.Length; i++)
        {
            outputData[i] = ((int)((outputData[i] * scale) * step)) / (step * 1.0f);
        }

        return outputData;
    }
}
