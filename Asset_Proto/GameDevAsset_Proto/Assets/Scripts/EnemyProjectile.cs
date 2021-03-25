using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    //public Rigidbody2D theRB;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        //theRB = GetComponent<Rigidbody2D>();
        direction = PlayerController.instance.transform.position - transform.position;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        //theRB.velocity = direction * speed * Time.deltaTime;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerHealthController.instance.damagePlayer();
        }

        Destroy(gameObject);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject, 3f);
    }
}
