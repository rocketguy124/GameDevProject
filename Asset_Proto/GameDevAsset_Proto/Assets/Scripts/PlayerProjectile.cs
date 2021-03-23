using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 7.5f;

    public Rigidbody2D theRB;

    public GameObject ImpactFX;

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
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject, 2f);
    }
}
