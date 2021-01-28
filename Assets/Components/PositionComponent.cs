using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SavedPosition
{
    public SavedPosition(Vector2 position, float time)
    {
        this.position = position;
        this.time = time;
    }

    public Vector2 position;
    public float time;
}

public struct PositionComponent : IComponent
{
    public PositionComponent(Vector2 position)
    {
        this.position = position;
        this.savedPositions = new List<SavedPosition>();
    }
    public Vector2 position;
    public List<SavedPosition> savedPositions;
}
