using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using System;
using System.Linq;
using System.Diagnostics;

public class DisplayOptionsView : EditorPanel
{
    public PaintView SelectedLayer { get; set; }
    PaintView prevLayer;
    public List<Tuple<string, LayerChromosome>> BestChromosomes;
    public List<Texture2D> BtnTextures;

    bool isRunning = false;
    bool IsRunning {
        get
        {
            return isRunning;
        }
        set {
            if (isRunning && !value && SelectedLayer != null)
            {
                SelectedLayer.Controller.Stop();
            }
            else if(!isRunning && value && SelectedLayer != null)
            {
                ResetCycle();
            }
            isRunning = value;
            UseData.IsRunning(isRunning);
        } }
    Stopwatch clock;

    bool pause;

    Vector2 scrollPosition;
    

    public DisplayOptionsView(int x, int y, int width, int height) : base(x, y, width, height)
    {
        BestChromosomes = new List<Tuple<string,LayerChromosome>>();
        clock = new Stopwatch();
        scrollPosition = Vector2.zero;
        BtnTextures = new List<Texture2D>();
        prevLayer = SelectedLayer;
        for(int i = 0; i < LayerController.maxGA; i++)
        {
            BtnTextures.Add(new Texture2D(128,128));
        }
    }

    void CleanTextures()
    {
        for(int i = 0; i < BtnTextures.Count; i++)
        {
            BtnTextures[i] = new Texture2D(128, 128);
            BtnTextures[i].Apply();
        }
    }

    //TODO EL LAG SPIKE ESTA AQUÍ
    ///Opciones para resolver:
    ///- Optimizar código: ideal pero complicado
    ///- Dividir en thread?
    ///- Dividir el proceso en Selectedlayer.Controller.geneticAlgorithms.Count frames
    public void Update()
    {
        if (SelectedLayer != null && SelectedLayer.Controller.GeneticAlgorithms.Count > 0)
        {
            if(isRunning && !pause)
            {
                if(!SelectedLayer.Equals(prevLayer))
                {
                    isRunning = false;
                    CleanTextures();
                    prevLayer.Controller.Stop();
                    BestChromosomes.Clear();
                    for (int i = 0; i < BtnTextures.Count; i++)
                    {
                        SelectedLayer.Controller.BaseChromosome.Paint(BtnTextures[i], SelectedLayer.Controller.Color);
                    }
                    ResetCycle();
                }
                else if (SelectedLayer.Controller.Waiting())
                {
                    BestChromosomes.Clear();
                    foreach (RunningGa ga in SelectedLayer.Controller.GeneticAlgorithms)
                    {
                        if (ga.GA == null) continue;
                        //if (ga.FitnessFunction == null) continue;
                        if (ga.GA.BestChromosome == null) continue;
                        BestChromosomes.Add(Tuple.Create(ga.FitnessFunction.name, ga.GA.BestChromosome as LayerChromosome));
                    }
                    for (int i = 0; i < BestChromosomes.Count; i++)
                    {
                        BestChromosomes[i].Item2.Paint(BtnTextures[i], SelectedLayer.Controller.Color);
                    }
                    ResetCycle();
                }
            }
        }
        prevLayer = SelectedLayer;
        //UnityEngine.Debug.Log(timer.ElapsedMilliseconds/1000.0f);
    }

    public void SetSelectedLayer(PaintView p)
    {
        SelectedLayer = p;
        ResetCycle();
    }

