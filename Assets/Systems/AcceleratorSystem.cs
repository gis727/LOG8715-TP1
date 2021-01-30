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
            foreach (EntityComponent entity in ComponentManager.tags[ComponentManager.defaultTag])
            {
                ComponentManager.Untag(ComponentManager.simulableTag, entity);

                if (EntityIsSimulable(entity)) ComponentManager.Tag(ComponentManager.simulableTag, entity);
            }
            
            ComponentManager.AddToCounter();
        }
        else
        {
            foreach (EntityComponent entity in ComponentManager.tags[ComponentManager.defaultTag])
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
