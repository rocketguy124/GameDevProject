using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed;

    private Vector3 direction;
    [Header("Sound")]
    public int enemyProjectileImpactSound;
    private Rigidbody2D theRB;

    public float TimeBeforeAffected; //The time after the object spawns until it will be affected by the timestop(for projectiles etc)
    private TimeManager timemanager;
    private Vector3 recordedVelocity;
    private float recordedMagnitude;

    private float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    private bool IsStopped;
    public TimeBody bossProjectileTimeBody;


    // Start is called before the first frame update
    void Start()
    {
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();

        theRB = GetComponent<Rigidbody2D>();
        direction = transform.right;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {

        if (!BossController.instance.gameObject.activeInHierarchy) // Bullets dissapear with Boss's death
        {
            Destroy(gameObject);
        }
        TimeBeforeAffectedTimer -= Time.deltaTime; // minus 1 per second
        if (TimeBeforeAffectedTimer <= 0f)
        {
            CanBeAffected = true; // Will be affected by timestop
        }
        if (!timemanager.TimeIsStopped)
        {
            theRB.velocity = direction * speed * Time.deltaTime;
            Debug.Log(theRB.velocity);
        }
        if (CanBeAffected && timemanager.TimeIsStopped && !IsStopped)
        {
            if (theRB.velocity.magnitude >= 0f) //If Object is moving
            {
                recordedVelocity = theRB.velocity.normalized; //records direction of movement
                recordedMagnitude = theRB.velocity.magnitude; // records magitude of movement

                theRB.velocity = Vector3.zero; //makes the rigidbody stop moving
                theRB.isKinematic = true; //not affected by forces
                bossProjectileTimeBody.IsStopped = true; // prevents this from looping
                

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealthController.instance.damagePlayer();
        }
        Debug.Log(collision.name);
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(enemyProjectileImpactSound);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject, 3f);
    }
}
