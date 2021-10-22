using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using System;
using System.Diagnostics;

public class DisplayOptionsView : EditorPanel
{
    public PaintView SelectedLayer { get; set; }
    public List<Tuple<string, LayerChromosome>> BestChromosomes;

    bool isRunning;
    bool IsRunning {
        get
        {
            return isRunning;
        }
        set {
            if(!isRunning && value && SelectedLayer != null)
                ResetCycle();
            if (isRunning && !value && SelectedLayer != null)
                SelectedLayer.Controller.Stop();
            isRunning = value;
        } }
    Stopwatch clock;

    Vector2 scrollPosition;
    

    public DisplayOptionsView(int x, int y, int width, int height) : base(x, y, width, height)
    {
        BestChromosomes = new List<Tuple<string,LayerChromosome>>();
        clock = new Stopwatch();
        scrollPosition = Vector2.zero;
    }

    public void Update()
    {
        if (SelectedLayer != null && IsRunning)
        {
            if (SelectedLayer.Controller.Waiting())
            {
                BestChromosomes.Clear();
                foreach (RunningGa ga in SelectedLayer.Controller.GeneticAlgorithms)
                {
                    if (ga.GA == null) continue;
                    if (ga.GA.BestChromosome == null) continue;
                    BestChromosomes.Add(Tuple.Create(ga.FitnessFunction.label,ga.GA.BestChromosome as LayerChromosome));
                }
                //ResetCycle();
                clock.Stop();
            }
        }
        Draw();
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
        
        IsRunning = EditorGUILayout.Toggle("Running",IsRunning);
        EditorGUILayout.Space();

        //UnityEngine.Debug.Log(IsRunning);
        if (SelectedLayer != null && IsRunning)
        {
            GUILayout.Label("Clock: " + (clock.ElapsedMilliseconds / 1000));
            EditorGUILayout.Space();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < SelectedLayer.Controller.GeneticAlgorithms.Count; i+=2)
            {
                EditorGUILayout.BeginHorizontal();
                
                #region First Button
                Texture2D btnTexture = new Texture2D(128, 128);
                if (i < BestChromosomes.Count)
                {
                    BestChromosomes[i].Item2.Paint(btnTexture, SelectedLayer.Controller.Color);
                }

                EditorGUILayout.BeginVertical();
                if (GUILayout.Button(btnTexture, GUILayout.Width(btnTexture.width), GUILayout.Height(btnTexture.height))) 
                {
                    if (i < BestChromosomes.Count)
                        ReplaceLayerChromosome(BestChromosomes[i].Item2 as LayerChromosome);
                }
                if (i < BestChromosomes.Count)
                {
                    GUILayout.Label(BestChromosomes[i].Item1 + ": " + (int)(100 * BestChromosomes[i].Item2.Fitness));
                }
                else
                {
                    GUILayout.Label("");
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                #endregion

                #region Second Button
                if (i+1 < SelectedLayer.Controller.GeneticAlgorithms.Count)
                {
                    Texture2D btnTexture2 = new Texture2D(128, 128);
                    if (i+1 < BestChromosomes.Count)
                    {
                        BestChromosomes[i+1].Item2.Paint(btnTexture2, SelectedLayer.Controller.Color);
                    }

                    EditorGUILayout.BeginVertical();
                    if (GUILayout.Button(btnTexture2, GUILayout.Width(btnTexture2.width), GUILayout.Height(btnTexture2.height)))
                    {
                        if (i+1 < BestChromosomes.Count)
                            ReplaceLayerChromosome(BestChromosomes[i+1].Item2 as LayerChromosome);
                    }
                    if (i+1 < BestChromosomes.Count)
                    {
                        GUILayout.Label(BestChromosomes[i + 1].Item1 + ": " + (int)(100 * BestChromosomes[i + 1].Item2.Fitness));
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
        SelectedLayer.Controller.Restart(SelectedLayer.Texture);
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
