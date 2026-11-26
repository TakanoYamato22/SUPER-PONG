
using UnityEngine;

public class OxygenSpawner : MonoBehaviour
{
    [Header("酸素アイテム")]
    [SerializeField] private GameObject oxygenPrefab;

    [Header("出現間隔")]
    [SerializeField, Min(0.1f)]
    private float spawnInterval = 10f;

    [Header("1回の出現数")]
    [SerializeField, Min(1)]
    private int spawnCount = 2;

    [Header("出現範囲")]
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;

    [Header("最大出現数")]
    [SerializeField, Min(2)]
    private int maxItems = 6;

    private float spawnTimer;
    private bool spawningEnabled;

    // ========================================
    // 酸素アイテム出現開始
    // ========================================

    public void StartSpawning()
    {
        spawnTimer = 0f;
        spawningEnabled = true;

        Debug.Log("酸素アイテム出現開始！");
    }

    // ========================================
    // 酸素アイテム出現停止
    // ========================================

    public void StopSpawning()
    {
        spawningEnabled = false;
    }

    private void Update()
    {
        if (!spawningEnabled || oxygenPrefab == null)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer < spawnInterval)
            return;

        spawnTimer = 0f;

        SpawnOxygenItems();
    }

    // ========================================
    // 酸素アイテムを2個同時に生成
    // ========================================

    private void SpawnOxygenItems()
    {
        // 現在存在する酸素アイテムの数
        int currentCount =
            FindObjectsByType<OxygenItem>(
                FindObjectsSortMode.None
            ).Length;

        // 2個まとめて出せる空きがなければ生成しない
        if (currentCount + spawnCount > maxItems)
        {
            Debug.Log(
                "酸素アイテムの最大数に近いため、今回は生成しません。"
            );
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                0f
            );

            Instantiate(
                oxygenPrefab,
                spawnPosition,
                Quaternion.identity
            );
        }

        Debug.Log(
            $"酸素アイテムを{spawnCount}個生成！"
        );
    }
}
