using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;
using System.Diagnostics;
using System;

public class MapWindow : EditorWindow
{
    public static Rect PaintArea;
    Texture2D PaintAreaBackground;
    GeneralSettingsView generalSettingsView;
    LayerSettingsView layerSettingsView;
    DisplayOptionsView dispayOptionsView;
    TerrainSelection terrainSelection;
    DrawingSettings drawingSettings;
    public static LayerChromosome selectedLayerChromosome;
    public static Vector2 chromosomeSize = new Vector2(128, 128);
    public static Vector2 TextureSize = new Vector2(512, 512);
    public static List<Tuple<int, int>> StartPoints = new List<Tuple<int, int>>();
    public static bool hasChanged;
    Texture2D PointTexture;
    int pointSize = 2;
    static Terrain selectedTerrain;
    public static Terrain Terrain
    {
        get
        {
            return selectedTerrain;
        }

        set
        {
            if (selectedTerrain != null && selectedTerrain.Equals(value)) return;
            selectedTerrain = value;
            selectedTerrain.terrainData.SyncHeightmap();

            heightmap = new Texture2D(512, 512);

            float width = (int)selectedTerrain.terrainData.bounds.size.x;
            float depth = (int)selectedTerrain.terrainData.bounds.size.z;

            //float centerHeight = selectedTerrain.terrainData.bounds.center.y;
            float totalHeight = selectedTerrain.terrainData.bounds.size.y;

            //float minHeight = selectedTerrain.transform.position.y + centerHeight - totalHeight / 2;
            //float maxHeight = selectedTerrain.transform.position.y + centerHeight + totalHeight / 2;

            for (int j = 0; j < TextureSize.y; j++)
            {
                for (int i = 0; i < TextureSize.x; i++)
                {
                    float alpha = selectedTerrain.SampleHeight(selectedTerrain.transform.position + new Vector3(i * width / TextureSize.x, 0, j * depth / TextureSize.y)) / totalHeight;
                    int a = (int)(alpha * mapDetail);
                    alpha = a / (1.0f * mapDetail);
                    //UnityEngine.Debug.Log(alpha);
                    heightmap.SetPixel(i, j, new Color(1, 1, 1, alpha));
                }
            }
            heightmap.Apply();
        }
    }
    static Texture2D heightmap;
    [Range(0, 100)]
    static int mapDetail;
    static float[] _PerlinNoise;
    public static float[] Perlin
    {
        get {
            if (_PerlinNoise == null)
            {
                _PerlinNoise = pnd.Init();
            }
            return _PerlinNoise;
        }
    }
    static PerlinNoiseDistortion pnd;
    public static List<PaintView> layers;

    string posText = "";

    public static int padding = 8;

    Stopwatch clock;
    float refreshDelay = 3f;
    

    [MenuItem("Window/BiomeGenerator/2D Map View")]
    public static void ShowWindow()
    {
        var window = GetWindow<MapWindow>("2D Map View");
        window.minSize = new Vector2(512,512);
        window.maxSize = new Vector2(1120, 710);
    }

