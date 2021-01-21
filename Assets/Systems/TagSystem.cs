﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSystem : ISystem
{
    public void UpdateSystem()
    {
        // Update des couleurs des composants statiques
        ComponentManager.ForEachElementWithTag("static", new List<string> { }, (EntityComponent entity, List<IComponent> components) => {
            ECSManager.Instance.UpdateShapeColor(entity.id, Color.red);
            return components;
        });

        // Update des couleurs des composants dynamiques avec collisions
        ComponentManager.ForEachElementWithTag("withCollision", new List<string> { }, (EntityComponent entity, List<IComponent> components) => {
            ECSManager.Instance.UpdateShapeColor(entity.id, Color.blue);
            return components;
        });

        // Update des couleurs des composants dynamiques sans collisions
        ComponentManager.ForEachElementWithTag("withoutCollision", new List<string> { }, (EntityComponent entity, List<IComponent> components) => {
            ECSManager.Instance.UpdateShapeColor(entity.id, Color.green);
            return components;
        });
    }

    public string Name
    {
        get
        {
            return "TagSystem";
        }
    }
}
