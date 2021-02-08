using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoringSystem : ISystem
{
    public void UpdateSystem()
    {
        // Update des couleurs des composants dynamiques avec collisions
        World.ForEachElementWithTag(new List<string> { World.simulableTag, World.withCollisionTag }, new List<string> { }, (EntityComponent entity, List<IComponent> components) => {
            ECSManager.Instance.UpdateShapeColor(entity.id, Color.blue);
            return components;
        });

        // Update des couleurs des composants dynamiques sans collisions
        World.ForEachElementWithTag(new List<string> { World.simulableTag, World.withoutCollisionTag }, new List<string> { }, (EntityComponent entity, List<IComponent> components) => {
            ECSManager.Instance.UpdateShapeColor(entity.id, Color.green);
            return components;
        });

        // Update des couleurs des composants statiques
        World.ForEachElementWithTag(new List<string> { World.simulableTag, World.staticTag }, new List<string> { }, (EntityComponent entity, List<IComponent> components) => {
            ECSManager.Instance.UpdateShapeColor(entity.id, Color.red);
            return components;
        });
    }

    public string Name
    {
        get
        {
            return "ColoringSystem";
        }
    }
}
