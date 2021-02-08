using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSystem : ISystem
{
    public void UpdateSystem()
    {
        UpdateCollisionTags();
        UpdateSimulableTag();
    }

    private void UpdateCollisionTags()
    {
        World.ForEachElementWithTag(new List<string> { World.simulableTag, World.dynamicTag }, new List<string> { "Size" }, (EntityComponent entity, List<IComponent> components) => {
            SizeComponent sizeComponent = (SizeComponent)components[0];

            string untag = (sizeComponent.size < ECSManager.Instance.Config.minSize) ? World.withCollisionTag : World.withoutCollisionTag;
            string tag = (sizeComponent.size < ECSManager.Instance.Config.minSize) ? World.withoutCollisionTag : World.withCollisionTag;

            World.Untag(untag, entity);
            World.Tag(tag, entity);

            return new List<IComponent> { sizeComponent };
        });
    }

    private void UpdateSimulableTag()
    {
        FramesCounterComponent counterComponent = (FramesCounterComponent)World.GetSingletonComponent<FramesCounterComponent>();

        if (counterComponent.GetCounterValue() < 4)
        {
            World.ForAllElements((EntityComponent entity, List<IComponent> components) => {
                World.Untag(World.simulableTag, entity);
                if (EntityIsSimulable(entity)) World.Tag(World.simulableTag, entity);
                return components;
            });

            counterComponent.AddToCounter();
        }
        else
        {
            World.ForAllElements((EntityComponent entity, List<IComponent> components) => {
                World.Tag(World.simulableTag, entity);
                return components;
            });

            counterComponent.ResetCounter();
        }
    }

    private bool EntityIsSimulable(EntityComponent entity)
    {
        PositionComponent posComponent = (PositionComponent)World.components["Position"][entity.id];
        return posComponent.position.y > 0;
    }

    public string Name
    {
        get
        {
            return "TagSystem";
        }
    }
}
