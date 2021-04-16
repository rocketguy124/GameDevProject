using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaTrigger : MonoBehaviour
{
    public string[] bossNames = new string[] { "EyeZen", "Gunther", "Gimme" };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string ReturnBossNameFromInt(int num)
    {
        return bossNames[num];
    }
}
