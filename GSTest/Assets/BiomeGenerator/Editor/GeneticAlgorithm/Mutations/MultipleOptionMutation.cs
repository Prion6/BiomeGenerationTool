using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleOptionMutation : MutationBase
{
    CrossLayerMutation crm;
    DistortMutation dm;
    PerlinCrossMutation pcm;

    public MultipleOptionMutation(float scale)
    {
        crm = new CrossLayerMutation(scale);
        dm = new DistortMutation(scale);
        pcm = new PerlinCrossMutation(scale);
    }

    protected override void PerformMutate(IChromosome chromosome, float probability)
    {
        System.Random r = new System.Random();

        int i = (int)(r.NextDouble()*3);

        switch(i)
        {
            case 0: dm.Mutate(chromosome, probability); break;
            case 1: crm.Mutate(chromosome, probability); break;
            case 2: pcm.Mutate(chromosome, probability); break;
            default: pcm.Mutate(chromosome, probability); break;
        }
    }
}
