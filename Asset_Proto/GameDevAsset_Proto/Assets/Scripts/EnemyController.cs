using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody2D theRB;
    public float moveSpeed;

    [Header("AI")]
    public float rangeToChase;
    private Vector3 moveDirection;
    public bool shouldShoot;
    public GameObject enemyProjectile;
    public Transform firePoint;
    public float fireRate;
    public float fireCounter;

    [Header("Animation")]
    public Animator enemyAnim;

    [Header("Stats")]
    public int enemyHealth = 150;

    [Header("FX")]
    public GameObject[] deathSplats;
    public GameObject deathPoof;
    public GameObject hitFX;


    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
        //Debug.Log(enemyHealth);
        //Debug.Log(amountToDeal);

        if (enemyHealth <= 0)
        {
            Destroy(gameObject);

            int selectedSplat = Random.Range(0, deathSplats.Length);

            int rotation = Random.Range(0, 4);


            Instantiate(deathPoof, transform.position, transform.rotation); //Poof FX
            Instantiate(deathSplats[selectedSplat], transform.position, Quaternion.Euler(0f,0f, rotation * 90)); // Random splatter with random rotation
        }
    }


}
