using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentManager
{
    #region ECS
    public static Dictionary<string, Dictionary<uint, IComponent>> components = new Dictionary<string, Dictionary<uint, IComponent>>();

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

        components["Position"][entityId] = new PositionComponent(pos);
        components["Velocity"][entityId] = new VelocityComponent(speed);
        components["Size"][entityId] = new SizeComponent(size);

        return entityId;
    }

    public static IComponent GetSingletonComponent<ComponentType>() where ComponentType : IComponent, new()
    {
        uint entityId = uint.MaxValue; // Entitee unique associee au singleton
        string componentName = System.ComponentModel.TypeDescriptor.GetClassName(typeof(ComponentType));

        if (!components.ContainsKey(componentName))
        {
            components.Add(componentName, new Dictionary<uint, IComponent>());
            components[componentName][entityId] = new ComponentType();
        }

        return components[componentName][entityId];
    }
    public static void SetSingletonComponent<ComponentType>(IComponent component)
    {
        uint entityId = uint.MaxValue;
        string componentName = System.ComponentModel.TypeDescriptor.GetClassName(typeof(ComponentType));
        if (components.ContainsKey(componentName)) components[componentName][entityId] = component;
    }
    #endregion


    #region TAG
    public static Dictionary<string, HashSet<EntityComponent>> tags = new Dictionary<string, HashSet<EntityComponent>>();
    public static readonly string simulableTag = "simulable";

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
            if (EntityIsTagged(simulableTag, entity))
            {
                // Get all required components
                List<IComponent> reqComponents = new List<IComponent>();
                foreach (string componentName in componentNames)
                {
                    reqComponents.Add(components[componentName][entity.id]);
                }

                // Execute lambda on components
                List<IComponent> newComponents = lambda(entity, reqComponents);

                // Update components
                int index = 0;
                foreach (string componentName in componentNames)
                {
                    components[componentName][entity.id] = newComponents[index];
                    index++;
                }

            }
        }
    }
    #endregion


    #region Extra frames counter
    public static int counter = 0;

    public static int GetCounterValue()
    {
        return counter;
    }

    public static void AddToCounter()
    {
        counter++;
    }

    public static void ResetCounter()
    {
        counter = 0;
    }
    #endregion
}
