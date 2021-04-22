using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAudio : MonoBehaviour
{
    public AudioSource audioSource;
    private GameObject Player;
    private Vector2 moveInput;

    private bool IsMoving;

    void Start()
    {
        //audioSource = gameObject.GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); 

        if (moveInput != Vector2.zero) IsMoving = true; 
        else IsMoving = false;

        if (IsMoving && !audioSource.isPlaying) audioSource.Play(); // if player is moving and audiosource is not playing play it
        if (!IsMoving) audioSource.Stop(); // if player is not moving and audiosource is playing stop it
    }
}
