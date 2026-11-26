
using UnityEngine;

public class ForestSpiritMovement : MonoBehaviour
{
    // ========================================
    // 移動設定
    // ========================================

    [Header("移動設定")]
    [SerializeField, Min(0f)]
    private float moveSpeed = 3f;

    [SerializeField, Min(0.1f)]
    private float directionChangeInterval = 2f;

    // ========================================
    // 移動範囲
    // ========================================

    [Header("移動範囲")]
    [SerializeField]
    private Vector2 minPosition = new Vector2(-7f, -4f);

    [SerializeField]
    private Vector2 maxPosition = new Vector2(7f, 4f);

    // ========================================
    // 内部変数
    // ========================================

    private Vector2 moveDirection;
    private float directionTimer;

    // ========================================
    // 初期化
    // ========================================

    private void OnEnable()
    {
        directionTimer = 0f;
        ChangeDirection();
    }

    // ========================================
    // 移動処理
    // ========================================

    private void Update()
    {
        directionTimer += Time.deltaTime;

        if (directionTimer >= directionChangeInterval)
        {
            ChangeDirection();
        }

        Vector2 currentPosition = transform.position;

        Vector2 nextPosition =
            currentPosition +
            moveDirection * moveSpeed * Time.deltaTime;

        // ========================================
        // 左右の端で反射
        // ========================================

        if (nextPosition.x <= minPosition.x)
        {
            nextPosition.x = minPosition.x;
            moveDirection.x = Mathf.Abs(moveDirection.x);
        }
        else if (nextPosition.x >= maxPosition.x)
        {
            nextPosition.x = maxPosition.x;
            moveDirection.x = -Mathf.Abs(moveDirection.x);
        }

        // ========================================
        // 上下の端で反射
        // ========================================

        if (nextPosition.y <= minPosition.y)
        {
            nextPosition.y = minPosition.y;
            moveDirection.y = Mathf.Abs(moveDirection.y);
        }
        else if (nextPosition.y >= maxPosition.y)
        {
            nextPosition.y = maxPosition.y;
            moveDirection.y = -Mathf.Abs(moveDirection.y);
        }

        transform.position = new Vector3(
            nextPosition.x,
            nextPosition.y,
            transform.position.z
        );
    }

    // ========================================
    // ランダム方向変更
    // ========================================

    private void ChangeDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        directionTimer = 0f;
    }

    // ========================================
    // 移動範囲の表示
    // ========================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 center = new Vector3(
            (minPosition.x + maxPosition.x) * 0.5f,
            (minPosition.y + maxPosition.y) * 0.5f,
            transform.position.z
        );

        Vector3 size = new Vector3(
            maxPosition.x - minPosition.x,
            maxPosition.y - minPosition.y,
            0f
        );

        Gizmos.DrawWireCube(center, size);
    }
}
