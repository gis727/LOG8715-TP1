using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationSystem : ISystem
{
    public void UpdateSystem()
    {
        if (World.ContainsEntities()) return;

        uint shapesCount = (uint) ECSManager.Instance.Config.allShapesToSpawn.Count;
        uint index = 0;
        uint maxStaticIndex = shapesCount / 4;

        foreach (Config.ShapeConfig config in ECSManager.Instance.Config.allShapesToSpawn)
        {
            EntityComponent entity = new EntityComponent();
            entity.id = World.AddEntity(config.initialPos, config.initialSpeed, config.size);

            string movementTag = (index < maxStaticIndex) ? "static" : "dynamic";
            World.Tag(movementTag, entity);
            World.Tag("withCollision", entity);
            World.Tag(World.defaultTag, entity);

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
