﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
        //theCam = Camera.main;

        activeMovespeed = moveSpeed;

        UIController.instance.currentStaff.sprite = availableStaffs[currentStaff].staffUI;
        UIController.instance.staffText.text = availableStaffs[currentStaff].weaponName;
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

            /*
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(projectileToFire, firePoint.transform.position, firePoint.transform.rotation);
                AudioManager.instance.PlaySFX(playerShootSound);
                shotCounter = timeBetweenShots;
            }
            if (Input.GetMouseButton(0))
            {
                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0)
                {
                    Instantiate(projectileToFire, firePoint.transform.position, firePoint.transform.rotation);
                    AudioManager.instance.PlaySFX(playerShootSound);
                    shotCounter = timeBetweenShots;
                }
            }*/

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
}
