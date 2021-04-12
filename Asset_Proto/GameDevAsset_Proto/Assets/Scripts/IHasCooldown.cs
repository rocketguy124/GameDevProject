using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasCooldown 
{
    int ID { get; }
    float CooldownDuration { get; }
}
