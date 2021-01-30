using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreStateSystem : ISystem
{
    public void UpdateSystem()
    {
        RestorationComponent component = (RestorationComponent)ComponentManager.GetSingletonComponent<RestorationComponent>();

        if (component.restorationRequired)
        {
            foreach (EntityComponent entity in ComponentManager.tags[ComponentManager.defaultTag])
            {
                ComponentManager.Tag(ComponentManager.simulableTag, entity);
            }

            ComponentManager.ForEachElementWithTag("dynamic", new List<string> { "Position", "Size", "Velocity" }, (EntityComponent entity, List<IComponent> components) => {
                PositionComponent posComponent = (PositionComponent)components[0];
                SizeComponent sizeComponent = (SizeComponent)components[1];
                VelocityComponent velComponent = (VelocityComponent)components[2];

                if (posComponent.savedPositions.Count > 0)
                {
                    posComponent.position = posComponent.savedPositions[0].position;
                    sizeComponent.size = sizeComponent.savedSizes[0].size;
                    velComponent.speed = velComponent.savedSpeeds[0].speed;
                }

                return new List<IComponent> { posComponent, sizeComponent, velComponent };
            });

            component.restorationRequired = false;
        }

    }

    public string Name
    {
        get
        {
            return "RestoreStateSystem";
        }
    }
}
