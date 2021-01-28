using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SavedSpeed
{
    public SavedSpeed(Vector2 speed, float time)
    {
        this.speed = speed;
        this.time = time;
    }

    public Vector2 speed;
    public float time;
}

public struct VelocityComponent : IComponent
{
    public VelocityComponent(Vector2 speed)
    {
        this.speed = speed;
        this.savedSpeeds = new List<SavedSpeed>();
    }
    public Vector2 speed;
    public List<SavedSpeed> savedSpeeds;
}
