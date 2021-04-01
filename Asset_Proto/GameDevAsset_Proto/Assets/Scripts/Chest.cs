using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public StaffPickup[] potentialStaffs;

    public SpriteRenderer theSR;
    public Sprite chestOpen;
    public Animator theAnim;

    public GameObject openNotification;

    private bool canOpen, isOpen;
    public Transform spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        theSR = gameObject.GetComponent<SpriteRenderer>();
        theAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && !isOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                int staffSelect = Random.Range(0, potentialStaffs.Length);
                Instantiate(potentialStaffs[staffSelect], spawnLocation.position, spawnLocation.rotation);

                theSR.sprite = chestOpen;

                isOpen = true;

                theAnim.SetBool("isOpen", true);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !isOpen)
        {
            openNotification.SetActive(true);

            canOpen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            openNotification.SetActive(false);

            canOpen = false;
        }
    }
}
