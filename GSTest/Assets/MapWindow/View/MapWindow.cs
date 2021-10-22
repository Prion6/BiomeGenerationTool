﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;

public class MapWindow : EditorWindow
{
    public static Rect PaintArea;
    Texture2D PaintAreaBackground;
    GeneralSettingsView generalSettingsView;
    LayerSettingsView layerSettingsView;
    DisplayOptionsView dispayOptionsView;
    public static LayerChromosome selectedLayerChromosome;
    public static Vector2 chromosomeSize = new Vector2(128, 128);
    public static Vector2 TextureSize = new Vector2(512, 512);

    string posText = "";

    public static int padding = 8;
    

    [MenuItem("Window/BiomeGenerator/2D Map View")]
    public static void ShowWindow()
    {
        var window = GetWindow<MapWindow>("2D Map View");
        window.minSize = new Vector2(1120,710);
        window.maxSize = new Vector2(1120, 710);
    }

    private void OnEnable()
    {
        PaintArea.width = TextureSize.x;
        PaintArea.height = TextureSize.y;
        PaintArea.x = PaintArea.y = 0;
        //paintView.Init(padding / 2, padding / 2);

        generalSettingsView = new GeneralSettingsView((int)PaintArea.width + padding * 2, padding, 256, 256);
        generalSettingsView.OnAddLayerBtn = (() =>
        {
            generalSettingsView.AddLayer((int)chromosomeSize.x, (int)chromosomeSize.y);
        });
        

        

        layerSettingsView = new LayerSettingsView((int)generalSettingsView.GetRect().x, 
            (int)(generalSettingsView.GetRect().height + padding * 2), 
            256, 
            (int)(PaintArea.height + 180 - generalSettingsView.GetRect().height - padding / 2));

        dispayOptionsView = new DisplayOptionsView((int)(PaintArea.width + layerSettingsView.GetRect().width) + 4 * padding, padding, 304, (int)(layerSettingsView.GetRect().height + generalSettingsView.GetRect().height + padding));

        PaintAreaBackground = new Texture2D(512, 512);
        for (int j = 0; j < PaintAreaBackground.width; j++)
        {
            for (int i = 0; i < PaintAreaBackground.height; i++)
            {
                PaintAreaBackground.SetPixel(i, j, Color.black);
            }
        }
        PaintAreaBackground.Apply();
    }

    private void OnGUI()
    {
        GUI.skin.label.normal.textColor = Color.white;
        
        //GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), new Texture2D(1,1), ScaleMode.StretchToFill);


        if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
        {
            var pos = Event.current.mousePosition;
            posText = pos.ToString();
            // EJE Y Invertido
            if(PaintArea.Contains(pos))
            {
                if(generalSettingsView.GetCurrentLayer() != null)
                {
                    if (Event.current.button == 0)
                    {
                        generalSettingsView.GetCurrentLayer().Paint((int)pos.x, 512 - (int)pos.y, true, generalSettingsView.Intensity, generalSettingsView.BrushSize);
                    }
                    else if (Event.current.button == 1)
                    {
                        generalSettingsView.GetCurrentLayer().Paint((int)pos.x, 512 - (int)pos.y, false, generalSettingsView.Intensity, generalSettingsView.BrushSize);
                    }
                    Repaint();
                }
                
            }
        }

        //Set PaintView According to Active layers

        GUI.DrawTexture(PaintArea,PaintAreaBackground);

        UpdateSections();

        GUILayout.Label(posText);
        Repaint();
    }

    void UpdateSections()
    {
        //System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();
        //clock.Start();
        generalSettingsView.Update();
        if (generalSettingsView.GetCurrentLayer() != null)
        {
            selectedLayerChromosome = generalSettingsView.GetCurrentLayer().Controller.BaseChromosome;
            if (layerSettingsView.SelectedLayer == null || !generalSettingsView.GetCurrentLayer().Controller.name.Equals(layerSettingsView.SelectedLayer.name))
            {
                layerSettingsView.SelectedLayer = generalSettingsView.GetCurrentLayer().Controller;
                dispayOptionsView.SetSelectedLayer(generalSettingsView.GetCurrentLayer());
            }
            
        }
        layerSettingsView.Update();
        dispayOptionsView.Update();
        //Debug.Log("Time: " + Time.time + " - Clock: " + clock.ElapsedMilliseconds);
    }

    private void OnDestroy()
    {
        if(layerSettingsView.SelectedLayer != null)
            DestroyImmediate(layerSettingsView.SelectedLayer);
    }
}