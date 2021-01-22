using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentManager
{
    public static Dictionary<string, Dictionary<uint, IComponent>> components = new Dictionary<string, Dictionary<uint, IComponent>>();
    public static Dictionary<string, HashSet<EntityComponent>> tags = new Dictionary<string, HashSet<EntityComponent>>();

    public static bool ContainsEntities()
    {
        return components.ContainsKey("Position");
    }

    public static uint AddEntity(Vector2 pos, Vector2 speed, float size)
    {
        if (!components.ContainsKey("Position")) components.Add("Position", new Dictionary<uint, IComponent>());
        if (!components.ContainsKey("Velocity")) components.Add("Velocity", new Dictionary<uint, IComponent>());
        if (!components.ContainsKey("Size")) components.Add("Size", new Dictionary<uint, IComponent>());

        uint entityId = (uint) components["Position"].Count;

        PositionComponent posComp;
        VelocityComponent velComp;
        posComp.position = pos;
        velComp.speed = speed;

        components["Position"][entityId] = posComp;
        components["Velocity"][entityId] = velComp;
        components["Size"][entityId] = new SizeComponent(size);

        return entityId;
    }

    public static void Tag(string tag, EntityComponent entity)
    {
        if (!tags.ContainsKey(tag)) tags.Add(tag, new HashSet<EntityComponent>());
        tags[tag].Add(entity);
    }

    public static void Untag(string tag, EntityComponent entity)
    {
        if (!tags.ContainsKey(tag)) return;
        tags[tag].Remove(entity);
    }

    public static bool EntityIsTagged(string tag, EntityComponent entity)
    {
        if (!tags.ContainsKey(tag)) return false;
        return tags[tag].Contains(entity);
    }

    public static void ForEachElementWithTag(string tag, List<string> componentNames, System.Func<EntityComponent, List<IComponent>, List<IComponent>> lambda)
    {
        if (!tags.ContainsKey(tag)) return;

        foreach(EntityComponent entity in new HashSet<EntityComponent>(tags[tag]))
        {
            // Get all required components
            List<IComponent> reqComponents = new List<IComponent>();
            foreach (string componentName in componentNames)
            {
                reqComponents.Add(components[componentName][entity.id]);
            }

            // Execute lambda on components
            List<IComponent> newComponents = lambda(entity, reqComponents);

            //Update components
            int index = 0;
            foreach (string componentName in componentNames)
            {
                components[componentName][entity.id] = newComponents[index];
                index++;
            }
        }
    }
}
