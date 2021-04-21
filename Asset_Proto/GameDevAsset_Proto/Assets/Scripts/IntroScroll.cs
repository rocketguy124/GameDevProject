using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScroll : MonoBehaviour
{
    public float scrollSpeed;
    public float scrollTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scrollTime -= Time.deltaTime;
        if (scrollTime > 0)
        {
            gameObject.transform.position += new Vector3(0f, scrollSpeed * Time.deltaTime);
            
        }
    }
}
