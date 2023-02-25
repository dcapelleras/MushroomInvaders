using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMissile : MonoBehaviour
{
    private GameManager gameManager;

    // Variables para el movimiento del misil
    private Transform playerTransform;
    [SerializeField] private float steeringSpeed = 0.6f;
    [SerializeField] private float speed = 13f;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Vector3 oldDirection;
    [SerializeField] private float playerMinDistance = 0.5f;

    // Variables de control de tiempo
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
        direction = (playerTransform.position - transform.position);
        oldDirection = direction;
    }

    private void Update()
{
        Vector3 newDirection = (playerTransform.position - transform.position);
        direction = Vector3.Slerp(oldDirection, newDirection, steeringSpeed);

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

