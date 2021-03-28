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
    public bool shouldChase;
    public float rangeToChase;
    private Vector3 moveDirection;
    public bool shouldShoot;
    public bool shouldRunAway;
    public float runAwayRange;
    public bool shouldWander;
    public float wanderMoveTime, wanderPauseTime;
    private float wanderMoveCounter, wanderPauseCounter;
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentpatrolPoint;
    private Vector3 wanderDirection;
    public GameObject enemyProjectile;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;
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

    [Header("Drops")]
    public bool shouldDrop;
    public GameObject[] itemsToDrop;
    public float dropPercent;


    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        if (shouldWander)
        {
            wanderPauseCounter = Random.Range(wanderPauseTime * 0.75f, wanderPauseTime * 1.25f);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;

            //Movement
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= rangeToChase && shouldChase)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                if (shouldWander)
                {
                    if (wanderMoveCounter > 0)
                    {
                        wanderMoveCounter -= Time.deltaTime;

                        moveDirection = wanderDirection;

                        if (wanderMoveCounter <= 0)
                        {
                            wanderPauseCounter = Random.Range(wanderPauseTime * 0.75f, wanderPauseTime * 1.25f);
                        }
                    }
                    if (wanderPauseCounter > 0)
                    {
                        wanderPauseCounter -= Time.deltaTime;

                        if (wanderPauseCounter <= 0)
                        {
                            wanderMoveCounter = Random.Range(wanderMoveTime * 0.75f, wanderMoveTime * 1.25f);

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }
                if (shouldPatrol)
                {
                    moveDirection = patrolPoints[currentpatrolPoint].position - transform.position;

                    if (Vector3.Distance(transform.position, patrolPoints[currentpatrolPoint].position) < 0.2f)
                    {
                        currentpatrolPoint++;
                        if (currentpatrolPoint >= patrolPoints.Length)
                        {
                            currentpatrolPoint = 0;
                        }
                    }
                }
            }

            if (shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= runAwayRange)
            {
                moveDirection = transform.position - PlayerController.instance.transform.position;

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

            //Dropping Items
            if (shouldDrop)
            {
                float dropChance = Random.Range(0f, 100f);

                if (dropChance <= dropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);
                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
        }
    }


}
