using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownSystem : ISystem
{
    public void UpdateSystem()
    {
        const float coolDownLength = 2;
        RestorationComponent component = (RestorationComponent)ComponentManager.GetSingletonComponent<RestorationComponent>();

        bool newCooldownDetected = component.coolDownStartTime == -1 && Input.GetKey(KeyCode.Space);
        bool cooldownInProgress =  component.coolDownStartTime != -1;

        if (newCooldownDetected)
        {
            component.restorationRequired = true;
            component.coolDownStartTime = Time.time;
        }
        else if (cooldownInProgress)
        {
            float coolDownLapse = Time.time - component.coolDownStartTime;
            if (coolDownLapse >= coolDownLength && !component.restorationRequired)
            {
                component.coolDownStartTime = -1;
                Debug.Log("END OF COOLDOWN");
            }
            else
            {
                Debug.Log("Cooldown In progress. " + coolDownLapse.ToString("F3") + " seconds left.");
            }
        }

        ComponentManager.SetSingletonComponent<RestorationComponent>(component);
    }

    public string Name
    {
        get
        {
            return "CoolDownSystem";
        }
    }
}
