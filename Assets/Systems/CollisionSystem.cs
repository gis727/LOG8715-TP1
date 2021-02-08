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
        World.ForEachElementWithTag(new List<string> { World.simulableTag, World.withCollisionTag }, new List<string> { "Position", "Size" }, (EntityComponent entity, List<IComponent> components) => {
            PositionComponent posComponent = (PositionComponent)components[0];
            SizeComponent sizeComponent = (SizeComponent)components[1];

            positions.Add(posComponent.position);
            sizes.Add(sizeComponent.size);
            return components;
        });

        // Detection des collisions entre entités
        World.ForEachElementWithTag(new List<string> { World.simulableTag, World.withCollisionTag }, new List<string> { "Position", "Size", "Velocity" }, (EntityComponent entity, List<IComponent> components) => {
            PositionComponent posComponent = (PositionComponent)components[0];
            SizeComponent sizeComponent = (SizeComponent)components[1];
            VelocityComponent velComponent = (VelocityComponent)components[2];

            bool entityIsStatic = World.EntityIsTagged(World.staticTag, entity);

            if (!entityIsStatic) // Les entités statiques ne réagissent pas aux collisions
            {
                bool entityIsEscapingWall = World.EntityIsTagged(World.escapingWallTag, entity);
                bool collisionDetected = CollisionDetected(positions, posComponent.position, sizes, sizeComponent.size);

                if (!entityIsEscapingWall && collisionDetected)
                {
                    velComponent.speed *= -1;
                    sizeComponent.size /= 2;
                    ECSManager.Instance.UpdateShapeSize(entity.id, sizeComponent.size);
                    World.Untag(World.escapingWallTag, entity);
                }
            }

            return new List<IComponent>{ posComponent, sizeComponent, velComponent };
        });

        // Détection des collisions avec l'écran
        World.ForEachElementWithTag(new List<string> { World.simulableTag, World.dynamicTag }, new List<string> { "Position", "Size", "Velocity" }, (EntityComponent entity, List<IComponent> components) => {
            PositionComponent posComponent = (PositionComponent)components[0];
            SizeComponent sizeComponent = (SizeComponent)components[1];
            VelocityComponent velComponent = (VelocityComponent)components[2];
            bool entityIsEscapingWall = World.EntityIsTagged(World.escapingWallTag, entity);
            bool collisionDetected = ScreenCollisionDetected(posComponent.position, sizeComponent.size);

            if (entityIsEscapingWall && !collisionDetected)
            {
                World.Untag(World.escapingWallTag, entity);
                RestoreEntity(ref sizeComponent, entity);
            }
            else if (!entityIsEscapingWall && collisionDetected)
            {
                World.Tag(World.escapingWallTag, entity);
                velComponent.speed *= -1;
                RestoreEntity(ref sizeComponent, entity);
            }
            ECSManager.Instance.UpdateShapeSize(entity.id, sizeComponent.size);

            return new List<IComponent> { posComponent, sizeComponent, velComponent };
        });

    }

    // Remet le tag de collision sur une entité
    private void RestoreEntity(ref SizeComponent sizeComponent, EntityComponent entity)
    {
        sizeComponent.size = sizeComponent.defaultSize;
        World.Tag(World.withCollisionTag, entity);
        World.Untag(World.withoutCollisionTag, entity);
    }

    // Retourne "True" si une collision est détectée entre toutes les entiés (positions + sizes) et l'entité visée (targetPosition + targetSize)
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

    // Retourne "True" si une collision est détectée entre l'entité visée et l'un des bords de l'écran
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
