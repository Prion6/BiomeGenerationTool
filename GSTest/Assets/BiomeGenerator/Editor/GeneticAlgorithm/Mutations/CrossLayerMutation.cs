using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossLayerMutation : MutationBase
{
    float Scale;

    public CrossLayerMutation(float scale)
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

        LayerChromosome chrom = MapWindow.layers[r.Next(0, MapWindow.layers.Count)].Controller.BaseChromosome;
        if(chrom == null)
        {
            //Debug.Log("return");
            return;
        }

        float[] cgenes = chrom.GetRawData();


        if (chrom.Layer.Label.Equals(c.Layer.Label))
        {
            //Debug.Log("return chrom");
            return;
        }

        for (int j = 0; j < sN; j++)
        {
            for (int i = 0; i < sN; i++)
            {
                data[j * sN + i] = cgenes[j * sN + i];
            }
        }

        float dist = sN / 2.0f;

        int xC = x + (int)dist;
        int yC = y + (int)dist;

        for (int j = 0; j < sN; j++)
        {
            for (int i = 0; i < sN; i++)
            {
                float pond = Mathf.Clamp01(MathTools.Distance(xC, yC, x + i, y + j) / dist);
                float val = (genes[(y + j) * c.N + (x + i)] * pond) + (mult * data[j * sN + i] * (1 - pond));
                c.SetGene((x + i), (y + j), val);
            }
        }
    }
}
