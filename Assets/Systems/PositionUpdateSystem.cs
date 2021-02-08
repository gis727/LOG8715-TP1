using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUpdateSystem : ISystem
{
    public void UpdateSystem()
    {
        // Update des composants dynamiques
        World.ForEachElementWithTag(new List<string> { World.simulableTag, World.dynamicTag }, new List<string>{ "Velocity", "Position" }, (EntityComponent entity, List<IComponent> components) => {
            VelocityComponent velComponent = (VelocityComponent)components[0];
            PositionComponent posComponent = (PositionComponent)components[1];

            posComponent.position += velComponent.speed * Time.deltaTime * 1;
            ECSManager.Instance.UpdateShapePosition(entity.id, posComponent.position);

            return new List<IComponent>{ velComponent, posComponent };
        });

        // Update des composants statiques
        World.ForEachElementWithTag(new List<string> { World.simulableTag, World.staticTag }, new List<string> { "Position" }, (EntityComponent entity, List<IComponent> components) => {
            PositionComponent posComponent = (PositionComponent)components[0];

            ECSManager.Instance.UpdateShapePosition(entity.id, posComponent.position);

            return new List<IComponent> { posComponent };
        });
    }

    public string Name
    {
        get
        {
            return "PositionUpdateSystem";
        }
    }
}
