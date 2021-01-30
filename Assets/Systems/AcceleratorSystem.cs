using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorSystem : ISystem
{
    public void UpdateSystem()
    {
        int iterationCounter = ComponentManager.GetCounterValue();

        if (iterationCounter < 4)
        {
            foreach (EntityComponent entity in ComponentManager.tags["shape"])
            {
                ComponentManager.Untag(ComponentManager.simulableTag, entity);

                if (EntityIsSimulable(entity)) ComponentManager.Tag(ComponentManager.simulableTag, entity);
            }
            
            ComponentManager.AddToCounter();
        }
        else
        {
            foreach (EntityComponent entity in ComponentManager.tags["shape"])
            {
                ComponentManager.Tag(ComponentManager.simulableTag, entity);
            }
            
            ComponentManager.ResetCounter();
        }
    }

    private bool EntityIsSimulable(EntityComponent entity)
    {
        PositionComponent posComponent = (PositionComponent)ComponentManager.components["Position"][entity.id];
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
