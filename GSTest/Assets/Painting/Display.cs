using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display
{
    public Texture2D Texture { get; protected set; }

    public Display(Texture2D baseTexture)
    {
        Texture = baseTexture;
    }

    public void SetPixel(int x, int y, Color color)
    {
        Texture.SetPixel(x, y, color);
    }

    public virtual void SaveChanges()
    {
        Texture.Apply();
    }
}
