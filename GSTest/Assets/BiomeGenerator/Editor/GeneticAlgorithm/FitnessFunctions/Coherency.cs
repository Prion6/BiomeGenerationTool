using System.Collections;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using UnityEngine;
using System.Linq;

[System.Serializable]
[CreateAssetMenu(menuName = "BiomeGenerator/Evaluation Function/Coherency")]
public class Coherency : BaseFitnessFunction
{
    public override double Evaluate(IChromosome chromosome)
    {
        LayerChromosome c = chromosome as LayerChromosome;
        float[] genes = c.GetRawData();
        LayerChromosome[] layerChromosomes = MapWindow.layers.Select((l) => l.Controller.BaseChromosome).ToArray();
        float incoherency = 0;

        foreach(MapElement m in c.Layer.mapElements)
        {
            float chance = c.Layer.NormalizedPriority(m);
            float mIncoherency = 0;

            foreach(Requirement r in m.requirements)
            {
                float rIncoherency = float.PositiveInfinity;
                foreach (LayerChromosome lc in layerChromosomes)
                {
                    float lcIncoherency = 0;
                    string[] stats = lc.Layer.mapElements.SelectMany((me) => me.stats.Select((s) => s.label)).ToArray();
                    int count = genes.Where((f) => f > 0).Count();
                    if (count == 0) continue;

                    if(!stats.Contains(r.label))
                    {
                        lcIncoherency = r.lack ? 0 : (genes.Sum()/ genes.Length);
                    }
                    else
                    {
                        float[] oponentGenes = lc.GetRawData();

                        for(int i = 0; i < genes.Length; i++)
                        {
                            if (genes[i] == 0) continue;
                            if((oponentGenes[i] > 0) == r.lack)
                            {
                                lcIncoherency += genes[i];
                            }
                        }
                        lcIncoherency /= genes.Length;
                    }

                    if(rIncoherency > lcIncoherency)
                    {
                        rIncoherency = lcIncoherency;
                    }
                }
                mIncoherency += rIncoherency;
            }
            incoherency += (mIncoherency/m.requirements.Count)*chance;
        }
        /*if(incoherency < 0)
        {
            Debug.Log("WHAT?: " + incoherency);
        }*/
        return 2*(0.5 - incoherency);
        //return (1.0f/(1.0f + incoherency));
    }
}
