using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public bool canDie = true;

    [Header("Health")]
    public int currentHealth;
    public int maxHealth;

    [Header("Invincibility")]
    public float invincLength;
    public float invincCount;

    [Header("HitText")]
    public GameObject damageTextPrefab;
    private string textToDisplay;

    [Header("Sound")]
    public int playerDeathSound;
    public int playerHurtSound;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = CharacterTracker.instance.maxHealth;

        currentHealth = CharacterTracker.instance.currentHealth;

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
                PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 1f); //Making Player opaque to show hittable again

            }
        }
    }
    public void damagePlayer(int dmgToGive)
    {
        if (invincCount <= 0)
        {

            currentHealth--;

            GameObject hitText = Instantiate(damageTextPrefab, PlayerController.instance.transform.position, transform.rotation);
            hitText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(dmgToGive.ToString());
            hitText.transform.GetChild(0).GetComponent<TextMeshPro>().faceColor = Color.red;
            AudioManager.instance.PlaySFX(playerHurtSound);

            invincCount = invincLength; //setting invincibility timer

            PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 0.5f); //Making Player transparent to show invincibility
            if (currentHealth <= 0 && canDie)
            {
                PlayerController.instance.gameObject.SetActive(false);

                UIController.instance.deathScreen.SetActive(true);

                AudioManager.instance.PlaySFX(playerDeathSound);
                AudioManager.instance.PlayGameoverMusic();
            }
            if(!canDie && currentHealth < 1)
            {
                currentHealth = 1;
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
    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        GameObject hitText = Instantiate(damageTextPrefab, PlayerController.instance.transform.position, transform.rotation);
        hitText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(healAmount.ToString());
        hitText.transform.GetChild(0).GetComponent<TextMeshPro>().faceColor = Color.green;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIController.instance.healthSlider.value = currentHealth;

        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;

        UIController.instance.healthSlider.value = currentHealth;

        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
