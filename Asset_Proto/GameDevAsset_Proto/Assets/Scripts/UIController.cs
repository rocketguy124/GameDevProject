using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("Health")]
    public Slider healthSlider;
    public Text healthText;
    public GameObject deathScreen;

    [Header("Gold")]
    public Text goldText;

    [Header("Fade")]
    public Image fadeScreen;
    public float fadeSpeed;
    private bool fadingToBlack, fadingFromBlack;

    [Header("WeaponElements")]
    public Image currentStaff;
    public Text staffText;

    [Header("SceneManagement")]
    public string newGameScene;
    public string mainMenuScene;
    public GameObject pauseMenu, mapDisplay, fullmapText;

    [Header("Boss")]
    public Slider bossHealthBar;



    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        fadingFromBlack = true;
        fadingToBlack = false;

        currentStaff.sprite = PlayerController.instance.availableStaffs[PlayerController.instance.currentStaff].staffUI;
        staffText.text = PlayerController.instance.availableStaffs[PlayerController.instance.currentStaff].weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                fadingFromBlack = false;
            }
        }
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                fadingToBlack = false;
            }
        }
    }
    public void StartFadeToBlack()
    {
        fadingToBlack = true;
        fadingFromBlack = false;
    }
    public void newGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGameScene);
        Destroy(PlayerController.instance.gameObject);
        Destroy(UIController.instance.gameObject);
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);
        Destroy(PlayerController.instance.gameObject);
        Destroy(UIController.instance.gameObject);


    }
    public void Resume()
    {
        LevelManager.instance.PauseUnPause();
    }
}
