using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenBullet : MonoBehaviour
{
    private GameManager gameManager;

    // Variables para el movimiento de la bala
    private Transform playerTransform;
    [SerializeField] private float speed = 13.0f;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float playerMinDistance = 0.2f;

    // variable de control de tiempo
    [SerializeField] private float timeLived;
    [SerializeField] private float timeToDie = 2f;


    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void OnEnable()
    {
        timeLived = 0;
        direction = playerTransform.position - transform.position;
    }

    private void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        timeLived += Time.deltaTime;
        if (timeLived > timeToDie || gameManager.gameOver)
        {
            gameObject.SetActive(false);
        }

        if (Vector3.Distance(transform.position, playerTransform.position) <= playerMinDistance)
        {
            gameObject.SetActive(false);
            gameManager.RemoveHealth(1);
        }
    }
}
