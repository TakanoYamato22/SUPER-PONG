
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    [Header("クリーチャー")]
    [SerializeField] private GameObject creaturePrefab;

    [Header("出現間隔")]
    [SerializeField, Min(0.1f)]
    private float spawnInterval = 10f;

    [Header("出現範囲")]
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;

    [Header("最大出現数")]
    [SerializeField, Min(1)]
    private int maxCreatures = 2;

    private float spawnTimer;
    private bool spawningEnabled;

    public void StartSpawning()
    {
        if (spawningEnabled)
            return;

        spawnTimer = 0f;
        spawningEnabled = true;

        Debug.Log("クリーチャー出現開始！");
    }

    public void StopSpawning()
    {
        spawningEnabled = false;
        spawnTimer = 0f;
    }

    private void Update()
    {
        if (!spawningEnabled || creaturePrefab == null)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer < spawnInterval)
            return;

        spawnTimer = 0f;

        SpawnCreature();
    }

    private void SpawnCreature()
    {
        int currentCount =
            FindObjectsByType<Creature>(
                FindObjectsSortMode.None
            ).Length;

        if (currentCount >= maxCreatures)
            return;

        Vector3 spawnPosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0f
        );

        Instantiate(
            creaturePrefab,
            spawnPosition,
            Quaternion.identity
        );

        Debug.Log("クリーチャー出現！");
    }
}
