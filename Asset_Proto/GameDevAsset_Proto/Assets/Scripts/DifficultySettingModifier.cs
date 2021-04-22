using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySettingModifier : MonoBehaviour
{
    public static DifficultySettingModifier instance;

    public Dropdown difficultyDD;
    public int damageModifier;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //difficultyToggle.onValueChanged.AddListener
        difficultyDD.value = 1;
        Debug.Log(damageModifier);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void dropChange(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                damageModifier = -1;
                break;

            case 1:
                damageModifier = 0;

                break;

            case 2:
                damageModifier = 1;

                break;

            case 3:
                damageModifier = 1000;

                break;
        }
        Debug.Log(damageModifier);
    }
}
