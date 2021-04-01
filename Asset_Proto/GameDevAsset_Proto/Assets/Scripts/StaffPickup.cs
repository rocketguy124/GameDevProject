using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffPickup : MonoBehaviour
{
    [Header("Staff")]
    public Staff theStaff;

    [Header("Time till Collection")]
    public float timeTillCollectable = 0.5f;

    [Header("Sound")]
    public int PickupSound;

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
            bool hasStaff = false;
            foreach(Staff staffToCheck in PlayerController.instance.availableStaffs) // Detects if Player already has the weapon
            {
                if(theStaff.weaponName == staffToCheck.weaponName)
                {
                    hasStaff = true;
                }
            }
            if (!hasStaff) // If the player doesn't have the weapon
            {
                Staff staffClone = Instantiate(theStaff);
                staffClone.transform.parent = PlayerController.instance.staffArm; // make the clone a child of the rotate point
                staffClone.transform.position = PlayerController.instance.staffArm.position;
                staffClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                staffClone.transform.localScale = new Vector3(1f, 1f, 1f); //Weird scaling bug

                PlayerController.instance.availableStaffs.Add(staffClone);
                PlayerController.instance.currentStaff = PlayerController.instance.availableStaffs.Count - 1;
                PlayerController.instance.SwitchStaff();

            }
            AudioManager.instance.PlaySFX(PickupSound);
            Destroy(gameObject);
        }
    }
}
