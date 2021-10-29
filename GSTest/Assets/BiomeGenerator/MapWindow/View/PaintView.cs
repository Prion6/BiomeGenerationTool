using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using GeneticSharp.Domain.Chromosomes;

public class PaintView : ScriptableObject
{
    public bool IsVisible;

    [HideInInspector]
    public LayerController Controller { get; protected set; }
    [HideInInspector]
    public Texture2D Texture;

    public void Init(int width, int height, string name)
    {
        Texture = new Texture2D((int)MapWindow.TextureSize.x, (int)MapWindow.TextureSize.y, TextureFormat.ARGB32, false);
        Controller = CreateInstance<LayerController>();
        Controller.Init((int)MapWindow.chromosomeSize.x, (int)MapWindow.chromosomeSize.y, name);

        Texture.alphaIsTransparency = true;

        for (int j = 0; j < Texture.width; j++)
        {
            for (int i = 0; i < Texture.height; i++)
            {
                Texture.SetPixel(i, j, new Color(0,0,0,0));
            }
        }

        Texture.Apply();
    }

    public void Draw()
    {
        GUILayout.BeginArea(MapWindow.PaintArea);

        GUI.DrawTexture(MapWindow.PaintArea, Texture);

        GUILayout.EndArea();
    }

    public void Paint(int x, int y, bool add, float Intensity, int BrushSize, bool Acumulative)
    {

        float val = add ? Intensity * 1 : Intensity * -1;
        for (int j = -BrushSize; j < BrushSize; j++)
        {
            for (int i = -BrushSize; i < BrushSize; i++)
            {
                if (MapWindow.PaintArea.Contains(new Vector2(x+i,y+j)) && MathTools.Distance(i,j,0,0) <= BrushSize)
                {
                    float alpha = Texture.GetPixel(x + i, y + j).a;
                    Color c = Controller.Color;
                    if(Acumulative)
                    {
                        c.a = alpha + val;
                    }
                    else
                    {
                        if(add)
                        {
                            c.a = alpha > val ? alpha : val; 
                        }
                        else 
                        {
                            c.a = alpha > -val ? -val : alpha;
                        }
                    }
                    Texture.SetPixel(x + i, y + j, c);
                }
            }
        }

        Texture.Apply();
    }
}