    private void OnEnable()
    {
        //Perlin = (CreateInstance<PerlinNoise>()).GenerateNoiseMap((int)chromosomeSize.x,10);
        mapDetail = 30;

        selectedTerrain = null;
        PaintArea.width = TextureSize.x;
        PaintArea.height = TextureSize.y;
        PaintArea.x = PaintArea.y = 0;
        //paintView.Init(padding / 2, padding / 2);

        drawingSettings = new DrawingSettings((int)PaintArea.width + padding * 2, padding, 256, 256);
        drawingSettings.OnGenerateBtn = (() => { Generate(); });

        generalSettingsView = new GeneralSettingsView(padding, (int)PaintArea.height + padding, (int)PaintArea.width - padding, 710 - (int)PaintArea.height - padding*2);
        generalSettingsView.OnAddLayerBtn = (() =>
        {
            generalSettingsView.AddLayer((int)chromosomeSize.x, (int)chromosomeSize.y);
        });
        layers = generalSettingsView.Layers;


        PaintAreaBackground = new Texture2D(1, 1);
        PaintAreaBackground.SetPixel(0, 0, Color.black);
        PaintAreaBackground.Apply();

        heightmap = new Texture2D(1, 1);
        heightmap.SetPixel(0, 0, Color.black);
        heightmap.Apply();

        terrainSelection = new TerrainSelection((int)PaintArea.width + padding * 2, padding, 256, 256);
        terrainSelection.OnSelectterrain = ((t) =>
        {
            Terrain = t;

        });



        layerSettingsView = new LayerSettingsView((int)drawingSettings.GetRect().x, 
            (int)(drawingSettings.GetRect().height + padding * 2), 
            256, 
            (int)(PaintArea.height + 180 - drawingSettings.GetRect().height - padding / 2));

        dispayOptionsView = new DisplayOptionsView((int)(PaintArea.width + layerSettingsView.GetRect().width) + 4 * padding, padding, 304, (int)(layerSettingsView.GetRect().height + drawingSettings.GetRect().height + padding));

        /*PaintAreaBackground = new Texture2D(512, 512);
        for (int j = 0; j < PaintAreaBackground.width; j++)
        {
            for (int i = 0; i < PaintAreaBackground.height; i++)
            {
                PaintAreaBackground.SetPixel(i, j, Color.black);
            }
        }
        PaintAreaBackground.Apply();*/


        PointTexture = new Texture2D(1, 1);
        PointTexture.SetPixel(0, 0, Color.red);
        PointTexture.Apply();

        hasChanged = false;

        clock = new Stopwatch();
        clock.Start();
        
    }

    private void OnGUI()
    {
        if(pnd == null)
        {
            pnd = Resources.Load("Distorter") as PerlinNoiseDistortion;
            pnd.Init();
        }

        GUI.skin.label.normal.textColor = Color.white;
        

        GUI.DrawTexture(PaintArea, PaintAreaBackground);
        GUI.DrawTexture(PaintArea, heightmap);

        if (selectedTerrain == null)
        {

            terrainSelection.Draw();
            return;
        }

        UpdateSections();

        GUILayout.Label(posText);

        if(drawingSettings.paintPoints)
        {
            PaintStartPoints();
        }
        



        if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
        {
            var pos = Event.current.mousePosition;
            posText = pos.ToString();
            // EJE Y Invertido
            if (PaintArea.Contains(pos))
            {
                if (generalSettingsView.GetCurrentLayer() != null)
                {
                    hasChanged = true;
                    if (Event.current.button == 0)
                    {
                        generalSettingsView.GetCurrentLayer().Paint((int)pos.x, 512 - (int)pos.y, true, drawingSettings.Intensity, drawingSettings.BrushSize, drawingSettings.Acumulative);
                    }
                    else if (Event.current.button == 1)
                    {
                        generalSettingsView.GetCurrentLayer().Paint((int)pos.x, 512 - (int)pos.y, false, drawingSettings.Intensity, drawingSettings.BrushSize, drawingSettings.Acumulative);
                    }
                    //Repaint();
                }

            }
        }
        Repaint();
    }

    public void PaintStartPoints()
    {
        
        foreach(Tuple<int,int> t in StartPoints)
        {
            float x = Mathf.Clamp(t.Item1 * (TextureSize.x / chromosomeSize.x) - pointSize, 0, TextureSize.x - (1 + pointSize*2));
            float y = Mathf.Clamp(t.Item2 * (TextureSize.y / chromosomeSize.y) - pointSize, 0, TextureSize.y - (1 + pointSize*2));
            Rect r = new Rect(x, y, pointSize * 2 + 1, pointSize * 2 + 1);
            GUI.DrawTexture(r, PointTexture);
        }
    }

