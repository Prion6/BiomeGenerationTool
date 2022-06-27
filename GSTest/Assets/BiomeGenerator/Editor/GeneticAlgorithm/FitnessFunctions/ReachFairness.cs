using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "BiomeGenerator/Evaluation Function/ReachFairness")]
public class ReachFairness : BaseFitnessFunction
{
    public string colliderStat;

    public override double Evaluate(IChromosome chromosome)
    {
        LayerChromosome c = chromosome as LayerChromosome;
        float[] genes = c.GetRawData();
        bool hasCollider = false;

        float p = 1;

        foreach (MapElement m in c.Layer.mapElements)
        {
            if (m == null) continue;
            foreach (BaseStat s in m.stats)
            {
                if (s == null) continue;
                if (s.label.Equals(colliderStat))
                {
                    hasCollider = true;
                    break;
                }
            }
            if (hasCollider) break;
        }

        if (!hasCollider)
        {
            if (UseData.IsDummy)
            {
                return 1 - p;

            }
            return p;
        }

        if (MapWindow.StartPoints.Count == 0)
        {
            if (UseData.IsDummy)
            {
                return 1 - p;

            }
            return p;
        }

        int[] dists = new int[MapWindow.StartPoints.Count];
        int i = 0;
        int breaks = 0;
        foreach (Tuple<int, int> t in MapWindow.StartPoints)
        {
            int dist = 0;
            foreach (Tuple<int, int> t2 in MapWindow.StartPoints)
            {
                if (t.Equals(t2)) continue;
                var path = PathFind.Astar(genes, c.N, (t.Item2*c.N+t.Item1), (t2.Item2 * c.N + t2.Item1));
                if(path == null)
                {
                    breaks++;
                    continue;
                }
                dist += path.Length;

            }
            dists[i] = dist;
            i++;
        }

        float avg = dists.Sum() / dists.Length;
        int max = dists.Max();
        int n = MapWindow.StartPoints.Count * (MapWindow.StartPoints.Count - 1) / 2;

        p = (avg / max) * (1 - breaks * 1.0f / n);

        p = p > float.NaN ? 0 : p;

        if (UseData.IsDummy)
        {
            return 1 - p;

        }
        return p;
    }
}
