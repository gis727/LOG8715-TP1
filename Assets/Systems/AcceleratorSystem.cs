using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorSystem : ISystem
{
    public void UpdateSystem()
    {
        // Recuperation des entites en vue de determiner si elles doivent etre simulees durant la frame
        string tag = "simulable";
        string conditionComponent = "Position";
        int iterationCounter = ComponentManager.GetCounterValue();

        PositionComponent posComponent;

        if (iterationCounter < 4)
        {
            foreach (EntityComponent entity in ComponentManager.tags["shape"])
            {
                posComponent = (PositionComponent)ComponentManager.components[conditionComponent][entity.id];

                // retire le tag simulable
                ComponentManager.Untag(tag, entity);

                // si applicable ajoute le tag simulable
                if (posComponent.position.y > 0)
                {
                    ComponentManager.Tag(tag, entity);
                }
            }
            ComponentManager.AddToCounter();
        }
        else
        {
            foreach (EntityComponent entity in ComponentManager.tags["shape"])
            {
                ComponentManager.Tag(tag, entity);
            }
            ComponentManager.ResetCounter();
        }
    }


    public string Name
    {
        get
        {
            return "AcceleratorSystem";
        }
    }
}
