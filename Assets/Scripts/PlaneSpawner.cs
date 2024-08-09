using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaneSpawner : MonoBehaviour {
    private float edgeOffset;
    [SerializeField] private float spawnInterval;
    [SerializeField] private List<GameObject> planePrefabs;

    void Start() {
        edgeOffset = 0.2f;
        InvokeRepeating("SpawnPlane", 0f, spawnInterval);
    }

    void SpawnPlane()
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

        // Instanciando o prefab
        System.Random random = new System.Random();
        int index = random.Next(planePrefabs.Count);
        GameObject plane = Instantiate(planePrefabs[index], spawnPosition, Quaternion.identity);

        // Movendo o avião e ajustando a rotação
        plane.GetComponent<Airplane>().SetDirection(direction);
    }
}
