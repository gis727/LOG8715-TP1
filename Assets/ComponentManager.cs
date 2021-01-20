using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentManager
{
    public static Dictionary<string, Dictionary<uint, IComponent>> components = new Dictionary<string, Dictionary<uint, IComponent>>();
    
    public static bool ContainsEntities()
    {
        return components.ContainsKey("Position");
    }

    public static uint AddEntity(Vector2 pos, Vector2 speed)
    {
        if (!components.ContainsKey("Position")) components.Add("Position", new Dictionary<uint, IComponent>());
        if (!components.ContainsKey("Velocity")) components.Add("Velocity", new Dictionary<uint, IComponent>());

        uint entityId = (uint) components["Position"].Count;

        PositionComponent posComp;
        posComp.position = pos;

        VelocityComponent velComp;
        velComp.speed = speed;

        components["Position"][entityId] = posComp;
        components["Velocity"][entityId] = velComp;

        return entityId;
    }
}
