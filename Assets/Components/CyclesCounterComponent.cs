using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclesCounterComponent : IComponent
{
    public CyclesCounterComponent()
    {
        this.counter = 0;
    }

    public int counter;

    public int GetCounterValue()
    {
        return this.counter;
    }

    public void AddToCounter()
    {
        this.counter++;
    }

    public void ResetCounter()
    {
        this.counter = 0;
    }
}
