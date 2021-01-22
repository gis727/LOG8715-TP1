using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PositionComponent : IComponent
{
    public PositionComponent(Vector2 position)
    {
        this.position = position;
    }
    public Vector2 position;
}
