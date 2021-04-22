using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public Rigidbody2D theRB;

    public int DamageToGive = 2;

    private Vector3 direction;
    [Header("Sound")]
    public int enemyProjectileImpactSound;

    public float TimeBeforeAffected; //The time after the object spawns until it will be affected by the timestop(for projectiles etc)
    private TimeManager timemanager;
    private Vector3 recordedVelocity;
    private float recordedMagnitude;

    private float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    private bool IsStopped;
    public TimeBody enemyProjectileTimeBody;

    public Animator enemyAnim;


    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();

        direction = PlayerController.instance.transform.position - transform.position;
        direction.Normalize();
        DamageToGive += DifficultySettingModifier.instance.damageModifier;
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
            theRB.velocity = (direction.normalized * speed);
            if (enemyAnim != null)
            {
                enemyAnim.enabled = true;
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
                enemyProjectileTimeBody.IsStopped = true; // prevents this from looping
                if(theRB.velocity.Equals( Vector3.zero))
                {
                    if(enemyAnim != null)
                    enemyAnim.enabled = false;
                    
                }
                
            }
        }

        //Debug.Log(theRB.velocity);
        //transform.position += direction * speed * Time.deltaTime;
    }

    public void ContinueTime()
    {
        theRB.isKinematic = false;
        IsStopped = false;
        theRB.velocity = recordedVelocity * recordedMagnitude; //Adds back the recorded velocity when time continues
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerHealthController.instance.damagePlayer(DamageToGive);
        }

        Destroy(gameObject);
        AudioManager.instance.PlaySFX(enemyProjectileImpactSound);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject, 3f);
    }
    public int GetDamage()
    {
        return DamageToGive;
    }
}
