using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "BiomeGenerator/Evaluation Function/Conectivity")]
public class Conectivity : BaseFitnessFunction
{
    public string colliderStat;

    public override double Evaluate(IChromosome chromosome)
    {
        LayerChromosome c = chromosome as LayerChromosome;
        bool hasCollider = false;
        foreach(MapElement m in c.Layer.mapElements)
        {
            foreach (BaseStat s in m.stats)
            {
                if (s.label.Equals(colliderStat))
                {
                    hasCollider = true;
                    break;
                }
            }
            if (hasCollider) break;
        }

        if(!hasCollider)
        {
            return 1;
        }

        bool[] Closed = new bool[chromosome.Length];

        float val = 0;
        int breaks = 0;

        if (MapWindow.StartPoints.Count == 0)
        {
            return val;
        }
        foreach(Tuple<int,int> t in MapWindow.StartPoints)
        {
            if(Closed[t.Item2 * c.N + t.Item1])
            {
                continue;
            }
            breaks++;
            val += FloodFIll.NodeCount(t, (chromosome as LayerChromosome).GetRawData(), (chromosome as LayerChromosome).N, Closed,0,0);
        }

        return (val/chromosome.Length*1.0f)*(1 - breaks*1.0f/ MapWindow.StartPoints.Count);
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
