using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using UnityEngine;

[CreateAssetMenu(menuName = "BiomeGenerator/Evaluation Function/Shelter")]
public class Shelter : BaseFitnessFunction
{
    [Range(0,1)]
    public float Area;

    public override double Evaluate(IChromosome chromosome)
    {
        throw new System.NotImplementedException();
    }
}
