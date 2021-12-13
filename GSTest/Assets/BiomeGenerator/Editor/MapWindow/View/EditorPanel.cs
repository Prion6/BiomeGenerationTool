using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditorPanel : EditorViewComponent
{
    protected Rect Rect;
    protected Texture2D Texture;

    public EditorPanel(int x, int y, int width, int height)
    {
        Rect.x = x;
        Rect.y = y;
        Rect.width = width;
        Rect.height = height;

        Texture = new Texture2D(1, 1);
        Texture.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f, 1));

        Texture.Apply();

    }

    public Texture2D GetTexture()
    {
        return Texture;
    }

    public Rect GetRect()
    {
        return Rect;
    }
}
