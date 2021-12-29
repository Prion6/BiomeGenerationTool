﻿using System.Collections;
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
            return 1;
        }

        if (MapWindow.StartPoints.Count == 0)
        {
            return 0;
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
        avgDist /= counter;

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

        avg /= vals.Count;

        return (avg / max)*(avgDist/maxDistance);
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