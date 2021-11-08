using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Chromosomes;

public class DistortionMutation : MutationBase
{
    INoiseGenerator NoiseGenerator;
    float Scale;

    public DistortionMutation(INoiseGenerator noiseGenerator, float scale)
    {
        NoiseGenerator = noiseGenerator;
        Scale = scale;
    }

    protected override void PerformMutate(IChromosome chromosome, float probability)
    {
        LayerChromosome c = chromosome as LayerChromosome;
        if (c == null) return;
        System.Random r = new System.Random();

        int sN = (int)(c.N * Scale);
        float[] genes = c.ToFloatingPoints();
        float[] data = new float[sN * sN];

        int x = r.Next(0, c.N - sN);
        int y = r.Next(0, c.N - sN);

        for(int j = 0; j < sN; j++)
        {
            for (int i = 0; i < sN; i++)
            {
                data[j * sN + i] = genes[(y + j) * c.N + (x + i)];
            }
        }

        data = NoiseGenerator.GenerateNoiseMap(data, sN, sN);

        for (int j = 0; j < sN; j++)
        {
            for (int i = 0; i < sN; i++)
            {
                genes[(y + j) * c.N + (x + i)] = data[j * sN + i];
            }
        }
    }
}
