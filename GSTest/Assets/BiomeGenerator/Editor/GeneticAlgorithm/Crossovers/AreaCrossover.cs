using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Chromosomes;
using System.Linq;

public class AreaCrossover : CrossoverBase
{
    float Scale;

    public AreaCrossover(float scale) : base(2, 2)
    {
        Scale = scale;
    }

    protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
    {
        var c1 = (parents[0] as LayerChromosome).Clone() as LayerChromosome;
        var c2 = (parents[1] as LayerChromosome).Clone() as LayerChromosome;

        var genes1 = c1.GetRawData();
        var genes2 = c2.GetRawData();
        
        System.Random r = new System.Random();

        int sN = (int)(c1.N * Scale);
        
        int x = r.Next(0, c1.N - sN);
        int y = r.Next(0, c1.N - sN);

        float dist = sN / 2.0f;

        int xC = x + (int)dist;
        int yC = y + (int)dist;


        int mult = r.NextDouble() > 0.5f ? 1 : -1;

        for (int j = 0; j < sN; j++)
        {
            for (int i = 0; i < sN; i++)
            {
                float pond = Mathf.Clamp01(MathTools.Distance(xC, yC, x + i, y + j) / dist);
                float val1 = (genes1[(y + j) * c1.N + (x + i)] * pond) + (mult * genes2[(y + j) * c2.N + (x + i)] * (1 - pond));
                float val2 = (genes2[(y + j) * c1.N + (x + i)] * pond) + (mult * genes1[(y + j) * c2.N + (x + i)] * (1 - pond));
                
                c1.SetGene((x + i), (y + j), val1);
                c2.SetGene((x + i), (y + j), val2);
            }
        }

        return new IChromosome[] { c1, c2 };
    }

}
