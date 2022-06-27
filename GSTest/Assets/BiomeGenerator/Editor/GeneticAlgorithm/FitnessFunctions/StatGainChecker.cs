using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using UnityEngine;

[CreateAssetMenu(menuName = "BiomeGenerator/Evaluation Function/Stat Gain")]
public class StatGainChecker : BaseFitnessFunction
{   
    public string stat;

    public override double Evaluate(IChromosome chromosome)
    {
        LayerChromosome c = chromosome as LayerChromosome;
        float totalChance = 0;
        float acummulatedChance = 0;

        float p = 0;

        if(c.Layer.mapElements.Count == 0)
        {
            if (UseData.IsDummy)
            {
                return 1 - p;

            }
            return p;
        }

        foreach(MapElement m in c.Layer.mapElements)
        {
            totalChance += m.priority;
            foreach(BaseStat s in m.stats)
            {
                if (s.label.Equals(stat))
                {
                    acummulatedChance += m.priority;
                    break;
                }
            }
        }

        if(acummulatedChance == 0)
        {
            if (UseData.IsDummy)
            {
                return 1 - p;

            }
            return p;
        }

        float pixelChance = acummulatedChance / totalChance;

        if (pixelChance == float.NaN || pixelChance == 0)
        {
            if (UseData.IsDummy)
            {
                return 1 - p;

            }
            return p;
        }

        float acummulatedPixelVal = 0;

        var arr = c.GetRawData();

        for(int i = 0; i < c.Length; i++)
        {
            acummulatedPixelVal += arr[i];
        }

        p = (pixelChance * acummulatedPixelVal) / (c.Length * 1.0f);
        p = (p == float.NaN) ? 0 : p;

        if (UseData.IsDummy)
        {
            return 1 - p;

        }
        return p;

    }
}
