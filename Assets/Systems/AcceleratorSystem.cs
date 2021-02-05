using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorSystem : ISystem
{
    public void UpdateSystem()
    {
        FramesCounterComponent counterComponent = (FramesCounterComponent)World.GetSingletonComponent<FramesCounterComponent>();

        if (counterComponent.GetCounterValue() < 4)
        {
            foreach (EntityComponent entity in World.tags[World.defaultTag])
            {
                World.Untag(World.simulableTag, entity);

                if (EntityIsSimulable(entity)) World.Tag(World.simulableTag, entity);
            }

            counterComponent.AddToCounter();
        }
        else
        {
            foreach (EntityComponent entity in World.tags[World.defaultTag])
            {
                World.Tag(World.simulableTag, entity);
            }

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
            return "AcceleratorSystem";
        }
    }
}
