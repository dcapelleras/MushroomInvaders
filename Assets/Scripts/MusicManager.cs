using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private GameManager gameManager;
    private AudioSource cameraAudio;

    // Temas de música
    public AudioClip mainTheme;
    public AudioClip endGameTheme;

    private bool musicToggle = false;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        cameraAudio = gameObject.GetComponent<AudioSource>();
        cameraAudio.Stop();
        cameraAudio.clip = mainTheme;
        cameraAudio.Play();
    }

    private void Update()
    {
        if (!musicToggle && gameManager.gameOver)
        {
            musicToggle = true;
            cameraAudio.Stop();
            cameraAudio.clip = endGameTheme;
            cameraAudio.Play();
        }
    }
}
