using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : ISystem
{
    public void UpdateSystem()
    {
        // Obtention et sauvegarde des positions et des tailles
        List<Vector2> positions = new List<Vector2>();
        List<float> sizes = new List<float>();
        ComponentManager.ForEachElementWithTag("withCollision", new List<string> { "Position", "Size" }, (EntityComponent entity, List<IComponent> components) => {
            PositionComponent posComponent = (PositionComponent)components[0];
            SizeComponent sizeComponent = (SizeComponent)components[1];

            positions.Add(posComponent.position);
            sizes.Add(sizeComponent.size);
            return components;
        });

        // Detection des collisions entre entites
        ComponentManager.ForEachElementWithTag("withCollision", new List<string> { "Position", "Size", "Velocity" }, (EntityComponent entity, List<IComponent> components) => {
            PositionComponent posComponent = (PositionComponent)components[0];
            SizeComponent sizeComponent = (SizeComponent)components[1];
            VelocityComponent velComponent = (VelocityComponent)components[2];
            
            if (ScreenCollisionDetected(posComponent.position, sizeComponent.size))
            {
                //Debug.Log("OUT_____");
                velComponent.speed *= -1;
                sizeComponent.size = 1;
                ECSManager.Instance.UpdateShapeSize(entity.id, sizeComponent.size);
            }
            else if (CollisionDetected(positions, posComponent.position, sizes, sizeComponent.size))
            {
                velComponent.speed *= -1;
                sizeComponent.size /= 2;
                ECSManager.Instance.UpdateShapeSize(entity.id, sizeComponent.size);
            }
            
            return new List<IComponent>{ posComponent, sizeComponent, velComponent };
        });


    }

    private bool CollisionDetected(List<Vector2> positions, Vector2 targetPosition, List<float> sizes, float targetSize)
    {
        int index = 0;
        foreach(Vector2 position in positions)
        {
            if (position != targetPosition)
            {
                float minDist = targetSize/2 + sizes[index]/2;
                if (Vector2.Distance(targetPosition, position) <= minDist) return true;
            }
            index++;
        }
        return false;
    }

    private bool ScreenCollisionDetected(Vector2 targetPosition, float targetSize)
    {
        // TODO
        Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Vector3 rstageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(-Screen.width, -Screen.height, 0));
        float maxX = stageDimensions.x;
        float maxY = stageDimensions.y;
        float minX = rstageDimensions.x;
        float minY = rstageDimensions.y;

        Vector2 pos = Camera.main.ScreenToWorldPoint(targetPosition);
        return pos.x >= maxX || pos.y >= maxY;
    }

    public string Name
    {
        get
        {
            return "CollisionSystem";
        }
    }
}
