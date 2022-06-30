using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using UnityEngine;

[CreateAssetMenu(menuName = "BiomeGenerator/Evaluation Function/MultiEvaluationL2")]
public class MultiEvaluationL2 : BaseFitnessFunction
{
    public List<BaseFitnessFunction> fitnessFunctions;

    public override double Evaluate(IChromosome chromosome)
    {
        float val = 0;
        foreach (BaseFitnessFunction f in fitnessFunctions)
        {
            val += Mathf.Pow((float)f.Evaluate(chromosome), 2); //L2
        }
        return val / fitnessFunctions.Count;
    }
}