    void UpdateSections()
    {
        //System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();
        //clock.Start();

        drawingSettings.Update();
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
        layers = generalSettingsView.Layers;
        layerSettingsView.Update();
        dispayOptionsView.Draw();
        if (clock.ElapsedMilliseconds / 1000 > refreshDelay)
        {
            //UnityEngine.Debug.Log("reset");
            clock.Reset();
            clock.Start();
        }
        else
        {
            return;
        }

        
        dispayOptionsView.Update();
        //Debug.Log("Time: " + Time.time + " - Clock: " + clock.ElapsedMilliseconds);
    }

    public void Generate()
    {
        float width = (int)selectedTerrain.terrainData.bounds.size.x;
        float depth = (int)selectedTerrain.terrainData.bounds.size.z;

        float density = drawingSettings.Density;
        if (density <= 0) return;

        foreach (PaintView p in generalSettingsView.Layers)
        {
            float addedOdds = 0;
            foreach (MapElement m in p.Controller.mapElements)
            {
                addedOdds += m.priority;
            }
            if (addedOdds == 0) continue;

            Color[] pixels = MathTools.Resize(p.Texture.GetPixels(),p.Texture.width,p.Texture.height, (int)chromosomeSize.x, (int)chromosomeSize.y);
            

            GameObject parent = new GameObject();
            parent.name = p.Controller.Label;
            for (int j = 0; j < chromosomeSize.y; j++)
            {
                for (int i = 0; i < chromosomeSize.x; i++)
                {
                    float val = pixels[j * (int)chromosomeSize.x + i].a;


                    //if (UnityEngine.Random.Range(0, 1) > val) continue;
                    if (val <= 0) continue;

                    float xDist = (1 - (val*val)) * width / chromosomeSize.x;
                    float zDist = (1 - (val * val)) * depth / chromosomeSize.y;


                    float xSteps = ((width / chromosomeSize.x) / (1 + xDist));// * (density * density);
                    float zSteps = ((depth / chromosomeSize.y) / (1 + zDist));// * (density * density);

                    float r = UnityEngine.Random.Range(0, addedOdds);
                    for (int z = 0; z < zSteps; z++)
                    {
                        for (int x = 0; x < xSteps; x++)
                        {
                            if (UnityEngine.Random.value > density) continue;
                            foreach (MapElement m in p.Controller.mapElements)
                            {

                                r -= m.priority;
                        
                                if (r <= 0)
                                {
                                    Vector3 dPosition = new Vector3(UnityEngine.Random.Range(0, m.positionRandomRange.x) + (xDist * x),
                                        0,
                                        UnityEngine.Random.Range(0, m.positionRandomRange.z) + (zDist * z));

                                    Vector3 dRotation = new Vector3(UnityEngine.Random.Range(0, m.rotationRandomRange.x),
                                        UnityEngine.Random.Range(0, m.rotationRandomRange.y),
                                        UnityEngine.Random.Range(0, m.rotationRandomRange.z));

                                    Vector3 pos = selectedTerrain.transform.position + new Vector3(width * i / chromosomeSize.x, 0, depth * j / chromosomeSize.y);
                                    pos += dPosition;

                                    float height = selectedTerrain.SampleHeight(pos) + UnityEngine.Random.Range(0, m.positionRandomRange.y);

                                    GameObject g = Instantiate(m.prefab, pos + Vector3.up * height, m.prefab.transform.rotation);
                                    g.transform.Rotate(dRotation);
                                    float dscale = UnityEngine.Random.Range(1, m.scaleRandomRange);
                                    g.transform.localScale = new Vector3(g.transform.localScale.x * dscale,
                                        g.transform.localScale.y * dscale,
                                        g.transform.localScale.z * dscale);

                                    g.transform.SetParent(parent.transform);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        if(layerSettingsView.SelectedLayer != null)
            DestroyImmediate(layerSettingsView.SelectedLayer);
    }
}
