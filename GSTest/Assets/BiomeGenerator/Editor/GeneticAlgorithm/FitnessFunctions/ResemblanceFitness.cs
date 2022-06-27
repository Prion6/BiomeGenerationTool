using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;
using System;

[CreateAssetMenu(menuName = "BiomeGenerator/Evaluation Function/Resemblance")]
public class ResemblanceFitness : BaseFitnessFunction
{
    public override double Evaluate(IChromosome chromosome)
    {
        float[] src = (chromosome as LayerChromosome).GetRawData();
        float[] genes = (MapWindow.selectedLayerChromosome).GetRawData();
        //var c = chromosome as LayerChromosome;

        float distance = 0;

        for (int x = 0; x < chromosome.Length; x++)
        {
            distance += (genes[x] - src[x])* (genes[x] - src[x]);
        }

        distance /= chromosome.Length;

        if(UseData.IsDummy)
        {
            return distance;
        }
        return (1 - distance);
    }
}
