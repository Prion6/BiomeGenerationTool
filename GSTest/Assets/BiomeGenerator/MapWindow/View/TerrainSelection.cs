using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainSelection : EditorPanel
{
    Terrain Terrain;
    public System.Action<Terrain> OnSelectterrain;

    public TerrainSelection(int x, int y, int width, int height) : base(x, y, width, height)
    {
    }

    public override void Draw()
    {
        GUI.DrawTexture(Rect, Texture, ScaleMode.StretchToFill);

        GUILayout.BeginArea(new Rect(Rect.x + MapWindow.padding, Rect.y + MapWindow.padding, Rect.width - MapWindow.padding * 2, Rect.height - MapWindow.padding * 2));
        //GUILayout.BeginArea(Rect);
        if(Terrain == null && Selection.objects.Length > 0)
        {
            if (Selection.objects[0] is GameObject)
            {
                Terrain = (Selection.objects[0] as GameObject).GetComponent<Terrain>();
            }
        }

        GUILayout.Label("Select a Terrain or \nchoose one in the Object Field");
        EditorGUILayout.Space();

        Terrain = EditorGUILayout.ObjectField("Terrain: ", Terrain, typeof(Terrain), true) as Terrain;

        EditorGUILayout.Space();

        if(GUILayout.Button("Select terrain") && Terrain != null)
        {
            OnSelectterrain?.Invoke(Terrain);
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
}
