using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class World
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

    #region Tags definitions
    public static readonly string simulableTag = "simulable";
    public static readonly string defaultTag = "shape";
    public static readonly string withCollisionTag = "withCollision";
    public static readonly string withoutCollisionTag = "withoutCollision";
    public static readonly string dynamicTag = "dynamic";
    public static readonly string staticTag = "static";
    public static readonly string escapingWallTag = "escapingWall";
    #endregion

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

    // Retourne toutes les entitées marquées avec tous les tags de "targetTags"
    private static List<EntityComponent> GetAllEntitiesWithTags(List<string> targetTags)
    {
        HashSet<EntityComponent> entities = new HashSet<EntityComponent>(tags[targetTags[0]]);

        foreach(string tag in targetTags)
        {
            entities = new HashSet<EntityComponent>(entities.Intersect(tags[tag]));
        }
        return entities.ToList();
    }

    public static void ForEachElementWithTag(List<string> targetTags, List<string> componentNames, System.Func<EntityComponent, List<IComponent>, List<IComponent>> lambda)
    {
        if (targetTags.Count == 0) targetTags = new List<string> {defaultTag};
        foreach(string tag in targetTags) if (!tags.ContainsKey(tag)) return;

        foreach(EntityComponent entity in GetAllEntitiesWithTags(targetTags))
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
    #endregion

}
