using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;
using System;

[CreateAssetMenu(menuName = "BiomeGenerator/Resemblance Fitness Function")]
public class ResemblanceFitness : BaseFitnessFunction
{
    public float Divergence;

    public ResemblanceFitness(float divergence)
    {
        Divergence = divergence;
    }

    public override double Evaluate(IChromosome chromosome)
    {
        /*var c = chromosome as LayerChromosome;

        float distance = 0;
        

        for (int x = 0; x < c.Length; x++)
        {
            distance += Mathf.Abs((float)Convert.ToDouble(c.GetGene(x).Value) - (float)Convert.ToDouble(MapWindow.selectedLayerChromosome.GetGene(x).Value));
        }

        distance /= c.Length;

        if (1 - distance > 1 - Divergence) return 0;
        return (1 - distance);*/
        return new System.Random().NextDouble();
    }
}
