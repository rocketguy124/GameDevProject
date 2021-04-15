using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherTimeStopping : MonoBehaviour
{
    public TimeManager timemanager;
    private Rigidbody2D theRB;
    private Animator otherAnim;

    private float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    private bool IsStopped;
    public TimeBody otherTimeBody;
    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        otherAnim = GetComponent<Animator>();
        timemanager = FindObjectOfType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeBeforeAffectedTimer -= Time.deltaTime; // minus 1 per second
        theRB.velocity = Vector3.zero; //makes the rigidbody stop moving
        theRB.isKinematic = true; //not affected by forces

        if (TimeBeforeAffectedTimer <= 0f)
        {
            CanBeAffected = true; // Will be affected by timestop
        }
        otherAnim.enabled = true;

        if (CanBeAffected && timemanager.TimeIsStopped && !IsStopped)
        {
            if (theRB.velocity.magnitude >= 0f) //If Object is moving
            {
                

                theRB.velocity = Vector3.zero; //makes the rigidbody stop moving
                theRB.isKinematic = true; //not affected by forces
                otherTimeBody.IsStopped = true; // prevents this from looping
                otherAnim.enabled = false;
            }
        }
    }
}
