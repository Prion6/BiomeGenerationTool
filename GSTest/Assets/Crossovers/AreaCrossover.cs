using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Chromosomes;

public class AreaCrossover : CrossoverBase
{
    readonly float CrossoverScale;

    public AreaCrossover(float crossoverScale) : base (2,2)
    {
        CrossoverScale = crossoverScale;
    }

    protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
    {
        System.Random r = new System.Random();
        int squareSide = (int)((parents[0] as LayerChromosome).N * CrossoverScale);

        //var parent1 = (parents[0] as LayerChromosome).ToFloatingPoints();
        //var parent2 = (parents[1] as LayerChromosome).ToFloatingPoints();

        var child1 = (parents[0].Clone()) as LayerChromosome;
        var child2 = (parents[1].Clone()) as LayerChromosome;

        int x = r.Next(0, child1.N - squareSide);
        int y = r.Next(0, child1.N - squareSide);

        int[] moves = { 1, child1.N + 1, child1.N, child1.N - 1, -1, -child1.N - 1, -child1.N, -child1.N + 1 };

        var data1 = new double[squareSide * squareSide];
        var data2 = new double[squareSide * squareSide];

        Vector2 center = new Vector2(x + (squareSide/2), y + (squareSide / 2));

        float diagonal = Vector2.Distance(Vector2.zero, new Vector2(squareSide,squareSide))/2;
         
        for (int j = y; j < y + squareSide; j++)
        {
            for(int i = x; i < x + squareSide; i++)
            {
                double val1 = double.Parse(parents[0].GetGene(j * child1.N + i).Value.ToString());
                double val2 = double.Parse(parents[1].GetGene(j * child1.N + i).Value.ToString());
                int dots1 = 1;
                int dots2 = 1;
                if (1 - MathTools.Distance(center.x, center.y, i,j)/diagonal < 0.25)
                {
                    data1[((j - y) * squareSide + (i - x))] = val1;
                    data2[((j - y) * squareSide + (i - x))] = val2;
                    continue;
                }
                //Agregar ponderación < 0.25 se mantiene el dato.
                
                /*foreach (int m in moves)
                {
                    if (j * child1.N + i + m < child1.Length && j * child1.N + i + m > 0)
                    {
                        val1 += double.Parse(parents[0].GetGene(j * child1.N + i + m).Value.ToString());
                        dots1++;
                        val2 += double.Parse(parents[1].GetGene(j * child1.N + i + m).Value.ToString());
                        dots2++;
                    }
                }*/
                val1 /= dots1;
                val2 /= dots2;
                data1[((j - y) * squareSide + (i - x))] = (val1 + double.Parse(parents[1].GetGene(j * child1.N + i).Value.ToString())) / 2;
                data2[((j - y) * squareSide + (i - x))] = (val2 + double.Parse(parents[0].GetGene(j * child1.N + i).Value.ToString())) / 2;
            }
        }

        for (int j = y; j < y + squareSide; j++)
        {
            for (int i = x; i < x + squareSide; i++)
            {
                child1.ReplaceGene(j * child1.N + i, new Gene(data1[((j - y) * squareSide + (i - x))]));
                child2.ReplaceGene(j * child2.N + i, new Gene(data2[((j - y) * squareSide + (i - x))]));
            }
        }

        var list = new List<IChromosome>();
        list.Add(child1);
        list.Add(child2);

        return list;
    }
}
