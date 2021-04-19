using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealUIElementOnTrigger : MonoBehaviour
{
    public GameObject element;
    // Start is called before the first frame update
    void Start()
    {
        element.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            element.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            element.SetActive(false);
        }

    }
}
