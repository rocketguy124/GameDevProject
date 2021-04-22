using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public Text staffStatsText;

    [Header("SceneManagement")]
    public string newGameScene;
    public string mainMenuScene;
    public GameObject pauseMenu, mapDisplay, fullmapText;

    [Header("Boss")]
    public Slider bossHealthBar;
    public Text bossNameText;

    [Header("Time Stop Cooldown")]
    [SerializeField] private Image timeStopCooldown;
    [SerializeField] private Image timeStopFill;
    [SerializeField] public float timeStopCount;
    [SerializeField] private TMP_Text timeStopCooldownText;



    private void Awake()
    {
        instance = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!CameraController.instance.isTutRoom)
        {
            DontDestroyOnLoad(gameObject);
        }
        fadingFromBlack = true;
        fadingToBlack = false;

        currentStaff.sprite = PlayerController.instance.availableStaffs[PlayerController.instance.currentStaff].staffUI;
        staffText.text = PlayerController.instance.availableStaffs[PlayerController.instance.currentStaff].weaponName;
        staffStatsText.text = "DMG->" + PlayerController.instance.availableStaffs[PlayerController.instance.currentStaff].damageToGive + " FR->" + (1f / PlayerController.instance.availableStaffs[PlayerController.instance.currentStaff].timeBetweenShots).ToString("n2");
    }

    // Update is called once per frame
    void Update()
    {
        if(timeStopCount > 0)
        {
            //timeStopFill.fillAmount -= Time.deltaTime;
            timeStopCount -= Time.deltaTime;
            timeStopFill.fillAmount = timeStopCount * (1/PlayerController.instance.timeStopLength);
            //Debug.Log("timeStopCount: " + timeStopCount);
            //Debug.Log("timeStopFill.fillAmount: " +timeStopFill.fillAmount);
        }
        else
        {
            timeStopFill.fillAmount = 0f;
            //timeStopCount = PlayerController.instance.timeStopLength;
            //Debug.Log("InElse");
        }
        if (PlayerController.instance.cooldownSystem.GetRemainingDuration(1) > 0) // Updating Cooldown Visuals
        {
            
            
            //timeStopCooldown.fillAmount = PlayerController.instance.cooldownSystem.GetRemainingDuration(1)*0.1f; 
            timeStopCooldown.fillAmount = PlayerController.instance.cooldownSystem.GetRemainingDuration(1)*0.1f; 
            timeStopCooldownText.text = Mathf.Floor(PlayerController.instance.cooldownSystem.GetRemainingDuration(1)).ToString();
        }
        
        if (fadingFromBlack) // Fading From black
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                fadingFromBlack = false;
            }
        }
        if (fadingToBlack) // Fading To black
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
    public void StartFadeFromBlack()
    {
        fadingToBlack = false;
        fadingFromBlack = true;
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

    public void Stuck()
    {
        PlayerController.instance.gameObject.transform.position = new Vector3(0, 0, 0);
    }
}
