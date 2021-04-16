using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationOnSpawn : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        float randomRotation = Random.Range(0f, 360f);
        float randomScale = Random.Range(.85f, 1.15f);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, randomRotation));
        transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
