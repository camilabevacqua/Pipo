using UnityEngine;
using System.Collections;

public class PipoCatchItemSpawner : MonoBehaviour
{
    [Header("Velocidad de Juego")]
    [SerializeField] private float initialSpawnRate = 2.0f;
    [SerializeField] private float minSpawnRate = 0.5f;
    [SerializeField] private float difficultyDelta = 0.05f;

    private float currentSpawnRate;
    private float xLimit;

    void Start()
    {
        currentSpawnRate = initialSpawnRate;

        CalcularLimitesDePantalla();

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnItem();

            yield return new WaitForSeconds(currentSpawnRate);

            if (currentSpawnRate > minSpawnRate)
            {
                currentSpawnRate -= difficultyDelta;
            }
        }
    }

    void SpawnItem()
    {
        float randomX = Random.Range(-xLimit, xLimit);

        Vector2 spawnPos =
            new Vector2(randomX, transform.position.y);

        GameObject item =
            CatchPool.instance.ObtenerObjeto();

        item.transform.position = spawnPos;
        item.transform.rotation = Quaternion.identity;

        item.SetActive(true);
    }

    void CalcularLimitesDePantalla()
    {
        float halfWidth =
            Camera.main.aspect * Camera.main.orthographicSize;

        xLimit = halfWidth - 0.5f;
    }
}