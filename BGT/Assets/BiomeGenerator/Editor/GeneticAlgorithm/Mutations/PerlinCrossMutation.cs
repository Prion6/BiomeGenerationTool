using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Chromosomes;

public class PerlinCrossMutation : MutationBase
{
    float Scale;

    public PerlinCrossMutation(float scale)
    {
        Scale = scale;
    }

    protected override void PerformMutate(IChromosome chromosome, float probability)
    {
        LayerChromosome c = chromosome as LayerChromosome;
        if (c == null) return;
        System.Random r = new System.Random();

        int mult = r.NextDouble() > 0.5f ? 1 : -1;

        int sN = (int)(c.N * Scale);
        float[] genes = c.GetRawData();
        float[] data = new float[sN * sN];

        int x = r.Next(0, c.N - sN);
        int y = r.Next(0, c.N - sN);

        int n = (int)MathTools.Q_sqrt(MapWindow.Perlin.Length);

        int xp = r.Next(0, n - sN);
        int yp = r.Next(0, n - sN);

        for (int j = 0; j < sN; j++)
        {
            for (int i = 0; i < sN; i++)
            {
                data[j * sN + i] = MapWindow.Perlin[(yp + j) * n + (xp + i)];
            }
        }

        float dist = sN / 2.0f;

        int xC = x + (int)dist;
        int yC = y + (int)dist;

        for (int j = 0; j < sN; j++)
        {
            for (int i = 0; i < sN; i++)
            {
                float pond = Mathf.Clamp01(MathTools.Distance(xC, yC, x+i, y+j)/dist);
                float val = (genes[(y + j) * c.N + (x + i)] * pond) + (mult*data[j * sN + i]*(1-pond));
                c.SetGene((x + i), (y + j), val);
            }
        }
    }
}
