using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorSystem : ISystem
{
    public void UpdateSystem()
    {
        int iterationCounter = World.GetCounterValue();

        if (iterationCounter < 4)
        {
            foreach (EntityComponent entity in World.tags[World.defaultTag])
            {
                World.Untag(World.simulableTag, entity);

                if (EntityIsSimulable(entity)) World.Tag(World.simulableTag, entity);
            }
            
            World.AddToCounter();
        }
        else
        {
            foreach (EntityComponent entity in World.tags[World.defaultTag])
            {
                World.Tag(World.simulableTag, entity);
            }
            
            World.ResetCounter();
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
