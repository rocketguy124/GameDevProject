using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public GameObject buyingMessage;

    private bool inZone;

    public bool isHealthRestore, isHealthUpgrade, isWeapon, isStatIncrease;

    public int itemCost;

    public int healthUpgradeAmount;

    public int buyItemSound;
    public int cantBuyItemSound;

    public Staff[] potentialStaffs;
    private Staff theStaff;
    public SpriteRenderer staffSprite;
    public Text informationText;

    // Start is called before the first frame update
    void Start()
    {
        if (isWeapon)
        {
            int staffSelect = Random.Range(0, potentialStaffs.Length);
            theStaff = potentialStaffs[staffSelect];

            staffSprite.sprite = theStaff.shopSprite;
            informationText.text = "Buy " + theStaff.weaponName + "\n - " + theStaff.weaponCost.ToString() + " - ";
            itemCost = theStaff.weaponCost;
        }
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
                    if (isWeapon)
                    {
                        Staff staffClone = Instantiate(theStaff);
                        staffClone.transform.parent = PlayerController.instance.staffArm; // make the clone a child of the rotate point
                        staffClone.transform.position = PlayerController.instance.staffArm.position;
                        staffClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                        staffClone.transform.localScale = new Vector3(1f, 1f, 1f); //Weird scaling bug

                        PlayerController.instance.availableStaffs.Add(staffClone);
                        PlayerController.instance.currentStaff = PlayerController.instance.availableStaffs.Count - 1;
                        PlayerController.instance.SwitchStaff();
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
