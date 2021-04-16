using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToLoad = 4f;
    public string nextLevel;

    public bool isPaused;

    public int currentGold;

    public Transform startPoint;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.transform.position = startPoint.position;
        PlayerController.instance.canMove = true;

        currentGold = CharacterTracker.instance.currentGold;
        Time.timeScale = 1f;
        UIController.instance.goldText.text = currentGold.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnPause();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Boss1");
        }
    }

    public IEnumerator LevelEnd()
    {
        AudioManager.instance.PlayWinMusic();

        PlayerController.instance.canMove = false;

        UIController.instance.StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        CharacterTracker.instance.currentGold = currentGold;
        CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;

        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnPause()
    {
        if (!isPaused)
        {
            UIController.instance.pauseMenu.SetActive(true);
            isPaused = true;

            Time.timeScale = 0f;
        }
        else
        {
            UIController.instance.pauseMenu.SetActive(false);
            isPaused = false;

            Time.timeScale = 1f;
        }
    }
    public void GetGold(int amount)
    {
        currentGold += amount;

        UIController.instance.goldText.text = currentGold.ToString();
    }
    public void SpendGold(int amount)
    {
        currentGold -= amount;
        if(currentGold < 0)
        {
            currentGold = 0;
        }
        UIController.instance.goldText.text = currentGold.ToString();
    }
}
