using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Infrastructure.Framework.Threading;
using System.Threading;
using System.Linq;

[System.Serializable]
public class LayerController : ScriptableObject
{
    //LAYER ATTRIBUTES
    public string Label;
    [Space]
    public Color Color;
    [Space]
    public int DrawOrder;
    [Space]

    [SerializeField]
    public List<MapElement> mapElements;
    


    //EVOLUTIVE ALGORITHM ATTRIBUTES
    public bool IsRunning { get; set; }
    public int EvaluatedStats { get; set; }
    public float LifeCycle { get; set; }
    public float Divergence { get; set; }

    public LayerChromosome BaseChromosome { get; set; }
    
    public List<RunningGa> GeneticAlgorithms;

    public INoiseGenerator noiseGenerator;

    

    // Start is called before the first frame update
    public void Init(int width, int height, string name)
    {
        System.Random r = new System.Random();
        Label = name;
        this.name = name;
        Color = new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), 1);
        mapElements = new List<MapElement>();

        noiseGenerator = Resources.Load("Distorter") as PerlinNoiseDistortion;
        LifeCycle = 3;
        
        BaseChromosome = new LayerChromosome(width * height, this);

        GeneticAlgorithms = new List<RunningGa>();
        //gaThread.Start();
    }

    public RunningGa CreateNewGA(BaseFitnessFunction fitnessFunction)
    {
        return new RunningGa(null, null, fitnessFunction);
    }

    public void AddGA(BaseFitnessFunction fitnessFunction)
    {
        GeneticAlgorithms.Add(CreateNewGA(fitnessFunction));
    }

    public void RemoveGA(int index)
    {
        if (index > 0 && index < GeneticAlgorithms.Count)
            if (GeneticAlgorithms[index] != null)
            {
                if (GeneticAlgorithms[index].GA != null && !GeneticAlgorithms[index].GA.IsRunning)
                    if (GeneticAlgorithms[index].Thread != null && GeneticAlgorithms[index].Thread.IsAlive)
                        GeneticAlgorithms[index].Thread.Abort();
            }  
        GeneticAlgorithms.RemoveAt(index);
    }

    public void Stop()
    {
        for (int i = 0; i < GeneticAlgorithms.Count; i++)
        {
            if (GeneticAlgorithms[i] != null)
            {
                if (GeneticAlgorithms[i].Thread != null && GeneticAlgorithms[i].Thread.IsAlive)
                    GeneticAlgorithms[i].Thread.Abort();
            }
        }
    }

    public void Restart(Texture2D texture)
    {
        BaseChromosome = new LayerChromosome(texture, (int)(MapWindow.chromosomeSize.x*MapWindow.chromosomeSize.y), this);
        for (int i = 0; i < GeneticAlgorithms.Count; i++)
        {
            if (GeneticAlgorithms[i] != null)
            {
                GeneticAlgorithms[i].Init(BaseChromosome,LifeCycle);
            }
        }
    }

    public LayerChromosome DisplayOptions(int index)
    {
        if (index < 0 || index >= GeneticAlgorithms.Count) return null;
        return GeneticAlgorithms[index].GA.BestChromosome as LayerChromosome;
    }

    private void OnDestroy()
    {
        Stop();
    }

    public bool Waiting()
    {
        foreach(RunningGa ga in GeneticAlgorithms)
        {
            if (ga.GA == null) continue;
            if (ga.GA.IsRunning)
                return false;
        }
        return true;
    }
}

[System.Serializable]
public class RunningGa
{
    [HideInInspector]
    public GeneticAlgorithm GA;
    [HideInInspector]
    public Thread Thread;

    public BaseFitnessFunction FitnessFunction;

    public RunningGa(GeneticAlgorithm ga, Thread thread, BaseFitnessFunction fitnessFunction)
    {
        GA = ga;
        Thread = thread;
        FitnessFunction = fitnessFunction;
    }

    public void Init(IChromosome BaseChromosome, float LifeCycle)
    {
        if (FitnessFunction == null) return;
        if (Thread != null && Thread.IsAlive)
            Thread.Abort();
        
        var crossover = new AreaCrossover(0.03125f);
        var mutation = new InsertionMutation();
        var selection = new EliteSelection();
        var population = new Population(5, 10, BaseChromosome);

        GA = new GeneticAlgorithm(population, FitnessFunction, selection, crossover, mutation);
        //GA.CrossoverProbability = 1;

        //GA.Termination = new GenerationNumberTermination(1);
        //GA.Start();

        //GA.Termination = new TimeEvolvingTermination(System.TimeSpan.FromSeconds(LifeCycle));
        GA.Termination = new GenerationNumberTermination(5);
        GA.TaskExecutor = new ParallelTaskExecutor
        {
            MinThreads = 100,
            MaxThreads = 200
        };
        //int i = 0;
        //GA.GenerationRan += delegate { i++;  Debug.Log(i); };

        //GA.Start();
        Thread = new Thread(() => { GA.Start();
        });
        
        Thread.Start();

    }
}
