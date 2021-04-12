using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownSystem : MonoBehaviour
{
    private readonly List<CooldownData> cooldowns = new List<CooldownData>();

    private void Update()
    {
        ProcessCoolDowns();
    }

    public void PutOnCooldown(IHasCooldown cooldown)
    {
        cooldowns.Add(new CooldownData(cooldown));
    }

    public bool IsOnCooldown(int id)
    {
        foreach(CooldownData cooldown in cooldowns)
        {
            if(cooldown.ID == id) { return true; }
        }
        return false;
    }

    public float GetRemainingDuration(int id)
    {
        foreach(CooldownData cooldown in cooldowns)
        {
            if(cooldown.ID != id) { continue; }
            return cooldown.remainingTime;
        }
        return 0f;
    }

    private void ProcessCoolDowns()
    {
        float deltaTime = Time.deltaTime;

        for (int i = cooldowns.Count - 1; i >= 0; i--)
        {
            if (cooldowns[i].DecrementationCooldown(deltaTime))
            {
                cooldowns.RemoveAt(i);
            }
        }
    }
}

public class CooldownData
{
    public CooldownData(IHasCooldown cooldown)
    {
        ID = cooldown.ID;
        remainingTime = cooldown.CooldownDuration;
    }

    public int ID { get; }
    public float remainingTime { get; private set; }

    public bool DecrementationCooldown(float deltaTime)
    {
        remainingTime = Mathf.Max(remainingTime - deltaTime, 0f);

        return remainingTime == 0f; 
    }
}