    public override void Draw()
    {
        GUI.DrawTexture(Rect, Texture, ScaleMode.StretchToFill);

        GUILayout.BeginArea(new Rect(Rect.x+MapWindow.padding,Rect.y+ MapWindow.padding, Rect.width- MapWindow.padding*2, Rect.height- MapWindow.padding*2));

        GUILayout.Label("Display Options Setion");
        
        //IsRunning = EditorGUILayout.Toggle("Running",IsRunning);
        EditorGUILayout.Space();

        //UnityEngine.Debug.Log(IsRunning);
        if (SelectedLayer != null)
        {
            if(isRunning)
            {
                GUILayout.Label("Clock: " + (clock.ElapsedMilliseconds / 1000));
                EditorGUILayout.Space();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                for (int i = 0; i < SelectedLayer.Controller.GeneticAlgorithms.Count; i += 2)
                {
                    EditorGUILayout.BeginHorizontal();

                    #region First Button
                    /*if (i < BestChromosomes.Count)
                    {
                        BestChromosomes[i].Item2.Paint(BtnTextures[i], SelectedLayer.Controller.Color);
                    }*/

                    EditorGUILayout.BeginVertical();
                    if (GUILayout.Button(BtnTextures[i], GUILayout.Width(BtnTextures[i].width), GUILayout.Height(BtnTextures[i].height)))
                    {
                        if (i < BestChromosomes.Count)
                        {
                            ReplaceLayerChromosome(BestChromosomes[i].Item2 as LayerChromosome);
                            UseData.RegisterSuggestionUse();
                        }
                    }
                    if (i < BestChromosomes.Count)
                    {
                        GUILayout.Label(BestChromosomes[i].Item1 + ": " + (int)(100 * BestChromosomes[i].Item2.Fitness) + "%");
                    }
                    else
                    {
                        GUILayout.Label("");
                    }
                    EditorGUILayout.EndVertical();
                    //EditorGUILayout.Space();
                    #endregion

                    #region Second Button
                    if (i + 1 < SelectedLayer.Controller.GeneticAlgorithms.Count)
                    {
                        /*if (i+1 < BestChromosomes.Count)
                        {
                            BestChromosomes[i+1].Item2.Paint(BtnTextures[i+1], SelectedLayer.Controller.Color);
                        }*/

                        EditorGUILayout.BeginVertical();
                        if (GUILayout.Button(BtnTextures[i + 1], GUILayout.Width(BtnTextures[i + 1].width), GUILayout.Height(BtnTextures[i + 1].height)))
                        {
                            if (i + 1 < BestChromosomes.Count)
                            {
                                ReplaceLayerChromosome(BestChromosomes[i + 1].Item2 as LayerChromosome);
                                UseData.RegisterSuggestionUse();
                            }
                        }
                        if (i + 1 < BestChromosomes.Count)
                        {
                            GUILayout.Label(BestChromosomes[i + 1].Item1 + ": " + (int)(100 * BestChromosomes[i + 1].Item2.Fitness) + "%");
                        }
                        else
                        {
                            GUILayout.Label("");
                        }
                        EditorGUILayout.EndVertical();
                        #endregion

                    }

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();

                EditorGUILayout.Space();
                if(pause)
                {
                    pause = !GUILayout.Button("UNPAUSE");
                    if (clock.IsRunning)
                    {
                        clock.Reset();
                        clock.Stop();
                    }
                }
                else
                {
                    pause = GUILayout.Button("PAUSE");
                    if (!clock.IsRunning)
                    {
                        clock.Restart();
                    }
                }
                
                if (GUILayout.Button("STOP"))
                {
                    isRunning = false;
                }
            }
            else
            {
                if (GUILayout.Button("RUN"))
                {
                    if (SelectedLayer.Controller.GeneticAlgorithms.Where((g) => g == null || g.FitnessFunction == null).Count() > 0 || SelectedLayer.Controller.GeneticAlgorithms.Count == 0)
                    {
                        return;
                    }
                    isRunning = true;
                    foreach(RunningGa ga in SelectedLayer.Controller.GeneticAlgorithms)
                    {
                        if(ga != null && ga.FitnessFunction != null)
                        {
                            UseData.AddGa(ga.FitnessFunction.name);
                        }
                    }
                }
            }
        }
            

        GUILayout.EndArea();
        
    }

    public void ResetCycle()
    {
        //UnityEngine.Debug.Log("Hello");
        if (clock.IsRunning)
        {
            clock.Stop();
        }
        if (SelectedLayer == null) return;
        if (SelectedLayer.Controller == null) return;
        foreach(PaintView v in MapWindow.layers)
        {
            v.Controller.Sample(v.Texture);
        }
        SelectedLayer.Controller.Restart(SelectedLayer.Texture);
        clock.Reset();
        clock.Restart();
    }

    public void ReplaceLayerChromosome(LayerChromosome chromose)
    {
        SelectedLayer.Controller.BaseChromosome = chromose;
        chromose.Paint(SelectedLayer.Texture, SelectedLayer.Controller.Color);
        ResetCycle();
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(int offsetX, int offsetY)
    {
        Rect.x = offsetX;
        Rect.y = offsetY;
    }
}
