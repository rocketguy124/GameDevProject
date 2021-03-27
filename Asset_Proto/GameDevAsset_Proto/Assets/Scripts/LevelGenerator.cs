using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;

    public Color startColor, endColor;
    //public GameObject startRoom;
   // public GameObject endRoom;

    public int distanceToExit;

    public Transform generationPoint;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom, generationPoint.position, generationPoint.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
