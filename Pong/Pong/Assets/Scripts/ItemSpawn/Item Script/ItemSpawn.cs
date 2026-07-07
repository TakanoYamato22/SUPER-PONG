using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;

    public float spawnInterval = 10f;

    private void Start()
    {
        InvokeRepeating(
            nameof(SpawnItem),
            3f,
            spawnInterval
        );
    }

    void SpawnItem()
    {
        Vector2 pos = new Vector2(
            Random.Range(-6f, 6f),
            Random.Range(-3f, 3f)
        );

        Instantiate(itemPrefab, pos, Quaternion.identity);
    }
}