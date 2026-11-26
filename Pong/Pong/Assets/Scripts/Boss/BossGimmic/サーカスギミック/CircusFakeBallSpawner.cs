
using UnityEngine;

public class CircusFakeBallSpawner : MonoBehaviour
{
    [Header("FakeBall Prefab")]
    [SerializeField]
    private CircusFakeBall fakeBallPrefab;

    [Header("出現位置")]
    [SerializeField]
    private Transform spawnPoint;

    [Header("Paddle")]
    [SerializeField]
    private Transform player1Paddle;

    [SerializeField]
    private Transform player2Paddle;

    // ========================================
    // FakeBall出現
    // ========================================

    public void SpawnFakeBall()
    {
        if (fakeBallPrefab == null)
        {
            Debug.LogWarning(
                "FakeBall Prefabが設定されていません！",
                this
            );

            return;
        }

        Vector3 position =
            spawnPoint != null
                ? spawnPoint.position
                : transform.position;

        CircusFakeBall fakeBall = Instantiate(
            fakeBallPrefab,
            position,
            Quaternion.identity
        );

        fakeBall.Initialize(
            player1Paddle,
            player2Paddle
        );

        Debug.Log(
            "【Circus】FakeBallを生成！"
        );
    }
}
