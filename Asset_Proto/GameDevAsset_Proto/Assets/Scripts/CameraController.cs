using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float transitionSpeed;

    public Transform target;

    public Camera mainCam, bigMapCam;

    private bool bigMapActive;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, -10f), transitionSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!bigMapActive)
            {
                ActivateBigMap();
            }
            else
            {
                DeactivateBigMap();
            }
        }
    }
    public void ChangeCameraTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public void ActivateBigMap()
    {
        if (!LevelManager.instance.isPaused)
        {
            bigMapActive = true;

            bigMapCam.enabled = true;
            mainCam.enabled = false;

            PlayerController.instance.canMove = false;
            Time.timeScale = 0f;

            UIController.instance.mapDisplay.SetActive(false);
            UIController.instance.fullmapText.SetActive(true);
        }
    }
    
    public void DeactivateBigMap()
    {
        if (!LevelManager.instance.isPaused)
        {
            bigMapActive = false;

            bigMapCam.enabled = false;
            mainCam.enabled = true;

            PlayerController.instance.canMove = true;
            Time.timeScale = 1f;
            UIController.instance.mapDisplay.SetActive(true);
            UIController.instance.fullmapText.SetActive(false);

        }
    }
}
