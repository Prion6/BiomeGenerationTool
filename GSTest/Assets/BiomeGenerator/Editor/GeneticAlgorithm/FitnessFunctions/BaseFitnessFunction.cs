using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;
using UnityEditor;

[System.Serializable]
public class BaseFitnessFunction : ScriptableObject, IFitness
{
    //public string label;

    protected BaseFitnessFunction()
    {
    }

    public virtual double Evaluate(IChromosome chromosome)
    {
        throw new System.NotImplementedException();
    }



    public override bool Equals(object other)
    {
        if (other == null) return false;
        if (other as BaseFitnessFunction == null) return false;
        return base.Equals(other) || name.Equals((other as BaseFitnessFunction).name);
    }

    public override int GetHashCode()
    {
        var hashCode = 466152083;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
        return hashCode;
    }
}
