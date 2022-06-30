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
        float[] data = c.GetRawData();
        bool[] Closed = new bool[data.Length];

        bool hasStat = false;

        float p = 0;

        if(c.Layer.mapElements.Count == 0)
        {
            if (UseData.IsDummy)
            {
                return 1 - p;
            }
            return p;
        }

        foreach (MapElement m in c.Layer.mapElements)
        {
            foreach (BaseStat s in m.stats)
            {
                if (s.label.Equals(stat))
                {
                    hasStat = true;
                    break;
                }
            }
            if (hasStat) break;
        }

        if (!hasStat)
        {
            if (UseData.IsDummy)
            {
                return p;
            }
            return 1 - p;
        }

        if (MapWindow.StartPoints.Count == 0)
        {
            if (UseData.IsDummy)
            {
                return 1 - p;
            }
            return p;
        }

        float maxDistance = 0;
        float avgDist = 0;
        int counter = 0;
        List<float> vals = new List<float>();

        //realizando procesos de sobra  
        foreach (Tuple<int,int> t1 in MapWindow.StartPoints)
        {
            foreach (Tuple<int, int> t2 in MapWindow.StartPoints)
            {
                if(t1.Equals(t2))
                {
                    continue;
                }
                float dist = MathTools.Distance(t1.Item1, t1.Item2, t2.Item1, t2.Item2);
                avgDist += dist;
                counter++;
                if(dist > maxDistance)
                {
                    maxDistance = dist;
                }
            }
            int xL = (int)(t1.Item1 - maxDistance / 2 < 0 ? 0 : t1.Item1 - maxDistance / 2);
            int xR = (int)(t1.Item1 + maxDistance / 2 > c.N ? c.N : t1.Item1 + maxDistance / 2);
            int yB = (int)(t1.Item2 - maxDistance / 2 < 0 ? 0 : t1.Item2 - maxDistance / 2);
            int yU = (int)(t1.Item2 + maxDistance / 2 > c.N ? c.N : t1.Item2 + maxDistance / 2);
            vals.Add(FloodFIll.ValCount(t1, data, c.N, Closed, 0, 1, xL, xR, yB, yU));
        }
        if(counter == 0)
        {
            avgDist /= counter;
        }

        float max = 0;
        float avg = 0;

        foreach(float f in vals)
        {
            avg += f;
            if(f > max)
            {
                max = f;
            }
        }
        if(vals.Count != 0)
        {
            avg /= vals.Count;
        }
        if(max != 0 && maxDistance != 0)
        {
            p = (avg / max) * (avgDist / maxDistance);
        }

        p = p == float.NaN ? 0 : p;

        if(UseData.IsDummy)
        {
            return 1 - p;
        }
        return p;
    }

}
