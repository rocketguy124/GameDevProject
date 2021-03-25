using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [Header("Health")]
    public int currentHealth;
    public int maxHealth;

    [Header("Invincibility")]
    public float invincLength;
    public float invincCount;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincCount > 0)
        {
            invincCount -= Time.deltaTime; //Counting down invincibility

            if(invincCount <= 0)
            {
                PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 1f); //Making Player transparent to show invincibility

            }
        }
    }
    public void damagePlayer()
    {
        if (invincCount <= 0)
        {

            currentHealth--;

            invincCount = invincLength; //setting invincibility timer

            PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 0.5f); //Making Player transparent to show invincibility
            if (currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false);

                UIController.instance.deathScreen.SetActive(true);
            }
            UIController.instance.healthSlider.value = currentHealth;

            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }
    public void MakeInvincible(float timeAmount)
    {
        invincCount = timeAmount;
        PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 0.5f); //Making Player transparent to show invincibility


    }
}
