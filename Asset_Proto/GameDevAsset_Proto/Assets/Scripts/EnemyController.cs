using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody2D theRB;
    public float moveSpeed;
    public SpriteRenderer theBody;

    [Header("AI")]
    public float rangeToChase;
    private Vector3 moveDirection;
    public bool shouldShoot;
    public GameObject enemyProjectile;
    public Transform firePoint;
    public float fireRate;
    public float fireCounter;
    public float shootRange = 5;

    [Header("Animation")]
    public Animator enemyAnim;

    [Header("Stats")]
    public int enemyHealth = 150;

    [Header("FX")]
    public GameObject[] deathSplats;
    public GameObject deathPoof;
    public GameObject hitFX;

    [Header("Sound")]
    public int enemyHitSound;
    public int enemyDeathSound;
    public int enemyShootSound;


    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            //Movement
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= rangeToChase)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                moveDirection = Vector3.zero;
            }


            moveDirection.Normalize(); //MoveDirection Normalization
            theRB.velocity = moveDirection * moveSpeed; // Velocity Setting


            //AI should Shoot
            if (shouldShoot && Vector3.Distance(PlayerController.instance.transform.position, transform.position) < shootRange)
            {
                fireCounter -= Time.deltaTime;
                if (fireCounter <= 0)
                {
                    fireCounter = fireRate;
                    Instantiate(enemyProjectile, firePoint.position, enemyProjectile.transform.rotation * Quaternion.Inverse(firePoint.rotation));
                    AudioManager.instance.PlaySFX(enemyShootSound);
                }
            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }


        //Animation
        if (moveDirection != Vector3.zero)
        {
            enemyAnim.SetBool("isMoving", true);
        }
        else
        {
            enemyAnim.SetBool("isMoving", false);
        }
    }

    public void DamageEnemy(int amountToDeal)
    {
        enemyHealth -= amountToDeal;

        Instantiate(hitFX, transform.position, transform.rotation);

        AudioManager.instance.PlaySFX(enemyHitSound);
        //Debug.Log(enemyHealth);
        //Debug.Log(amountToDeal);

        if (enemyHealth <= 0)
        {
            Destroy(gameObject);

            int selectedSplat = Random.Range(0, deathSplats.Length);

            int rotation = Random.Range(0, 4);


            Instantiate(deathPoof, transform.position, transform.rotation); //Poof FX
            Instantiate(deathSplats[selectedSplat], transform.position, Quaternion.Euler(0f, 0f, rotation * 90)); // Random splatter with random rotation
            AudioManager.instance.PlaySFX(enemyDeathSound);
        }
    }


}
