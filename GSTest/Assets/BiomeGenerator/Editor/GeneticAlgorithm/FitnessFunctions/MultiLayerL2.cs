using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "BiomeGenerator/Evaluation Function/MultiLayerL2")]
public class MultiLayerL2 : BaseFitnessFunction
{
    public BaseFitnessFunction FitnessFunction;

    public override double Evaluate(IChromosome chromosome)
    {
        LayerChromosome[] chromosomes = MapWindow.layers.Select((l) => l.Controller.BaseChromosome).ToArray();
        float val = 0;
        foreach (LayerChromosome c in chromosomes)
        {
            val += Mathf.Pow((float)FitnessFunction.Evaluate(chromosome), 2); //L2
        }
        return val / chromosomes.Length;
    }
}
