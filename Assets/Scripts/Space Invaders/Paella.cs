using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paella : MonoBehaviour
{
    [SerializeField] private int health = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            health--;
            other.gameObject.SetActive(false);

            if (health <= 0)
            {
                gameObject.SetActive(false);
            }
        } else if (other.gameObject.CompareTag("PlayerBullet"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
