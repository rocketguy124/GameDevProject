using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered;

    public GameObject[] doors;

    //public List<GameObject> enemies = new List<GameObject>();
    [HideInInspector]
    public bool roomActive;

    public GameObject mapHider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);

            closeWhenEntered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            CameraController.instance.ChangeCameraTarget(transform);
            PlayerController.instance.dodgeCounter = PlayerController.instance.dodgeLength;
            PlayerHealthController.instance.MakeInvincible(1f);


            if (closeWhenEntered)
            {
                foreach(GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            roomActive = true;
            


            mapHider.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            roomActive = false;
            OpenDoors();
        }
    }

    public void ActivateEnemies(List<GameObject> enemies)
    {
        StartCoroutine(ActivateOnDelay(enemies));
    }

    public IEnumerator ActivateOnDelay(List<GameObject> enemies)
    {
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }
}
