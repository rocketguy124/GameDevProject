using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource levelMusic, gameOverMusic, WinMusic;

    public AudioSource[] SFX;

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
        
    }

    public void PlayGameoverMusic()
    {
        levelMusic.Stop();
        gameOverMusic.Play();
    }

    public void PlayWinMusic()
    {
        levelMusic.Stop();
        WinMusic.Play();
    }

    public void PlaySFX(int soundToPlay)
    {
        SFX[soundToPlay].Stop();
        SFX[soundToPlay].Play();
    }
}
