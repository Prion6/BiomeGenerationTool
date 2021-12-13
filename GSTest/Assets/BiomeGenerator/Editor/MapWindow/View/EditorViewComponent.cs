using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class EditorViewComponent
{
    public abstract void Init();
    public abstract void Init(int offsetX, int offsetY);
    public abstract void Draw();
}
