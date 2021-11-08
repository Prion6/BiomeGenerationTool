using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DrawingSettings : EditorPanel
{
    public int BrushSize { get; protected set; }
    public float Intensity { get; protected set; }
    public bool Acumulative { get; protected set; }
    public bool paintPoints { get; protected set; }

    public System.Action OnGenerateBtn;

    public DrawingSettings(int x, int y, int width, int height) : base(x, y, width, height)
    {
        BrushSize = 10;
        Intensity = 0.5f;
        paintPoints = true;
    }

    public override void Draw()
    {
        GUI.DrawTexture(Rect, Texture, ScaleMode.StretchToFill);

        GUILayout.BeginArea(new Rect(Rect.x + MapWindow.padding, Rect.y + MapWindow.padding, Rect.width - MapWindow.padding * 2, Rect.height - MapWindow.padding * 2));

        GUILayout.Label("Draw Settings");

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Brush Size\t");
        BrushSize = (int)EditorGUILayout.Slider(BrushSize, 1, 52);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Intensity\t");
        Intensity = EditorGUILayout.Slider(Intensity, 0, 1);
        EditorGUILayout.EndHorizontal();

        Acumulative = EditorGUILayout.Toggle("Acumulative", Acumulative);
        paintPoints = EditorGUILayout.Toggle("Start Points", paintPoints);

        EditorGUILayout.Space();

        if(GUILayout.Button("Generate"))
        {
            OnGenerateBtn?.Invoke();
        }

        GUILayout.EndArea();
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(int offsetX, int offsetY)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    public void Update()
    {
        Draw();
    }
}
