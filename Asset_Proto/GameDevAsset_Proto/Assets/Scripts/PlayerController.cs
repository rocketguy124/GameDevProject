using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Movement")]
    public float moveSpeed;
    private Vector2 moveInput;
    public Rigidbody2D theRB;

    [Header("Aiming")]
    public Transform staffArm;
    private Camera theCam;

    [Header("Animation")]
    public Animator bodyAnim;
    public Animator wholePlayerAnim;

    [Header("Projectiles")]
    public GameObject projectileToFire;
    public Transform firePoint;
    public float timeBetweenShots;
    private float shotCounter;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        theRB = gameObject.GetComponent<Rigidbody2D>();
        theCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        theRB.velocity = moveInput * moveSpeed;

        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenPoint = theCam.WorldToScreenPoint(transform.localPosition);

        //Flipping player and staff based on aim
        if(mousePosition.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            staffArm.localScale = new Vector3(-.7f, .7f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            staffArm.localScale = new Vector3(.7f, .7f, 1f);
        }

        //rotation for staff to aim at mouse
        Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        staffArm.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle-90));


        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(projectileToFire, firePoint.transform.position, firePoint.transform.rotation);
            shotCounter = timeBetweenShots;
        }
        if (Input.GetMouseButton(0))
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                Instantiate(projectileToFire, firePoint.transform.position, firePoint.transform.rotation);
                shotCounter = timeBetweenShots;
            }
        }







        //Animation Code
        if (moveInput != Vector2.zero)
        {
            bodyAnim.SetBool("isMoving", true);
        }
        else
        {
            bodyAnim.SetBool("isMoving", false);
        }
        
    }
}
