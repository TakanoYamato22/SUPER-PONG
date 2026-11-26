
using UnityEngine;

public class WhirlpoolSpawner : MonoBehaviour
{
    [Header("渦潮Prefab")]
    [SerializeField] private GameObject whirlpoolPrefab;

    [Header("出現間隔")]
    [SerializeField, Min(0.1f)]
    private float spawnInterval = 12f;

    [Header("出現範囲")]
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;

    [Header("最大出現数")]
    [SerializeField, Min(1)]
    private int maxWhirlpools = 1;

    private float spawnTimer;
    private bool spawningEnabled;

    public void StartSpawning()
    {
        if (spawningEnabled)
            return;

        spawnTimer = 0f;
        spawningEnabled = true;

        Debug.Log("渦潮ギミック開始！");
    }

    public void StopSpawning()
    {
        spawningEnabled = false;
        spawnTimer = 0f;
    }

    private void Update()
    {
        if (!spawningEnabled || whirlpoolPrefab == null)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer < spawnInterval)
            return;

        spawnTimer = 0f;

        SpawnWhirlpool();
    }

    private void SpawnWhirlpool()
    {
        int currentCount =
            FindObjectsByType<Whirlpool>(
                FindObjectsSortMode.None
            ).Length;

        if (currentCount >= maxWhirlpools)
            return;

        Vector3 spawnPosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0f
        );

        Instantiate(
            whirlpoolPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Debug.Log("渦潮出現！");
    }
}
