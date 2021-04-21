using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimationOnEnd : MonoBehaviour
{

    public void DestroyParent()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        Destroy(parent);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
