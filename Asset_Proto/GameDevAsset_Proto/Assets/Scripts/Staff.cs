using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [Header("Projectiles")]
    public GameObject projectileToFire;
    public Transform firePoint;
    public float timeBetweenShots;
    private float shotCounter;

    [Header("Weapon Info")]
    public string weaponName;
    public GameObject staffUI;

    [Header("Sound")]
    public int playerShootSound;

    // Start is called before the first frame update
    void Start()
    {
        
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
                    Instantiate(projectileToFire, firePoint.transform.position, firePoint.transform.rotation);
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
