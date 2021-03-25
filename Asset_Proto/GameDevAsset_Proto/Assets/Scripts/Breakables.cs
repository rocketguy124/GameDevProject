using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    [Header("Breaking")]
    public GameObject[] brokenPieces;
    public int maxPiece = 5;

    [Header("Drops")]
    public bool shouldDrop;
    public GameObject[] itemsToDrop;
    public float dropPercent;

    [Header("Sound")]
    public int brokenSound;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SmashBreakable()
    {
        Destroy(gameObject);

        AudioManager.instance.PlaySFX(brokenSound);
        //Pieces
        int piecesToDrop = Random.Range(1, maxPiece);
        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);

            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (PlayerController.instance.dodgeCounter > 0)
            {
                SmashBreakable();
            }
        }
        if(collision.tag == "PlayerProjectile")
        {
            SmashBreakable();
        }
    }
}
