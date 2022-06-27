using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortMutation : MutationBase
{

    float Scale;

    public DistortMutation(float scale)
    {
        Scale = scale;
    }

    protected override void PerformMutate(IChromosome chromosome, float probability)
    {
        LayerChromosome c = chromosome as LayerChromosome;
        
        System.Random r = new System.Random();


        int sN = (int)(c.N * Scale);
        float[] genes = c.GetRawData();
        float[] data = new float[sN * sN];

        int mult = r.NextDouble() > 0.5f ? 1 : -1;

        int x = r.Next(0, c.N - sN);
        int y = r.Next(0, c.N - sN);

        for (int j = 0; j < sN; j++)
        {
            for (int i = 0; i < sN; i++)
            {
                data[j * sN + i] = genes[j * sN + i];
            }
        }

        c.Layer.noiseGenerator.GenerateNoiseMap(data, sN, sN);


        float dist = sN / 2.0f;

        int xC = x + (int)dist;
        int yC = y + (int)dist;

        for (int j = 0; j < sN; j++)
        {
            for (int i = 0; i < sN; i++)
            {
                float pond = Mathf.Clamp01(MathTools.Distance(xC, yC, x + i, y + j) / dist);
                if (UseData.IsDummy)
                {
                    pond = 1 - pond;
                }
                float val = (genes[(y + j) * c.N + (x + i)] * pond) + (mult*data[j * sN + i] * (1 - pond));
                c.SetGene((x + i), (y + j), val);
            }
        }
    }
}
