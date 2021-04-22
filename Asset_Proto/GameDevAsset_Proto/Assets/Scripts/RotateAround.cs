using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Transform rotatePointObject;
    Vector3 point = new Vector3(5, 0, 0);
    Vector3 axis = new Vector3(0, 0, 1);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(rotatePointObject.position, axis, Time.deltaTime * 25);
    }
}
