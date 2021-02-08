using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSystem : ISystem
{
    public void UpdateSystem()
    {
        World.ForEachElementWithTag(new List<string> { World.simulableTag, World.dynamicTag }, new List<string> { "Size" }, (EntityComponent entity, List<IComponent> components) => {
            SizeComponent sizeComponent = (SizeComponent)components[0];

            string untag = (sizeComponent.size < ECSManager.Instance.Config.minSize) ? World.withCollisionTag : World.withoutCollisionTag;
            string tag = (sizeComponent.size < ECSManager.Instance.Config.minSize) ? World.withoutCollisionTag : World.withCollisionTag;

            World.Untag(untag, entity);
            World.Tag(tag, entity);

            return new List<IComponent> { sizeComponent };
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
