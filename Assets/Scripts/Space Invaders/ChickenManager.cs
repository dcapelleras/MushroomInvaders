using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenManager : MonoBehaviour
{
    private GameManager gameManager;

    // Variables para la matriz de pollos
    public GameObject[] chickenPrefabs;

    // Variables para el movimiento de los pollos
    public float chickenSpeed = 2.0f;
    [SerializeField] private Vector3 moveDirection = Vector2.right;
    [SerializeField] private float padding = 2.0f;
    public Transform rightEdge;
    public Transform leftEdge;

    // Variables para el ataque de los pollos
    public GameObject chickenBulletPrefab;
    public List<GameObject> chickenBulletList;
    public Transform chickenBulletListParent;
    public GameObject chickenMissilePrefab;
    public List<GameObject> chickenMissileList;
    public Transform chickenMissileListParent;
    [SerializeField] private float chickenAttackRate;
    [SerializeField] private float minShootingProbability = 0.3f;
    [SerializeField] private float chickenMissileProbability = 0.3f;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        int rows = gameManager.chickenRows;
        int columns = gameManager.chickenColumns;

        // Se calculan variables para saber dónde posicionar a los pollos chef
        // y a los pollos termómetro
        float a, b;
        a = Mathf.Round((float)columns / 3.0f);
        b = a;
        if (columns % 3 == 1)
        {
            b = a + 1;
        }
        else if (columns % 3 == 2)
        {
            b = a - 1;
        }

        // Se instancia una matriz de pollos
        for (int row = 0; row < rows; row++)
        {
            // Se calcula la posición en la fila en la que se va a instanciar
            float width = 2.0f * (columns - 1);
            float height = 2.0f * (rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);

            for (int col = 0; col < columns; col++)
            {
                int prefabIndex = 0;

                if (row == (rows - 1) && ((col >= a) && (col < (a + b))))
                {
                    prefabIndex = 1;
                }
                else if (row >= (rows - 2))
                {
                    prefabIndex = 2;
                }

                GameObject chicken = Instantiate(chickenPrefabs[prefabIndex], transform);
                Vector3 position = rowPosition;
                position.x += col * 2.0f;
                chicken.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        StartCoroutine("ChickenAttack");
    }

    private void Update()
    {
        if (!gameManager.gameOver)
        {
            // Movemos el bloque de pollos en la dirección actual
            transform.position += moveDirection * chickenSpeed * Time.deltaTime;

            // Obtenemos los bordes de la pantalla (modificable por otros bordes que queramos poner nosotros)
            Vector3 leftEdgePos = leftEdge.position;
            Vector3 rightEdgePos = rightEdge.position;

            foreach (Transform chicken in transform)
            {
                // Si el pollo está desactivado, continúa al siguiente
                if (!chicken.gameObject.activeInHierarchy)
                {
                    continue;
                }

                // Si el bloque de pollos choca con los bordes de la pantalla, los pollos cambian de dirección
                if (moveDirection == Vector3.right && chicken.position.x >= (rightEdgePos.x - padding))
                {
                    AdvanceRow();
                }
                else if (moveDirection == Vector3.left && chicken.position.x <= (leftEdgePos.x + padding))
                {
                    AdvanceRow();
                }
            }
        } else
        {
            StopCoroutine("ChickenAttack");
        }
    }

    private void AdvanceRow()
    {
        moveDirection.x *= -1.0f;

        Vector3 position = transform.position;
        position.y -= 1.0f;
        transform.position = position;
    }

    private IEnumerator ChickenAttack()
    {
        if (gameManager.gameOver)
        {
            yield break;
        }

        while (!gameManager.gameOver)
        {
            yield return new WaitForSeconds(chickenAttackRate);

            foreach (Transform chicken in transform)
            {
                // Si el pollo está desactivado, continúa al siguiente
                if (!chicken.gameObject.activeInHierarchy)
                {
                    continue;
                }

                // Dada una probabilidad de disparo
                if (Random.value < ((float)(gameManager.GetScore() / transform.childCount) + minShootingProbability))
                {
                    bool projectileFound = false;
                    string projectile = "bullet";
                    List<GameObject> projectileList = chickenBulletList;

                    // Dada una probabilidad, el proyectil será una bala o un misil
                    if (Random.value < chickenMissileProbability)
                    {
                        projectileList = chickenMissileList;
                        projectile = "missile";
                    }

                    // Busca la primera bala de la lista que no esté activada
                    foreach (GameObject chickenProjectile in projectileList)
                    {
                        if (!chickenProjectile.activeInHierarchy)
                        {
                            chickenProjectile.transform.position = chicken.position;

                            chickenProjectile.SetActive(true);
                            projectileFound = true;
                            break;
                        }
                    }

                    // Si no hay balas desactivadas, añade una nueva
                    if (!projectileFound)
                    {
                        if (projectile == "bullet")
                        {
                            GameObject chickenBullet = Instantiate(chickenBulletPrefab, chicken.position, Quaternion.identity);
                            chickenBullet.SetActive(false);
                            chickenBulletList.Add(chickenBullet);
                            chickenBullet.transform.parent = chickenBulletListParent;
                        } else if (projectile == "missile")
                        {
                            GameObject chickenMissile = Instantiate(chickenMissilePrefab, chicken.position, Quaternion.identity);
                            chickenMissile.SetActive(false);
                            chickenMissileList.Add(chickenMissile);
                            chickenMissile.transform.parent = chickenMissileListParent;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (chickenAttackRate >= .5f)
            {
                chickenAttackRate -= chickenAttackRate * ((float)gameManager.GetScore() / (transform.childCount * 10));
            }
        }
    }
}
