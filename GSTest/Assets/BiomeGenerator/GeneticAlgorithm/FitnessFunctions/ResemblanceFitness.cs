using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;
using System;

[CreateAssetMenu(menuName = "BiomeGenerator/Evaluation Function/Resemblance")]
public class ResemblanceFitness : BaseFitnessFunction
{
    [Range(0, 1)]
    public float Divergence;

    public override double Evaluate(IChromosome chromosome)
    {
        //var c = chromosome as LayerChromosome;

        float distance = 0;
        

        for (int x = 0; x < chromosome.Length; x++)
        {
            distance += Mathf.Abs(float.Parse(chromosome.GetGene(x).Value.ToString()) - float.Parse(MapWindow.selectedLayerChromosome.GetGene(x).Value.ToString()));
        }

        distance /= chromosome.Length;

        if (1 - distance < Divergence)
        {
            return 0;
        }
        return (1 - distance);
    }
}
