using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorationComponent : IComponent
{
    public RestorationComponent()
    {
        this.coolDownStartTime = -1;
        this.restorationRequired = false;
    }

    public float coolDownStartTime;
    public bool restorationRequired;
}
