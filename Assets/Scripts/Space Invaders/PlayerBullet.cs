using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private float speed = 10f;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (!gameManager.gameOver)
            transform.Translate(transform.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameManager.AddScore(1);
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("OutOfScene"))
        {
            gameObject.SetActive(false);
        }
    }
}

