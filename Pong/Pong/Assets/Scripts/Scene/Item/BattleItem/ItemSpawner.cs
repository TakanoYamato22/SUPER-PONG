using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] itemPrefabs;

    [Header("Spawn Range")]
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 30f;
    [SerializeField] private int spawnCount = 2;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnItems();
        }
    }

    private void SpawnItems()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0)
            return;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            Vector2 spawnPos = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            Instantiate(itemPrefab, spawnPos, Quaternion.identity);
        }
    }
}