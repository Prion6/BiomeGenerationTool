using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoiseGenerator
{
    float[] GenerateNoiseMap(LayerChromosome map);
    float[] GenerateNoiseMap(float[] map, int width, int height);
}
