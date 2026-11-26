
using System.Collections;
using UnityEngine;

public class BlackHoleSpawner : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private SpaceBossController boss;
    [SerializeField] private GameObject blackHolePrefab;

    [Header("出現条件")]
    [SerializeField, Range(0f, 1f)]
    private float startHpRatio = 0.7f;

    [Header("出現範囲")]
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;

    [Header("出現間隔")]
    [SerializeField] private float spawnInterval = 15f;

    private bool started;
    private bool stopped;

    private GameObject currentBlackHole;
    private Coroutine spawnCoroutine;

    // ========================================
    // 毎フレーム処理
    // ========================================

    private void Update()
    {
        if (boss == null)
            return;

        // 第2形態への移行が始まったら停止
        if (boss.IsPhaseChanging || boss.IsSecondPhase)
        {
            StopBlackHoleGimmick();
            return;
        }

        if (boss.IsDead)
        {
            StopBlackHoleGimmick();
            return;
        }

        if (started || stopped)
            return;

        if (!boss.BattleStarted || boss.MaxHP <= 0f)
            return;

        // 第1形態のHPが70%以下になったら開始
        if (boss.CurrentHP / boss.MaxHP <= startHpRatio)
        {
            started = true;

            spawnCoroutine = StartCoroutine(
                SpawnRoutine()
            );

            Debug.Log("ブラックホールギミック開始！");
        }
    }

    // ========================================
    // ブラックホール生成
    // ========================================

    private IEnumerator SpawnRoutine()
    {
        while (boss != null &&
               !boss.IsDead &&
               !boss.IsPhaseChanging &&
               !boss.IsSecondPhase &&
               !stopped)
        {
            if (currentBlackHole == null &&
                blackHolePrefab != null)
            {
                Vector2 spawnPosition = new Vector2(
                    Random.Range(minX, maxX),
                    Random.Range(minY, maxY)
                );

                currentBlackHole = Instantiate(
                    blackHolePrefab,
                    spawnPosition,
                    Quaternion.identity
                );

                Debug.Log("ブラックホール出現！");
            }

            yield return new WaitForSeconds(
                spawnInterval
            );
        }

        spawnCoroutine = null;
    }

    // ========================================
    // ギミック停止
    // ========================================

    private void StopBlackHoleGimmick()
    {
        if (stopped)
            return;

        stopped = true;

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // 残っているブラックホールを削除
        if (currentBlackHole != null)
        {
            Destroy(currentBlackHole);
            currentBlackHole = null;
        }

        Debug.Log("ブラックホールギミック終了！");
    }

    // ========================================
    // オブジェクト無効化時
    // ========================================

    private void OnDisable()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}
