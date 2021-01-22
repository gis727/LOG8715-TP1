using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VelocityComponent : IComponent
{
    public VelocityComponent(Vector2 speed)
    {
        this.speed = speed;
    }
    public Vector2 speed;
}
