using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.Events;

public class GeneralSettingsView : EditorPanel
{
    public int BrushSize { get; protected set; }
    public float Intensity { get; protected set; }
    //public int CurrentLayerIndex { get; protected set; }
    public PaintView SelectedLayer;

    //public LayerMode LayerMode { get; protected set; }

    Vector2 layerScrollPosition;

    public System.Action OnAddLayerBtn;

    List<PaintView> Layers;

    public int AddedLayers { get; protected set; }

    //GUIStyle TextStyle;

    public GeneralSettingsView(int x, int y, int width, int height) : base(x, y, width, height)
    {
        Layers = new List<PaintView>();
        BrushSize = 10;
        Intensity = 0.1f;
        layerScrollPosition = Vector2.zero;
        //CurrentLayerIndex = -1;
        //LayerMode = LayerMode.DEPTH;
        AddedLayers = 0;
        SelectedLayer = null;
        //TextStyle.normal.textColor = Color.black;
    }

    public void Update()
    {
        Draw();
    }

    public void AddLayer(int width, int height)
    {
        PaintView p = ScriptableObject.CreateInstance<PaintView>();
        p.Init(width, height, "Layer " + AddedLayers);
        Layers.Add(p);
        AddedLayers++;
        if (Layers.Count == 1)
            SelectedLayer = Layers[0];
    }

    public int GetLayerCount()
    {
        return Layers.Count;
    }

    public PaintView GetLayer(int i)
    {
        if (i > 0 && i < Layers.Count - 1)
            return Layers[i];
        return null;
    }

    public PaintView GetCurrentLayer()
    {
        //if (CurrentLayerIndex < 0 || CurrentLayerIndex > Layers.Count) return null;
        //return Layers[CurrentLayerIndex];
        return SelectedLayer;
    }

    public override void Draw()
    {
        GUI.DrawTexture(Rect, Texture, ScaleMode.StretchToFill);

        GUILayout.BeginArea(new Rect(Rect.x + MapWindow.padding, Rect.y + MapWindow.padding, Rect.width - MapWindow.padding * 2, Rect.height - MapWindow.padding * 2));

        GUILayout.Label("General Settings");

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Brush Size\t");
        BrushSize = (int)EditorGUILayout.Slider(BrushSize, 1, 52);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Intensity\t");
        Intensity = EditorGUILayout.Slider(Intensity, 0, 1);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Selected Layer: ");
        //CurrentLayerIndex = EditorGUILayout.Popup(CurrentLayerIndex, Layers.Select((l) => l.Controller.Label).ToArray());

        int CurrentLayerIndex = -1;
        if (SelectedLayer != null)
        {
            CurrentLayerIndex = Layers.FindIndex((p) => p.name.Equals(SelectedLayer.name));
        }

        if(CurrentLayerIndex >= 0 && CurrentLayerIndex < Layers.Count)
        {
            /*Debug.Log(CurrentLayerIndex);
            CurrentLayerIndex = EditorGUILayout.Popup(CurrentLayerIndex, Layers.Select((l) => l.Controller.Label).ToArray());
            SelectedLayer = Layers[CurrentLayerIndex];*/
            SelectedLayer = Layers[EditorGUILayout.Popup(Layers.FindIndex((p) => p.Controller.name.Equals(SelectedLayer.Controller.name)), Layers.Select((l) => l.Controller.Label).ToArray())];
        }
        if (CurrentLayerIndex != -1 && CurrentLayerIndex < Layers.Count)
            Layers[CurrentLayerIndex].IsVisible = true;

        EditorGUILayout.EndHorizontal();

        //LayerController must not be GameObject()
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Layers ");
        if (GUILayout.Button("+"))
        {
            OnAddLayerBtn?.Invoke();
            CurrentLayerIndex = Layers.Count - 1;
        }
        EditorGUILayout.EndHorizontal();

        Layers = Layers.AsEnumerable().OrderBy((l) => l.Controller.DrawOrder).ToList<PaintView>();

        layerScrollPosition = EditorGUILayout.BeginScrollView(layerScrollPosition);
        /*foreach(PaintView p in Layers)
        {
            var obj = Editor.CreateEditor(p);
            EditorGUILayout.BeginHorizontal();
            obj.DrawDefaultInspector();
            EditorGUILayout.EndHorizontal();
        }*/
        List<LayerSelectionoPanel> panels  = new List<LayerSelectionoPanel>();
        for(int i = 0; i < Layers.Count; i++)
        {
            LayerSelectionoPanel p =(new LayerSelectionoPanel(Layers[i], i, 
                () => {
                    Layers.RemoveAt(i);
                    if (i == CurrentLayerIndex) CurrentLayerIndex = -1;
                }, 
                (PaintView pv) => SelectedLayer = pv));
            p.Draw();
        }// can add AsParallel() to query
        EditorGUILayout.EndScrollView();

        GUILayout.EndArea();


        DrawLayers();
    }

    public void DrawLayers()
    {
        foreach(PaintView p in Layers)
        {
            if (p.IsVisible)
                p.Draw();

        }
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(int offsetX, int offsetY)
    {
        Rect.x = offsetX;
        Rect.y = offsetY;

        AddLayer(512,512);
    }
}


public class LayerSelectionoPanel
{
    PaintView PaintView;
    System.Action Delete;
    System.Action<PaintView> Select;

    public LayerSelectionoPanel(PaintView paintView, int index, System.Action delete, System.Action<PaintView> select)
    {
        PaintView = paintView;
        Delete = delete;
        Select = select;
    }

    public void Draw()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(PaintView.Controller.Label);
        GUILayout.FlexibleSpace();
        PaintView.IsVisible = GUILayout.Toggle(PaintView.IsVisible,"");
        //GUILayout.Label(LayerController.Name);

        if (GUILayout.Button("S"))
        {
            //Becomes the active Layer
            Select?.Invoke(PaintView);
            PaintView.IsVisible = true;
        }
        if(GUILayout.Button("D"))
        {
            Delete?.Invoke();
        }
        EditorGUILayout.EndHorizontal();
    }
}

public enum LayerMode
{
    DEPTH,
    OVERLAP
}