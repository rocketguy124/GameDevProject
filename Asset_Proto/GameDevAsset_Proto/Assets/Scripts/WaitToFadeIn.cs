using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToFadeIn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b, 1f);
        StartCoroutine(WaitToFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator WaitToFade()
    {
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b, Mathf.MoveTowards(gameObject.GetComponent<SpriteRenderer>().color.a, 0f, 10));

    }
}
