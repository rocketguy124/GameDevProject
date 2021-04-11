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

    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.right * speed;
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
}
