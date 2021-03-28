using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    public int value = 1;

    public float timeTillCollectable = 0.5f;

    public int goldCollectSound = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeTillCollectable > 0)
        {
            timeTillCollectable -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && timeTillCollectable <= 0)
        {
            LevelManager.instance.GetGold(value);
            AudioManager.instance.PlaySFX(goldCollectSound);
            Destroy(gameObject);
        }
    }
}
