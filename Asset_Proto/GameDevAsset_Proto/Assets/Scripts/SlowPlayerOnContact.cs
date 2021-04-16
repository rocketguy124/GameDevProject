using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPlayerOnContact : MonoBehaviour
{
    [SerializeField]private float slowPercent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController.instance.SlowPlayer(slowPercent);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController.instance.MakePlayerNormalSpeed();
        }
    }
}
