using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStateSystem : ISystem
{
    public void UpdateSystem()
    {
        World.ForEachElementWithTag(new List<string> { World.simulableTag, World.dynamicTag }, new List<string> { "Velocity", "Position", "Size" }, (EntityComponent entity, List<IComponent> components) => {
            VelocityComponent velComponent = (VelocityComponent)components[0];
            PositionComponent posComponent = (PositionComponent)components[1];
            SizeComponent sizeComponent    = (SizeComponent)components[2];

            SaveEntityState(ref posComponent, ref velComponent, ref sizeComponent);

            return new List<IComponent> { velComponent, posComponent, sizeComponent };
        });
    }

    private void SaveEntityState(ref PositionComponent posComponent, ref VelocityComponent velComponent, ref SizeComponent sizeComponent)
    {
        const float maxTime = 2;
        float currentTime = Time.time;

        posComponent.savedPositions.Add(new SavedPosition(posComponent.position, currentTime));
        velComponent.savedSpeeds.Add(new SavedSpeed(velComponent.speed, currentTime));
        sizeComponent.savedSizes.Add(new SavedSize(sizeComponent.size, currentTime));

        posComponent.savedPositions.RemoveAll((SavedPosition savedPos) => { return OlderThan(currentTime, savedPos.time, maxTime); });
        velComponent.savedSpeeds.RemoveAll((SavedSpeed savedSpeed)     => { return OlderThan(currentTime, savedSpeed.time, maxTime); });
        sizeComponent.savedSizes.RemoveAll((SavedSize savedSize)       => { return OlderThan(currentTime, savedSize.time, maxTime); });
    }

    private bool OlderThan(float currentTime, float stateTime, float maxTime)
    {
        return currentTime - stateTime > maxTime;
    }

    public string Name
    {
        get
        {
            return "SaveStateSystem";
        }
    }
}
