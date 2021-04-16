using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;

    public int currentHealth;
    public GameObject deathFX;
    public GameObject levelExit;
    public GameObject hitFX;

    public BossAction[] actions;
    private int currentAction;
    private float actionCounter;
    private float shotCounter;
    public Rigidbody2D theRB;
    public Vector2 moveDirection;

    public BossPhase[] phases;
    public int currentPhase;

    private float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    private bool IsStopped;
    public TimeBody enemyTimeBody;
    public float TimeBeforeAffected; //The time after the object spawns until it will be affected by the timestop(for projectiles etc)
    private TimeManager timemanager;
    private Vector3 recordedVelocity;
    private float recordedMagnitude;






    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();


        actions = phases[currentPhase].actions;
        actionCounter = actions[currentAction].actionLength;

        UIController.instance.bossHealthBar.maxValue = currentHealth;
        UIController.instance.bossHealthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        TimeBeforeAffectedTimer -= Time.deltaTime; // minus 1 per second

        if (TimeBeforeAffectedTimer <= 0f)
        {
            CanBeAffected = true; // Will be affected by timestop
        }
        if (!timemanager.TimeIsStopped)
        {
            if (actionCounter > 0)
            {
                actionCounter -= Time.deltaTime;

                //Movement
                moveDirection = Vector2.zero;

                if (actions[currentAction].shouldMove)
                {
                    if (actions[currentAction].shouldChase)
                    {
                        moveDirection = PlayerController.instance.transform.position - transform.position;
                        moveDirection.Normalize();
                    }

                    if (actions[currentAction].moveToPoints && Vector3.Distance(transform.position, actions[currentAction].pointToMoveTo.position) > 0.5f)
                    {
                        moveDirection = actions[currentAction].pointToMoveTo.position - transform.position;
                        moveDirection.Normalize();
                    }
                }

                theRB.velocity = moveDirection * actions[currentAction].moveSpeed;

                //Shooting
                if (actions[currentAction].shouldShoot)
                {
                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        shotCounter = actions[currentAction].timeBetweenShots;
                        foreach (Transform t in actions[currentAction].shotPoints)
                        {
                            Instantiate(actions[currentAction].thingToShoot, t.position, t.rotation);
                        }
                    }
                }
            }
            else
            {
                currentAction++;
                if (currentAction >= actions.Length)
                {
                    currentAction = 0;
                }
                actionCounter = actions[currentAction].actionLength;

            }
        }
        if (CanBeAffected && timemanager.TimeIsStopped && !IsStopped)
        {
            if (theRB.velocity.magnitude >= 0f) //If Object is moving
            {
                recordedVelocity = theRB.velocity.normalized; //records direction of movement
                recordedMagnitude = theRB.velocity.magnitude; // records magitude of movement

                theRB.velocity = Vector3.zero; //makes the rigidbody stop moving
                theRB.isKinematic = true; //not affected by forces
                enemyTimeBody.IsStopped = true; // prevents this from looping
                //enemyAnim.enabled = false;
            }
        }
    }
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        //Instantiate(hitFX, transform.position, transform.rotation);

        if (currentHealth <= 0)
        {
            

            if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)//Prevent player from Instantly moving on
            {
                levelExit.transform.position += new Vector3(4f, 0f, 0f);
            }
            levelExit.SetActive(true); //Exit Appearing after Boss Death

            for (int i = 0; i < 5; i++)
            {
                float randomxPosCoord = Random.Range(0f, 3f);
                float randomyPosCoord = Random.Range(0f, 3f);

                Instantiate(deathFX, transform.position + new Vector3(randomxPosCoord, randomyPosCoord, 0f), transform.rotation);//Boss Death effects
            }
            
            gameObject.SetActive(false);//Boss Deactivated
            UIController.instance.bossHealthBar.gameObject.SetActive(false);

        }
        else
        {
            if(currentHealth <= phases[currentPhase].endPhasehealth && currentPhase < phases.Length-1)
            {
                currentPhase++;
                actions = phases[currentPhase].actions;
                currentAction = 0;
                actionCounter = actions[currentAction].actionLength;
            }
        }
        UIController.instance.bossHealthBar.value = currentHealth;

    }

}

[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionLength;

    [Header("Checks")]
    public bool shouldMove;
    public bool shouldChase;
    public bool moveToPoints;
    public bool shouldShoot;

    [Header("Movement")]
    public float moveSpeed;
    public Transform pointToMoveTo;

    [Header("Shooting")]
    public GameObject thingToShoot;
    public float timeBetweenShots;
    public Transform[] shotPoints;


}
[System.Serializable]
public class BossPhase
{
    [Header("Phase")]
    public BossAction[] actions;

    public int endPhasehealth;

}
