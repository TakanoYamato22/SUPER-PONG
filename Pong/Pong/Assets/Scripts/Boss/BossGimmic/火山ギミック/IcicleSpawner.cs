using UnityEngine;

public class IcicleSpawner : MonoBehaviour
{
    [Header("氷柱設定")]
    [SerializeField] private GameObject iciclePrefab;
    [SerializeField] private float spawnInterval = 3f;

    [Header("生成位置")]
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private float spawnY = 5f;

    private float spawnTimer;
    private bool isSpawning;

    private void Update()
    {
        if (!isSpawning) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnIcicle();
            spawnTimer = spawnInterval;
        }
    }

    public void StartSpawning()
    {
        if (isSpawning) return;

        isSpawning = true;
        spawnTimer = spawnInterval;

        Debug.Log("氷柱ギミック開始！");
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }


    private void SpawnIcicle()
    {
        Debug.Log("氷柱の生成処理に到達！");

        if (iciclePrefab == null)
        {
            Debug.LogError("Icicle Prefabが設定されていません！");
            return;
        }

        float randomX = Random.Range(minX, maxX);

        Vector3 spawnPosition = new Vector3(
            randomX,
            spawnY,
            0f
        );

        GameObject icicle = Instantiate(
            iciclePrefab,
            spawnPosition,
            Quaternion.identity
        );

        Debug.Log($"氷柱生成成功！位置：{icicle.transform.position}");
    }

}