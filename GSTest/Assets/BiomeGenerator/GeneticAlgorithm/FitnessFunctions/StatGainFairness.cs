using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "BiomeGenerator/Evaluation Function/Stat Gain Fairness")]
public class StatGainFairness : BaseFitnessFunction
{
    public string stat;

    public override double Evaluate(IChromosome chromosome)
    {
        LayerChromosome c = chromosome as LayerChromosome;
        float[] data = c.ToFloatingPoints();
        bool[] Closed = new bool[data.Length];

        float totalChance = 0;
        float acummulatedChance = 0;

        foreach (MapElement m in c.Layer.mapElements)
        {
            totalChance += m.priority;
            foreach (BaseStat s in m.stats)
            {
                if (s.label.Equals(stat))
                {
                    acummulatedChance += m.priority;
                    break;
                }
            }
        }
        float pixelChance = acummulatedChance / totalChance;

        if(pixelChance == float.NaN || pixelChance == 0)
        {
            return 1;
        }

        float minDistance = float.PositiveInfinity;
        List<float> vals = new List<float>();

        foreach (Tuple<int,int> t1 in MapWindow.StartPoints)
        {
            foreach (Tuple<int, int> t2 in MapWindow.StartPoints)
            {
                if(t1.Equals(t2))
                {
                    continue;
                }
                float dist = MathTools.Distance(t1.Item1, t1.Item2, t2.Item1, t2.Item2);
                if(dist < minDistance)
                {
                    minDistance = dist;
                }
            }
            int xB = (int)(t1.Item1 - minDistance / 2 < 0 ? 0 : t1.Item1 - minDistance / 2);
            int xU = (int)(t1.Item1 + minDistance / 2 > c.N ? 0 : t1.Item1 + minDistance / 2);
            int yB = (int)(t1.Item2 - minDistance / 2 < 0 ? 0 : t1.Item2 - minDistance / 2);
            int yU = (int)(t1.Item2 + minDistance / 2 > c.N ? 0 : t1.Item2 + minDistance / 2);
            vals.Add(FloodFIll.ValCount(t1, data, c.N, Closed, 0.05f, 1, xB, xU, yB, yU));
        }

        float max = 0;
        float min = float.PositiveInfinity;

        foreach(float f in vals)
        {
            if(f < min)
            {
                min = f;
            }
            if(f > max)
            {
                max = f;
            }
        }

        return 1 - ((max - min) / max);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
