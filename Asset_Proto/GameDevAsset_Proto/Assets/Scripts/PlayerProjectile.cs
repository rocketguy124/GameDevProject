using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 7.5f;

    public Rigidbody2D theRB;

    public GameObject ImpactFX;

    public int damageToGive = 50;
    [Header("Sound")]
    public int impactSound;


    public float TimeBeforeAffected; //The time after the object spawns until it will be affected by the timestop(for projectiles etc)
    private TimeManager timemanager;
    private Vector3 recordedVelocity;
    private float recordedMagnitude;

    private float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    private bool IsStopped;
    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();

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
            theRB.velocity = transform.right * speed;
        }
        if (CanBeAffected && timemanager.TimeIsStopped && !IsStopped)
        {
            if (theRB.velocity.magnitude >= 0f) //If Object is moving
            {
                recordedVelocity = theRB.velocity.normalized; //records direction of movement
                recordedMagnitude = theRB.velocity.magnitude; // records magitude of movement

                theRB.velocity = Vector3.zero; //makes the rigidbody stop moving
                theRB.isKinematic = true; //not affected by forces
                IsStopped = true; // prevents this from looping
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(ImpactFX, transform.position, transform.rotation);
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(impactSound);
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }
        if (collision.gameObject.tag == "Boss")
        {
            BossController.instance.TakeDamage(damageToGive);
            Instantiate(BossController.instance.hitFX, transform.position, transform.rotation);
        }

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject, 1f);
    }

    public int GetDamage()
    {
        return damageToGive;
    }
}
