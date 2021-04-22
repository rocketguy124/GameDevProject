using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [Header("Projectiles")]
    public GameObject projectileToFire;
    public GameObject additionalProjectileToFire;
    public Transform firePoint;
    public Transform additionalFirePoint;
    public float timeBetweenShots;
    private float shotCounter;
    public int damageToGive;

    [Header("Weapon Info")]
    public string weaponName;
    public Sprite staffUI;
    public int weaponCost;
    public Sprite shopSprite;
    public bool isSplitShot;
    public bool isSwordStaff;

    [Header("Sound")]
    public int playerShootSound;

    // Start is called before the first frame update
    void Start()
    {
        damageToGive = projectileToFire.GetComponent<PlayerProjectile>().damageToGive;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused)
        {
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
            else
            {


                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    if (isSplitShot)
                    {
                        Instantiate(projectileToFire, firePoint.transform.position, firePoint.transform.rotation * Quaternion.Euler(new Vector3(0f, 0f,45)));
                        Instantiate(projectileToFire, firePoint.transform.position, firePoint.transform.rotation * Quaternion.Euler(new Vector3(0f, 0f,-45)));
                    }
                    else if (isSwordStaff)
                    {
                        Instantiate(projectileToFire, firePoint.transform.position, firePoint.transform.rotation * Quaternion.Euler(new Vector3(0f, 0f, -15)));
                        Instantiate(projectileToFire, additionalFirePoint.transform.position, firePoint.transform.rotation * Quaternion.Euler(new Vector3(0f, 0f, 15)));
                    }
                    else
                    {
                        Instantiate(projectileToFire, firePoint.transform.position, firePoint.transform.rotation);
                    }
                    AudioManager.instance.PlaySFX(playerShootSound);
                    shotCounter = timeBetweenShots;

                }

                /*if (Input.GetMouseButton(0))
                {
                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        Instantiate(projectileToFire, firePoint.transform.position, firePoint.transform.rotation);
                        AudioManager.instance.PlaySFX(playerShootSound);
                        shotCounter = timeBetweenShots;
                    }
                }*/
            }
        }
    }
}
