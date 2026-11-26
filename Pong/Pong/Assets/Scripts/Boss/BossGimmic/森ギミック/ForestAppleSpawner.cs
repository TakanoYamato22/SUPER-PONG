
using UnityEngine;

public class ForestAppleSpawner : MonoBehaviour
{
    [Header("古代樹")]
    [SerializeField]
    private ForestBossController forestBoss;

    [Header("リンゴPrefab")]
    [SerializeField]
    private GameObject applePrefab;

    [Header("出現範囲")]
    [SerializeField]
    private Vector2 minSpawnPosition = new Vector2(-5f, -3f);

    [SerializeField]
    private Vector2 maxSpawnPosition = new Vector2(5f, 3f);

    [Header("出現条件")]
    [SerializeField, Range(0f, 1f)]
    private float spawnHpRatio = 0.5f;

    private bool hasSpawned;

    private void Update()
    {
        if (hasSpawned ||
            forestBoss == null ||
            applePrefab == null)
        {
            return;
        }

        if (!forestBoss.BattleStarted ||
            forestBoss.IsDead ||
            forestBoss.IsPhaseChanging ||
            forestBoss.MaxHP <= 0f)
        {
            return;
        }

        float hpRatio =
            forestBoss.CurrentHP / forestBoss.MaxHP;

        if (hpRatio <= spawnHpRatio)
        {
            SpawnApple();
        }
    }

    private void SpawnApple()
    {
        // 1回だけ出現
        hasSpawned = true;

        float randomX = Random.Range(
            minSpawnPosition.x,
            maxSpawnPosition.x
        );

        float randomY = Random.Range(
            minSpawnPosition.y,
            maxSpawnPosition.y
        );

        Vector3 spawnPosition = new Vector3(
            randomX,
            randomY,
            0f
        );

        Instantiate(
            applePrefab,
            spawnPosition,
            Quaternion.identity
        );

        Debug.Log("古代樹のHPが50%以下！リンゴ出現！");
    }
}
