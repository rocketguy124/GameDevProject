using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanelHandler : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject InstructionsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwitchPanel(int panelNum)
    {
        switch (panelNum)
        {
            case 0:
                MainMenuPanel.SetActive(true);
                InstructionsPanel.SetActive(false);
                break;

            case 1:
                MainMenuPanel.SetActive(false);
                InstructionsPanel.SetActive(true);
                break;
        }
    }
}
