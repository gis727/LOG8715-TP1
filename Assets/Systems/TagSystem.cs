using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSystem : ISystem
{
    public void UpdateSystem()
    {
        ComponentManager.ForEachElementWithTag("dynamic", new List<string> { "Size" }, (EntityComponent entity, List<IComponent> components) => {
            SizeComponent sizeComponent = (SizeComponent)components[0];

            string untag = (sizeComponent.size < ECSManager.Instance.Config.minSize) ? "withCollision" : "withoutCollision";
            string tag = (sizeComponent.size < ECSManager.Instance.Config.minSize) ? "withoutCollision" : "withCollision";

            ComponentManager.Untag(untag, entity);
            ComponentManager.Tag(tag, entity);

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
