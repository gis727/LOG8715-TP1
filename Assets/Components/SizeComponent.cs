using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SizeComponent : IComponent
{
    public SizeComponent(float size)
    {
        this.size = size;
    }
    public float size;
}
