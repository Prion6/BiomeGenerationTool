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
            return 1;
        }

        if (MapWindow.StartPoints.Count == 0)
        {
            return 1;
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

        return (avg / max) * (1 - breaks * 1.0f / MapWindow.StartPoints.Count);
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
