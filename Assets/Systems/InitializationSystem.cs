using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationSystem : ISystem
{
    public void UpdateSystem()
    {
        if (ComponentManager.ContainsEntities()) return;

        foreach(Config.ShapeConfig config in ECSManager.Instance.Config.allShapesToSpawn)
        {
            ECSManager.Instance.CreateShape(ComponentManager.AddEntity(config.initialPos, config.initialSpeed), config);
        }
    }

    public string Name {
        get
        {
            return "InitializationSystem";
        }
    }
}
