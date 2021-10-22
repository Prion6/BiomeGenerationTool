using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using UnityEngine;

[CreateAssetMenu(menuName = "BiomeGenerator/Stat Deprivation Checker")]
public class StatDeprivationChecker : StatGainChecker
{
    public override double Evaluate(IChromosome chromosome)
    {
        return 1 - base.Evaluate(chromosome);
    }
}
