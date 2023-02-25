using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private GameManager gameManager;

    // Variables para el movimiento del jugador
    [SerializeField] private float speed = 10f;
    private float maxRangeX = 26.0f;
    private float maxRangeY = 12.0f;

    // Variables para el disparo del jugador
    public bool canShoot = true;
    public float shootCD = .5f;

    // Variables para las balas del jugador
    public List<GameObject> playerBulletList;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public Transform playerBulletListParent;

    public Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        ConstrainPlayerMovement();

        if (!gameManager.gameOver)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            transform.Translate(new Vector3(horizontal, vertical, 0f).normalized * speed * Time.deltaTime);

            if (canShoot && Input.GetMouseButton(0))
            {
                canShoot = false;
                Shoot();
                StartCoroutine("ShootCD");
            }
        } else
        {
            canShoot = false;
            StopCoroutine("ShootCD");
        }
            
    }

    private void ConstrainPlayerMovement()
    {
        if (transform.position.x > maxRangeX)
        {
            transform.position = new Vector3(maxRangeX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -maxRangeX)
        {
            transform.position = new Vector3(-maxRangeX, transform.position.y, transform.position.z);
        }

        if (transform.position.y > maxRangeY)
        {
            transform.position = new Vector3(transform.position.x, maxRangeY, transform.position.z);
        }
        else if (transform.position.y < -maxRangeY)
        {
            transform.position = new Vector3(transform.position.x, -maxRangeY, transform.position.z);
        }
    }

    void Shoot()
    {
        bool bulletFound = false;
        //anim.Play("ketchupAnim 0");
        // Busca la primera bala de la lista que no esté activada
        foreach(GameObject playerBullet in playerBulletList)
        {
            if (!playerBullet.activeInHierarchy)
            {
                playerBullet.transform.position = shootPoint.position;
                playerBullet.SetActive(true);
                bulletFound = true;
                break;
            }
        }

        // Si no hay balas desactivadas, añade una nueva
        if (!bulletFound)
        {
            GameObject playerBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            playerBullet.SetActive(false);
            playerBulletList.Add(playerBullet);
            playerBullet.transform.parent = playerBulletListParent;
        }
    }

    IEnumerator ShootCD()
    {
        if (gameManager.gameOver)
        {
            yield break;
        }

        yield return new WaitForSeconds(shootCD);
        canShoot = true;
    }
}
