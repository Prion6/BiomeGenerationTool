using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UIElements;
using System;
using GeneticSharp.Domain.Fitnesses;

public class LayerSettingsView : EditorPanel
{
    public LayerController SelectedLayer { get; set; }
    Vector2 scrollPos;

    public LayerSettingsView(int x, int y, int width, int height) : base(x, y, width, height)
    {
        scrollPos = Vector2.zero;
    }

    public void Update()
    {
        //if (SelectedLayer == null) return;
        Draw();
    }

    public override void Draw()
    {


        GUI.DrawTexture(Rect, Texture, ScaleMode.StretchToFill);

        if (SelectedLayer == null) return;
        GUILayout.BeginArea(new Rect(Rect.x + MapWindow.padding, Rect.y + MapWindow.padding, Rect.width - MapWindow.padding * 2, Rect.height - MapWindow.padding * 2));

        GUILayout.Label("Layer Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        var editor = Editor.CreateEditor(SelectedLayer);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        editor.DrawDefaultInspector();
        EditorGUILayout.EndScrollView();

        if (SelectedLayer.GeneticAlgorithms.Count > 8)
        {
            SelectedLayer.GeneticAlgorithms.RemoveRange(8, SelectedLayer.GeneticAlgorithms.Count - 8);
        }
        /*

        GUILayout.Label("Layer Settings");
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name: ");
        SelectedLayer.Name = EditorGUILayout.TextField(SelectedLayer.Name);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Color: ");
        SelectedLayer.Color = EditorGUILayout.ColorField(SelectedLayer.Color);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Draw Order: ");
        SelectedLayer.DrawOrder = EditorGUILayout.DelayedIntField(SelectedLayer.DrawOrder);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        mapElements = EditorGUILayout.Foldout(mapElements, "Map Elements");
        if(mapElements)
        {
            elementsScrollPos = EditorGUILayout.BeginScrollView(elementsScrollPos);
            var obj = new GameObject();
            obj.AddComponent(typeof(MapElementWrapper));
            var wraper = obj.GetComponent<MapElementWrapper>();
            wraper.LayerPrefabs = SelectedLayer.mapElements;
            var objEditor = Editor.CreateEditor(wraper);
            objEditor.DrawDefaultInspector();
            EditorGUILayout.EndScrollView();

            GameObject.DestroyImmediate(obj);
        }

        EditorGUILayout.Space();

        fitnessSelection = EditorGUILayout.Foldout(fitnessSelection, "Fitness Functions");
        if(fitnessSelection)
        {
            /*EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            var add = GUILayout.Button("+");
            var remove = GUILayout.Button("-");
            EditorGUILayout.EndHorizontal();

            if(add)
            {
                SelectedLayer.AddGA(default);
                SelectedLayer.FitnessesToRun++;
            }

            if(remove)
            {
                SelectedLayer.RemoveGA(SelectedLayer.GeneticAlgorithms.Count - 1);
                SelectedLayer.FitnessesToRun--;
            }
            

            for (int i = 0; i < SelectedLayer.FitnessesToRun; i++)
            {
                EditorGUILayout.Space();
                GUILayout.Label("Fitness Function: " + i);
                SelectedLayer.SetFitnessFunction(i,
                    EditorGUILayout.ObjectField(
                    SelectedLayer.GeneticAlgorithms[i].FitnessFunction, typeof(IFitness), true, GUILayout.ExpandWidth(false)) as BaseFitnessFunction);
               
            }

            fitnessScrollPos = EditorGUILayout.BeginScrollView(fitnessScrollPos);
            var obj = new GameObject();
            obj.AddComponent(typeof(FitnessesWrapper));
            var wraper = obj.GetComponent<FitnessesWrapper>();
            wraper.FitnessFunctions = SelectedLayer.GeneticAlgorithms;
            var objEditor = Editor.CreateEditor(wraper);
            objEditor.DrawDefaultInspector();
            EditorGUILayout.EndScrollView();

            GameObject.DestroyImmediate(obj);
        }*/


        GUILayout.EndArea();
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public void SetLayer()
    {

    }

    public override void Init(int offsetX, int offsetY)
    {
        Rect.x = offsetX;
        Rect.y = offsetY;
    }

    
}

public class StatPanel
{
    [Range(0,1)]
    public float StatFitness;
    public string StatName;
    
    public StatPanel(float statFitness, string statName)
    {
        StatFitness = statFitness;
        StatName = statName;
    }

    public void Draw()
    {
        EditorGUILayout.BeginVertical();
        EditorGUI.ProgressBar(new Rect(0,0,100,30), StatFitness, StatName);
        EditorGUILayout.EndVertical();
    }
}

public class MapElementWrapper : MonoBehaviour
{
    public List<MapElement> LayerPrefabs;
}

public class FitnessesWrapper : MonoBehaviour
{
    public List<RunningGa> FitnessFunctions;
}
