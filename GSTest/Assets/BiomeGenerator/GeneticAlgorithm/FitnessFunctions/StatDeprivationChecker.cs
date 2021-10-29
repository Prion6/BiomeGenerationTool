using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using UnityEngine;

[CreateAssetMenu(menuName = "BiomeGenerator/Evaluation Function/Stat Deprivation")]
public class StatDeprivationChecker : StatGainChecker
{
    public override double Evaluate(IChromosome chromosome)
    {
        return 1 - base.Evaluate(chromosome);
    }
}
