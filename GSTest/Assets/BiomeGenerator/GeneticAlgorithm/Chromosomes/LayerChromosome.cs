using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticSharp.Domain.Chromosomes;
using System;
using System.Linq;

public class LayerChromosome : FloatingPointChromosome
{
    public LayerController Layer;
    public int N { get; private set; }
    

    public LayerChromosome(int size, LayerController layer) : base(0,1,size,3)
    {
        N = (int)MathTools.Q_sqrt(size);
        for(int i = 0; i < size; i ++)
        {
            ReplaceGene(i,new Gene(0));
        }
        Layer = layer;
    }

    public LayerChromosome(float[] Genes, LayerController layer) : base(0, 1, Genes.Length, 3)
    {
        N = (int)MathTools.Q_sqrt(Genes.Length);
        for (int i = 0; i < Genes.Length; i++)
        {
            ReplaceGene(i, new Gene(Genes[i]));
        }
        Layer = layer;
    }

    public LayerChromosome(Texture2D texture, LayerController layer) : base(0, 1, texture.width*texture.height, 3)
    {
        N = texture.width;
        for (int i = 0; i < texture.width * texture.height; i++)
        {
            ReplaceGene(i, new Gene(texture.GetPixel(i%texture.height, i/texture.height).a));
        }
        Layer = layer;
    }

    public LayerChromosome(Texture2D texture, int size, LayerController layer) : base(0, 1, texture.width * texture.height, 3)
    {
        //Debug.Log("Constructor");
        N = (int)MathTools.Q_sqrt(size);
        var genes = MathTools.Resize(texture.GetPixels(),texture.width,texture.height,N,N);
        for (int i = 0; i < genes.Length; i++)
        {
            ReplaceGene(i, new Gene(genes[i].a));
        }
        Layer = layer;
    }

    public override IChromosome CreateNew()
    {
        LayerChromosome c = new LayerChromosome(Layer.noiseGenerator.GenerateNoiseMap(this), Layer);
        return c;
    }

    public override Gene GenerateGene(int geneIndex)
    {
        return new Gene(new System.Random().NextDouble());
    }  
    
    public override IChromosome Clone()
    {
        var clone = base.Clone() as LayerChromosome;
        clone.N = N;
        clone.Layer = Layer;
        return clone;
    }

    public void Paint(Texture2D texture, Color color)
    {
        Color[] pixels = new Color[N * N]; 

        var genes = ToFloatingPoints();

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Color c = new Color(color.r, color.g, color.b, genes[i*N + j]);
                //Color c = new Color(color.r, color.g, color.b, float.Parse(GetGene(i * N + j).Value.ToString()));

                pixels[i * N + j] = c;
            }
        }

        if (texture.width != N || texture.height != N)
        {
            Debug.Log("Resizing");
            //Using Graphuc class causes Editor Window to Crash
            texture.SetPixels(MathTools.Resize(pixels,N,N,texture.width,texture.height));
            //texture = TextureScaler.scaled(text, texture.width, texture.height);
        }
        else
        {
            texture.SetPixels(pixels);
        }
        texture.Apply();
    }

    public new float[] ToFloatingPoints()
    {
        return GetGenes().AsParallel().Select((g) => float.Parse(g.Value.ToString())).ToArray();
    }

    public void SetGene(int x, int y, float val)
    {
        ReplaceGene(y * N + x, new Gene(val));
    }

}
