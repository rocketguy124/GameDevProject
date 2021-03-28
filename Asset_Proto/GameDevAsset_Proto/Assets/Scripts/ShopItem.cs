using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public GameObject buyingMessage;

    private bool inZone;

    public bool isHealthRestore, isHealthUpgrade, isWeapon, isStatIncrease;

    public int itemCost;

    public int healthUpgradeAmount;

    public int buyItemSound;
    public int cantBuyItemSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(LevelManager.instance.currentGold >= itemCost)
                {
                    LevelManager.instance.SpendGold(itemCost);

                    if (isHealthRestore)
                    {
                        PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);
                    }

                    if (isHealthUpgrade)
                    {
                        PlayerHealthController.instance.IncreaseMaxHealth(healthUpgradeAmount);
                    }

                    gameObject.SetActive(false); //remove item after purchase
                    inZone = false;

                    AudioManager.instance.PlaySFX(buyItemSound);
                }
                else
                {
                    AudioManager.instance.PlaySFX(cantBuyItemSound);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            buyingMessage.SetActive(true);
            inZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            buyingMessage.SetActive(false);
            inZone = false;
        }
    }
}
