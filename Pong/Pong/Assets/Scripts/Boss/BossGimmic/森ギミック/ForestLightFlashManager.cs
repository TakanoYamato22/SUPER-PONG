
using UnityEngine;

public class ForestLightFlashManager : MonoBehaviour
{
    [Header("参照")]
    [SerializeField]
    private ForestSpirit lightSpirit;

    [SerializeField]
    private ForestBossController forestBoss;

    [Header("閃光Prefab")]
    [SerializeField]
    private ForestLightFlash flashPrefab;

    [Header("発生設定")]
    [SerializeField, Min(0.1f)]
    private float spawnInterval = 8f;

    [SerializeField, Min(0.1f)]
    private float flashDuration = 3f;

    [SerializeField, Range(0f, 1f)]
    private float flashAlpha = 0.9f;

    [SerializeField]
    private Vector3 spawnOffset = Vector3.zero;

    private float timer;
    private ForestLightFlash currentFlash;

    private void Update()
    {
        if (lightSpirit == null ||
            forestBoss == null ||
            flashPrefab == null)
        {
            return;
        }

        // 戦闘中かつ光の精霊が生存している場合のみ
        if (!forestBoss.BattleStarted ||
            forestBoss.IsDead ||
            forestBoss.IsPhaseChanging ||
            lightSpirit.IsDead ||
            !lightSpirit.gameObject.activeInHierarchy)
        {
            timer = 0f;
            return;
        }

        // 前の閃光が残っている間は新しく出さない
        if (currentFlash != null)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnFlash();
        }
    }

    private void SpawnFlash()
    {
        timer = 0f;

        // 現在位置を取得
        Vector3 spawnPosition =
            lightSpirit.transform.position + spawnOffset;

        // 精霊の子にはしない
        // そのため移動しても光は固定される
        currentFlash = Instantiate(
            flashPrefab,
            spawnPosition,
            Quaternion.identity
        );

        currentFlash.Initialize(
            flashDuration,
            flashAlpha
        );

        Debug.Log("光の精霊：閃光発生！");
    }
}
