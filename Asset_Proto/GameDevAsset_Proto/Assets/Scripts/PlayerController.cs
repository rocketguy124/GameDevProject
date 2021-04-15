using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour, IHasCooldown
{
    public static PlayerController instance;

    [Header("Movement")]
    [HideInInspector]
    public bool canMove = true;
    public float moveSpeed;
    private Vector2 moveInput;
    public Rigidbody2D theRB;
    private float activeMovespeed;
    public float dodgeSpeed = 8f, dodgeLength = 0.5f, dodgeCooldown = 1f, dodgeInvincTime = 0.5f;
    private float dodgeCooldownCounter;
    [HideInInspector]
    public float dodgeCounter;
    public float speedBoostCounter;
    public float speedBoostLength = 2f;
    public float speedBoostSpeed = 8f;

    [Header("Aiming")]
    public Transform staffArm;
    //private Camera theCam;

    [Header("Animation")]
    public Animator bodyAnim;
    public Animator wholePlayerAnim;
    public SpriteRenderer bodySR;

    [Header("Weapons")]
    public List<Staff> availableStaffs = new List<Staff>();
    [HideInInspector]
    public int currentStaff;

    [Header("Inventory")]
    private Inventory inventory;
    [SerializeField] private UIInventory uiInventory;
    public GameObject UIInventory;
    public Transform UIInventory_In, UIInventory_Out;
    private bool Inv_In, Inv_Out;
    public float InvButtonCD = 0.5f;
    public float InvButtonCDCounter;
    public float InvButtonCounter;

    [Header("Time Stopping")]
    private TimeManager timemanager;
    public float timeStopCoolDown;
    public float timeStopCoolDownCounter;
    public float timeStopCounter;
    public float timeStopLength;
    public int timeStopSound;
    public int timeResumeSound;

    [Header("Time Stopping Cooldown")]
    [SerializeField] public CooldownSystem cooldownSystem = null;
    [SerializeField] private int id = 1;
    [SerializeField] private float cooldownDuration = 10f;

    public int ID => id;
    public float CooldownDuration => cooldownDuration;


    /*
    [Header("Projectiles")]
    public GameObject projectileToFire;
    public Transform firePoint;
    public float timeBetweenShots;
    private float shotCounter;*/

    [Header("Sound")]
    public int playerDodgeSound;
    //public int playerShootSound;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    // Start is called before the first frame update
    void Start()
    {
        theRB = gameObject.GetComponent<Rigidbody2D>();
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();

        //theCam = Camera.main;
        Inv_Out = true;
        InvButtonCD = 1f;
        activeMovespeed = moveSpeed;

        UIController.instance.currentStaff.sprite = availableStaffs[currentStaff].staffUI;
        UIController.instance.staffText.text = availableStaffs[currentStaff].weaponName;

        inventory = new Inventory(UseItem);
        uiInventory.SetInventory(inventory);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();

            theRB.velocity = moveInput * activeMovespeed;

            Vector3 mousePosition = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCam.WorldToScreenPoint(transform.localPosition);

            //Flipping player and staff based on aim
            if (mousePosition.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                staffArm.localScale = new Vector3(-.7f, .7f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                staffArm.localScale = new Vector3(.7f, .7f, 1f);
            }

            //rotation for staff to aim at mouse
            Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            staffArm.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));

            

            if (Input.GetKeyDown(KeyCode.Q) && !cooldownSystem.IsOnCooldown(id)) //Stop Time when Q is pressed
            {
                cooldownSystem.PutOnCooldown(this);
                UIController.instance.timeStopCount = timeStopLength;
                PostProcessingEffects.instance.TimeStopping();
                StartCoroutine(TimeStopAbility());
                //Grayscale.enabled = true;
            }
            if (Input.GetKeyDown(KeyCode.E) && timemanager.TimeIsStopped)  //Continue Time when E is pressed
            {
                
                //Grayscale.enabled = false;

            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if(availableStaffs.Count > 0)
                {
                    currentStaff++;
                    if(currentStaff >= availableStaffs.Count)
                    {
                        currentStaff = 0;
                    }
                    SwitchStaff();
                }
                else
                {
                    Debug.LogError("Player has no Weapon");
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dodgeCounter <= 0 && dodgeCooldownCounter <= 0)
                {
                    activeMovespeed = dodgeSpeed;
                    dodgeCounter = dodgeLength;
                    bodyAnim.SetTrigger("Dodge");
                    PlayerHealthController.instance.MakeInvincible(dodgeInvincTime);
                    AudioManager.instance.PlaySFX(playerDodgeSound);
                }
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                if(InvButtonCounter <= 0 && InvButtonCDCounter <= 0)
                {
                    if (Inv_Out)
                    {
                        LeanTween.moveLocalX(UIInventory, 533f, 0.5f).setOnComplete(InventoryMoveComplete);
                    }
                    if (!Inv_Out)
                    {
                        LeanTween.moveLocalX(UIInventory, 1255f, 0.5f).setOnComplete(InventoryMoveComplete);
                    }
                }

                // UIInventory.transform.position = Vector3.MoveTowards(transform.position, new Vector3(UIInventory_In.position.x, UIInventory_In.position.y, -10f), 50 * Time.deltaTime);
            }
            if (InvButtonCounter > 0)
            {
                InvButtonCounter -= Time.deltaTime;
                if (InvButtonCounter <= 0)
                {
                    InvButtonCDCounter = InvButtonCD;
                }
            }
            if(InvButtonCDCounter > 0)
            {
                InvButtonCDCounter -= Time.deltaTime;
            }
            if (dodgeCounter > 0)
            {
                dodgeCounter -= Time.deltaTime;
                if (dodgeCounter <= 0)
                {
                    activeMovespeed = moveSpeed;
                    dodgeCooldownCounter = dodgeCooldown;
                }
            }
            if (dodgeCooldownCounter > 0)
            {
                dodgeCooldownCounter -= Time.deltaTime;
            }

            if (speedBoostCounter > 0)
            {
                activeMovespeed = speedBoostSpeed;
                speedBoostCounter -= Time.deltaTime;
                if (speedBoostCounter <= 0)
                {
                    activeMovespeed = moveSpeed;
                }
            }





            //Animation Code
            if (moveInput != Vector2.zero)
            {
                bodyAnim.SetBool("isMoving", true);
            }
            else
            {
                bodyAnim.SetBool("isMoving", false);
            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
            bodyAnim.SetBool("isMoving", false);
        }
    }
    public void SwitchStaff()
    {
        foreach(Staff theStaff in availableStaffs)
        {
            theStaff.gameObject.SetActive(false);
        }

        availableStaffs[currentStaff].gameObject.SetActive(true);

        UIController.instance.currentStaff.sprite = availableStaffs[currentStaff].staffUI;
        UIController.instance.staffText.text = availableStaffs[currentStaff].weaponName;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if(itemWorld != null)
        {
            AudioManager.instance.PlaySFX(25);
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                AudioManager.instance.PlaySFX(7);
                PlayerHealthController.instance.HealPlayer(5);
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                break;

            case Item.ItemType.MajorHealthPotion:
                AudioManager.instance.PlaySFX(7);
                PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);
                inventory.RemoveItem(new Item { itemType = Item.ItemType.MajorHealthPotion, amount = 1 });
                break;

            case Item.ItemType.SpeedPotion:
                AudioManager.instance.PlaySFX(8);
                speedBoostCounter = 2f;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.SpeedPotion, amount = 1 });
                break;

            case Item.ItemType.InvincibilityPotion:
                AudioManager.instance.PlaySFX(20);
                PlayerHealthController.instance.MakeInvincible(2f);
                inventory.RemoveItem(new Item { itemType = Item.ItemType.InvincibilityPotion, amount = 1 });
                break;
        }
    }

    void InventoryMoveComplete()
    {
        if (!Inv_Out)
        {
            Inv_Out = true;
            //Debug.Log("Inventory Complete Inv should be true");
        }
        else
        {
            Inv_Out = false;
            //Debug.Log("Inventory Complete Inv should be false");
        }
        
    }

    public IEnumerator TimeStopAbility()
    {
        timemanager.StopTime();
        AudioManager.instance.PlaySFX(23);
        AudioManager.instance.levelMusic.Pause();

        yield return new WaitForSeconds(timeStopLength);

        timemanager.ContinueTime();
        PostProcessingEffects.instance.TimeResuming();

        AudioManager.instance.PlaySFX(24);
        AudioManager.instance.levelMusic.Play();



    }


}
