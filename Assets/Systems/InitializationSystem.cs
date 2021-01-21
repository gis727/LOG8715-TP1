using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationSystem : ISystem
{
    public void UpdateSystem()
    {
        if (ComponentManager.ContainsEntities()) return;

        uint shapesCount = (uint) ECSManager.Instance.Config.allShapesToSpawn.Count;
        uint index = 0;
        uint maxStaticIndex = shapesCount / 4;

        foreach (Config.ShapeConfig config in ECSManager.Instance.Config.allShapesToSpawn)
        {
            EntityComponent entity = new EntityComponent();
            entity.id = ComponentManager.AddEntity(config.initialPos, config.initialSpeed, config.size);
            if (index < maxStaticIndex) ComponentManager.Tag("static", entity);
            else
            {
                ComponentManager.Tag("dynamic", entity);
                ComponentManager.Tag("withCollision", entity);
            }

            ECSManager.Instance.CreateShape(entity.id, config);
            index++;
        }
    }

    public string Name {
        get
        {
            return "InitializationSystem";
        }
    }
}
