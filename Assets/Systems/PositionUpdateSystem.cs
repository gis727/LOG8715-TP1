using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUpdateSystem : ISystem
{
    public void UpdateSystem()
    {
        // Lists to save modified components
        List<PositionComponent> newPos = new List<PositionComponent>();
        List<uint> keys = new List<uint>();

        foreach(uint entityKey in ComponentManager.components["Position"].Keys)
        {
            PositionComponent posComponent = (PositionComponent) ComponentManager.components["Position"][entityKey];
            VelocityComponent velComponent = (VelocityComponent) ComponentManager.components["Velocity"][entityKey];

            posComponent.position += velComponent.speed * Time.deltaTime*1;

            newPos.Add(posComponent);
            keys.Add(entityKey);

            ECSManager.Instance.UpdateShapePosition(entityKey, posComponent.position);
        }

        // Updated all positions in component
        int index = 0;
        foreach(uint key in keys)
        {
            ComponentManager.components["Position"][key] = newPos[index];
            index++;
        }
    }

    public string Name
    {
        get
        {
            return "PositionUpdateSystem";
        }
    }
}
