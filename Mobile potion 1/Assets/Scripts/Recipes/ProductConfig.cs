using System;
using UnityEngine;

public abstract class ProductConfig : ScriptableObject, IComparable
{
    public string Name;
    public string Description;
    public Sprite Sprite;

    public int CompareTo(object obj)
    {
        var otherConfig = (ProductConfig) obj;

        return Name.CompareTo(otherConfig.Name);
    }
}