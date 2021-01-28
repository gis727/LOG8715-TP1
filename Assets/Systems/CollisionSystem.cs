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

            bool entityIsStatic = ComponentManager.EntityIsTagged("static", entity);

            if (!entityIsStatic) // Les entitees statiques ne reagissent pas aux collisions
            {
                bool entityIsEscapingWall = ComponentManager.EntityIsTagged("escapingWall", entity);
                bool collisionDetected = CollisionDetected(positions, posComponent.position, sizes, sizeComponent.size);

                if (!entityIsEscapingWall && collisionDetected)
                {
                    velComponent.speed *= -1;
                    sizeComponent.size /= 2;
                    ECSManager.Instance.UpdateShapeSize(entity.id, sizeComponent.size);
                    ComponentManager.Untag("escapingWall", entity);
                }
            }

            return new List<IComponent>{ posComponent, sizeComponent, velComponent };
        });

        // Detection des collisions avec l'ecran
        ComponentManager.ForEachElementWithTag("dynamic", new List<string> { "Position", "Size", "Velocity" }, (EntityComponent entity, List<IComponent> components) => {
            PositionComponent posComponent = (PositionComponent)components[0];
            SizeComponent sizeComponent = (SizeComponent)components[1];
            VelocityComponent velComponent = (VelocityComponent)components[2];
            bool entityIsEscapingWall = ComponentManager.EntityIsTagged("escapingWall", entity);
            bool collisionDetected = ScreenCollisionDetected(posComponent.position, sizeComponent.size);

            if (entityIsEscapingWall && !collisionDetected)
            {
                ComponentManager.Untag("escapingWall", entity);
                RestoreEntity(ref sizeComponent, entity);
            }
            else if (!entityIsEscapingWall && collisionDetected)
            {
                ComponentManager.Tag("escapingWall", entity);
                velComponent.speed *= -1;
                RestoreEntity(ref sizeComponent, entity);
            }
            ECSManager.Instance.UpdateShapeSize(entity.id, sizeComponent.size);

            return new List<IComponent> { posComponent, sizeComponent, velComponent };
        });

    }

    private void RestoreEntity(ref SizeComponent sizeComponent, EntityComponent entity)
    {
        sizeComponent.size = sizeComponent.defaultSize;
        ComponentManager.Tag("withCollision", entity);
        ComponentManager.Untag("withoutCollision", entity);
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

    private bool ScreenCollisionDetected(Vector2 pos, float targetSize)
    {
        Vector2 upperScreenLimits = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 lowerScreenLimits = Camera.main.ScreenToWorldPoint(Vector2.zero);
        float targetHalfSize = targetSize / 2;
        float maxX = upperScreenLimits.x - targetHalfSize;
        float maxY = upperScreenLimits.y - targetHalfSize;
        float minX = lowerScreenLimits.x + targetHalfSize;
        float minY = lowerScreenLimits.y + targetHalfSize;

        return pos.x <= minX || pos.x >= maxX || pos.y <= minY || pos.y >= maxY;
    }

    public string Name
    {
        get
        {
            return "CollisionSystem";
        }
    }
}
