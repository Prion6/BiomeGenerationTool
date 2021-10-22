using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStat
{
    public string label;

    public override bool Equals(object obj)
    {
        if (obj as BaseStat == null) return false;
        return label.Equals((obj as BaseStat).label);
    }

    public override int GetHashCode()
    {
        return -1322592123 + EqualityComparer<string>.Default.GetHashCode(label);
    }
}
