using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody2D theRB;
    public float moveSpeed;
    public SpriteRenderer theBody;

    [Header("AI")]
    private bool isAttacked = false;
    [SerializeField]private bool isActive = false;
    [SerializeField] private bool hasBeenRevealed = false;
    public bool canSpawnMore;
    private bool hasSpawned = false;
    public GameObject spawnedMob;
    public bool shouldChase;
    public bool shouldShoot;
    public bool shouldRunAway;
    public bool shouldWander;
    public bool shouldPatrol;

    
    public float rangeToChase;
    private Vector3 moveDirection;
    
    public float runAwayRange;
    public float wanderMoveTime, wanderPauseTime;
    private float wanderMoveCounter, wanderPauseCounter;
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
    public GameObject healthBarUI;
    public Slider healthSlider;
    [Header("HitText")]
    public GameObject damageTextPrefab;
    private string textToDisplay;

    [Header("Stats")]
    public int enemyHealth = 150;
    public int maxHealth;

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

    public float TimeBeforeAffected; //The time after the object spawns until it will be affected by the timestop(for projectiles etc)
    private TimeManager timemanager;
    private Vector3 recordedVelocity;
    private float recordedMagnitude;

    private float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    private bool IsStopped;
    public TimeBody enemyTimeBody;

    private float t = 5f;

    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
        healthSlider.maxValue = enemyHealth;
        healthSlider.value = enemyHealth;
        maxHealth = enemyHealth;
        healthBarUI.SetActive(false);

        gameObject.SetActive(false);

        if (isActive)
        {
            MakeActive();
        }
        



        if (shouldWander)
        {
            wanderPauseCounter = Random.Range(wanderPauseTime * 0.75f, wanderPauseTime * 1.25f);

        }
    }

    // Update is called once per frame
    void Update()
    {
        TimeBeforeAffectedTimer -= Time.deltaTime; // minus 1 per second
        if(gameObject.activeSelf && !hasBeenRevealed)
        {
            
            hasBeenRevealed = true;
            Instantiate(deathPoof, transform.position, transform.rotation); //Poof FX
            
            StartCoroutine(DelayedMovement());
        }
        
        if (enemyHealth< maxHealth)
        {
            healthBarUI.SetActive(true);
            isAttacked = true;
        }
        if (TimeBeforeAffectedTimer <= 0f)
        {
            CanBeAffected = true; // Will be affected by timestop
        }
        if (!timemanager.TimeIsStopped)
        {
            if (isActive)
            {
                if (theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
                {
                    moveDirection = Vector3.zero;

                    //Movement
                    if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= rangeToChase && shouldChase || shouldChase && isAttacked)
                    {
                        if (canSpawnMore)
                        {
                            if (!hasSpawned)
                            {
                                Instantiate(spawnedMob, firePoint.position, firePoint.rotation);
                                Instantiate(deathPoof, firePoint.position, firePoint.rotation);
                                hasSpawned = true;
                            }
                        }
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

                        if (canSpawnMore)
                        {
                            if (!hasSpawned)
                            {
                                Instantiate(spawnedMob, firePoint.position, firePoint.rotation);
                                Instantiate(deathPoof, firePoint.position, firePoint.rotation);
                                hasSpawned = true;
                            }
                        }
                    }




                    moveDirection.Normalize(); //MoveDirection Normalization
                    theRB.velocity = moveDirection * moveSpeed; // Velocity Setting


                    //AI should Shoot
                    if (shouldShoot && Vector3.Distance(PlayerController.instance.transform.position, transform.position) < shootRange || shouldShoot && isAttacked)
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

                enemyAnim.enabled = true;
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
        }
        //Debug.Log(CanBeAffected + ", " + timemanager.TimeIsStopped + ", " + !IsStopped);
        if (CanBeAffected && timemanager.TimeIsStopped && !IsStopped)
        {
            if (theRB.velocity.magnitude >= 0f) //If Object is moving
            {
                recordedVelocity = theRB.velocity.normalized; //records direction of movement
                recordedMagnitude = theRB.velocity.magnitude; // records magitude of movement

                theRB.velocity = Vector3.zero; //makes the rigidbody stop moving
                theRB.isKinematic = true; //not affected by forces
                enemyTimeBody.IsStopped = true; // prevents this from looping
                enemyAnim.enabled = false;
            }
        }
        
    }

    public void DamageEnemy(int amountToDeal)
    {
        enemyHealth -= amountToDeal;

        StartCoroutine(EnemyBlinkingRed());

        healthSlider.value = enemyHealth;

        GameObject hitText = Instantiate(damageTextPrefab, transform.position, transform.rotation);
        hitText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(amountToDeal.ToString());
        hitText.transform.GetChild(0).GetComponent<TextMeshPro>().faceColor = Color.yellow;

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

    public void MakeActive()
    {
        gameObject.SetActive(true);
    }


    public IEnumerator EnemyBlinkingRed()
    {
        //float t = 0.3f;
        float tempRedValue = theBody.color.r;
        float tempGreenValue = theBody.color.g;
        float tempBlueValue = theBody.color.b;

        theBody.color = new Color(1f, 0, 0, 1f); //blinking Enemy red after getting hit
        
        yield return new WaitForSeconds(0.1f);
        theBody.color = new Color(tempRedValue, tempGreenValue, tempBlueValue, 1f);
    }

    public IEnumerator DelayedMovement()
    {
        theRB.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.8f);
        isActive = true;
        
    }
}
