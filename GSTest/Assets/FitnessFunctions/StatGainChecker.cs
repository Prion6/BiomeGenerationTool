﻿using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using UnityEngine;

[CreateAssetMenu(menuName = "BiomeGenerator/Stat Gain Checker")]
public class StatGainChecker : BaseFitnessFunction
{
    public string stat;

    public override double Evaluate(IChromosome chromosome)
    {
        LayerChromosome c = chromosome as LayerChromosome;
        float totalChance = 0;
        float acummulatedChance = 0;

        foreach(MapElement m in c.Layer.mapElements)
        {
            totalChance += m.priority;
            foreach(BaseStat s in m.stats)
            {
                if (s.label.Equals(stat))
                    acummulatedChance += m.priority;
            }
        }

        float pixelChance = acummulatedChance / totalChance;

        float acummulatedPixelVal = 0;

        var arr = c.ToFloatingPoints();

        for(int i = 0; i < c.Length; i++)
        {
            acummulatedPixelVal += arr[i];
        }

        return pixelChance * acummulatedPixelVal / c.Length;
    }
}
