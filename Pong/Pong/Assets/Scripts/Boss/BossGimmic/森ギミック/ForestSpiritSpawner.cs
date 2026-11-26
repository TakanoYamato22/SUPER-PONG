
using UnityEngine;
using System.Collections;

public class ForestSpiritSpawner : MonoBehaviour
{
    [Header("古代樹")]
    [SerializeField]
    private ForestBossController forestBoss;

    [Header("精霊")]
    [SerializeField]
    private ForestSpirit windSpirit;

    [SerializeField]
    private ForestSpirit waterSpirit;

    [SerializeField]
    private ForestSpirit fireSpirit;

    [SerializeField]
    private ForestSpirit lightSpirit;

    [Header("水の精霊の出現条件")]
    [Range(0f, 1f)]
    [SerializeField]
    private float waterSpawnHpRatio = 0.5f;

    [Header("火の精霊の出現条件")]
    [Min(0f)]
    [SerializeField]
    private float fireSpawnDelay = 20f;

    [Header("光の精霊の出現条件")]
    [Min(0f)]
    [SerializeField]
    private float lightSpawnDelay = 20f;

    private bool spawningStarted;
    private bool waterSpawned;
    private bool fireSpawned;
    private bool lightSpawned;

    private void Start()
    {
        SetSpiritActive(windSpirit, false);
        SetSpiritActive(waterSpirit, false);
        SetSpiritActive(fireSpirit, false);
        SetSpiritActive(lightSpirit, false);
    }

    private void Update()
    {
        if (forestBoss == null)
        {
            return;
        }

        // 戦闘開始待ち
        if (!forestBoss.BattleStarted)
        {
            return;
        }

        // ボス撃破後は処理しない
        if (forestBoss.IsDead)
        {
            return;
        }

        // フェーズ切り替え中は処理しない
        if (forestBoss.IsPhaseChanging)
        {
            return;
        }

        // 戦闘開始時に風の精霊を出現
        if (!spawningStarted)
        {
            StartSpawning();
        }

        // 水の精霊の出現判定
        CheckWaterSpawn();
    }

    private void StartSpawning()
    {
        spawningStarted = true;

        SetSpiritActive(windSpirit, true);

        Debug.Log("風の精霊 出現！");
    }

    private void CheckWaterSpawn()
    {
        if (waterSpawned || windSpirit == null)
        {
            return;
        }

        if (windSpirit.MaxHP <= 0f)
        {
            return;
        }

        float hpRatio =
            windSpirit.CurrentHP / windSpirit.MaxHP;

        if (hpRatio <= waterSpawnHpRatio)
        {
            waterSpawned = true;

            SetSpiritActive(waterSpirit, true);

            Debug.Log("水の精霊 出現！");

            StartCoroutine(SpawnFireAfterDelay());
        }
    }

    private IEnumerator SpawnFireAfterDelay()
    {
        yield return new WaitForSeconds(fireSpawnDelay);

        if (forestBoss == null || forestBoss.IsDead)
        {
            yield break;
        }

        fireSpawned = true;

        SetSpiritActive(fireSpirit, true);

        Debug.Log("火の精霊 出現！");

        StartCoroutine(SpawnLightAfterDelay());
    }

    private IEnumerator SpawnLightAfterDelay()
    {
        yield return new WaitForSeconds(lightSpawnDelay);

        if (forestBoss == null || forestBoss.IsDead)
        {
            yield break;
        }

        lightSpawned = true;

        SetSpiritActive(lightSpirit, true);

        Debug.Log("光の精霊 出現！");
    }

    private void SetSpiritActive(
        ForestSpirit spirit,
        bool active
    )
    {
        if (spirit == null)
        {
            return;
        }

        spirit.gameObject.SetActive(active);
    }
}
