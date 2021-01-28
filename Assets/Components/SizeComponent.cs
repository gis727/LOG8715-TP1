using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SavedSize
{
    public SavedSize(float size, float time)
    {
        this.size = size;
        this.time = time;
    }

    public float size;
    public float time;
}

public struct SizeComponent : IComponent
{
    public SizeComponent(float size)
    {
        this.size = size;
        this.defaultSize = size;
        this.savedSizes = new List<SavedSize>();
    }
    public float size;
    public readonly float defaultSize;
    public List<SavedSize> savedSizes;
}
