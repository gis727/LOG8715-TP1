using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownSystem : ISystem
{
    public void UpdateSystem()
    {
        
        RestorationComponent component = (RestorationComponent)World.GetSingletonComponent<RestorationComponent>();

        bool newCooldownDetected = component.coolDownStartTime == -1 && Input.GetKey(KeyCode.Space);
        bool cooldownInProgress = component.coolDownStartTime != -1;

        UpdateCoolDown(component, newCooldownDetected, cooldownInProgress);
        ShowCoolDownState(component, cooldownInProgress);
    }

    private void ShowCoolDownState(RestorationComponent component, bool cooldownInProgress)
    {
        if (Input.GetKey(KeyCode.Space) && cooldownInProgress)
        {
            float coolDownLapse = Time.time - component.coolDownStartTime;
            Debug.Log("Cooldown In progress. " + coolDownLapse.ToString("F3") + " seconds left.");
        }
    }

    private void UpdateCoolDown(RestorationComponent component, bool newCooldownDetected, bool cooldownInProgress)
    {
        const float coolDownLength = 2;

        if (newCooldownDetected)
        {
            component.restorationRequired = true;
            component.coolDownStartTime = Time.time;
        }
        else if (cooldownInProgress)
        {
            float coolDownLapse = Time.time - component.coolDownStartTime;
            if (coolDownLapse >= coolDownLength) component.coolDownStartTime = -1;
        }

        World.SetSingletonComponent<RestorationComponent>(component);
    }

    public string Name
    {
        get
        {
            return "CoolDownSystem";
        }
    }
}
