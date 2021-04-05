using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed;

    private Vector3 direction;
    [Header("Sound")]
    public int enemyProjectileImpactSound;


    // Start is called before the first frame update
    void Start()
    {
        direction = transform.right;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (!BossController.instance.gameObject.activeInHierarchy) // Bullets dissapear with Boss's death
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealthController.instance.damagePlayer();
        }

        Destroy(gameObject);
        AudioManager.instance.PlaySFX(enemyProjectileImpactSound);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject, 3f);
    }
}
