using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaneSpawner : MonoBehaviour
{
    private int planesOnScreen;
    private float edgeOffset;
    private float currentSpawnInterval;
    [SerializeField] private List<GameObject> planePrefabs;
    [SerializeField] private int maxPlanesOnScreen;
    [SerializeField] private float spawnInterval, decreaseInterval, minInterval, ducksChance;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject ducksPrefab;


    void Start()
    {
        planesOnScreen = 0;
        edgeOffset = 0.2f;
        currentSpawnInterval = spawnInterval;
        GameController.Instance.Restart();
        Invoke("SpawnPlane", currentSpawnInterval);
    }

    private void UpdateInterval()
    {
        Debug.Log("Current Interval: " + currentSpawnInterval + " s");
        currentSpawnInterval -= decreaseInterval;
        if (planesOnScreen >= maxPlanesOnScreen)
        {
            currentSpawnInterval = spawnInterval/2f;
        }
        if (currentSpawnInterval < minInterval)
        {
            currentSpawnInterval = minInterval;
        }
    }

    private void SpawnPlane()
    {
        // Definindo as bordas da tela
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculando os limites para evitar os cantos
        float xMin = screenWidth * edgeOffset;
        float xMax = screenWidth * (1 - edgeOffset);
        float yMin = screenHeight * edgeOffset;
        float yMax = screenHeight * (1 - edgeOffset);

        // Escolhendo uma posição aleatória na borda
        Vector3 spawnPosition = Vector3.zero;
        Vector2 direction = Vector2.zero;

        int edge = Random.Range(0, 4); // 0 = topo, 1 = direita, 2 = baixo, 3 = esquerda

        switch (edge)
        {
            case 0: // Topo
                spawnPosition = new Vector3(Random.Range(xMin, xMax), screenHeight, 0);
                direction = new Vector2(Random.Range(-1f, 1f), -1).normalized;
                break;
            case 1: // Direita
                spawnPosition = new Vector3(screenWidth, Random.Range(yMin, yMax), 0);
                direction = new Vector2(-1, Random.Range(-1f, 1f)).normalized;
                break;
            case 2: // Baixo
                spawnPosition = new Vector3(Random.Range(xMin, xMax), 0, 0);
                direction = new Vector2(Random.Range(-1f, 1f), 1).normalized;
                break;
            case 3: // Esquerda
                spawnPosition = new Vector3(0, Random.Range(yMin, yMax), 0);
                direction = new Vector2(1, Random.Range(-1f, 1f)).normalized;
                break;
        }

        // Convertendo a posição da tela para o espaço do mundo
        spawnPosition = Camera.main.ScreenToWorldPoint(spawnPosition);
        spawnPosition.z = 0; // Definindo a posição Z para 0

        // Start blinking warning before spawning the plane
        StartCoroutine(BlinkWarning(spawnPosition, direction, edge));
    }

    private IEnumerator BlinkWarning(Vector3 position, Vector2 direction, int edge)
    {
        Vector3 adjustedPosition = position + new Vector3(direction.x * 0.5f, direction.y * 0.5f, 0);
        GameObject warning = Instantiate(warningPrefab, adjustedPosition, Quaternion.identity);
        SpriteRenderer warningRenderer = warning.GetComponent<SpriteRenderer>();

        for (int i = 0; i < 3; i++)
        {
            warningRenderer.enabled = true;
            yield return new WaitForSeconds(0.5f);
            warningRenderer.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }

        Destroy(warning);

        // Randomly decide whether to spawn a plane or a duck
        if ((edge == 1 || edge == 3) && Random.Range(0f, 1f) < ducksChance)
        {
            this.SpawnDucks(position, direction);
        }
        else
        {
            this.SpawnAirplane(position, direction);
        }
    }

    private void SpawnAirplane(Vector3 position, Vector2 direction)
    {
        System.Random random = new System.Random();
        int index = random.Next(planePrefabs.Count);
        GameObject plane = Instantiate(planePrefabs[index], position, Quaternion.identity);

        // Movendo o avião e ajustando a rotação
        plane.GetComponent<Airplane>().SetDirection(direction);

        Invoke("SpawnPlane", currentSpawnInterval);
        planesOnScreen++;
        this.UpdateInterval();
    }

    private void SpawnDucks(Vector3 position, Vector2 direction)
    {
        // Adjust position for duck spawn
        Vector3 adjustedPosition = position + new Vector3(direction.x * 0.5f, direction.y * 0.5f, 0);
        GameObject duck = Instantiate(ducksPrefab, adjustedPosition, Quaternion.identity);
        Invoke("SpawnPlane", currentSpawnInterval);
    }

    public void RemovePlane()
    {
        planesOnScreen--;
    }
}
