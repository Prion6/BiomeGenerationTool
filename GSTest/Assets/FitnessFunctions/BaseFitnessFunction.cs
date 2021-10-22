using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;

[System.Serializable]
[CreateAssetMenu(menuName = "BiomeGenerator/FitnessFunction")]
public abstract class BaseFitnessFunction : ScriptableObject, IFitness
{
    public string label;

    protected BaseFitnessFunction()
    {
    }

    public abstract double Evaluate(IChromosome chromosome);



    public override bool Equals(object other)
    {
        if (other == null) return false;
        if (other as BaseFitnessFunction == null) return false;
        return base.Equals(other) || label.Equals((other as BaseFitnessFunction).label);
    }

    public override int GetHashCode()
    {
        var hashCode = 466152083;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(label);
        return hashCode;
    }
}
