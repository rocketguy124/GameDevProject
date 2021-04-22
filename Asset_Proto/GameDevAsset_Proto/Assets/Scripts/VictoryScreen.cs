using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public float waitForAnyKey = 2f;

    public GameObject anyKeyText;

    public string mainMenuScene;

    public bool isOutro;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        if (PlayerController.instance != null)
        {
            Destroy(PlayerController.instance.gameObject);
        }
        if (UIController.instance != null)
        {
            Destroy(UIController.instance.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOutro)
        {
            if (waitForAnyKey > 0)
            {
                waitForAnyKey -= Time.deltaTime;
                if (waitForAnyKey <= 0)
                {
                    anyKeyText.SetActive(true);
                }
            }
            else
            {
                if (Input.anyKeyDown)
                {
                    SceneManager.LoadScene(mainMenuScene);
                }
            }
        }
    }
}
