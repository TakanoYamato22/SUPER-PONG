
using UnityEngine;

public class SpaceWallWarp : MonoBehaviour
{
    [Header("ワープする上下の境界")]
    [SerializeField] private Transform topWall;
    [SerializeField] private Transform bottomWall;

    [Header("ワープ位置の余白")]
    [SerializeField] private float warpOffset = 0.5f;

    private Rigidbody2D rb;
    private float ballRadius;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        CircleCollider2D circle = GetComponent<CircleCollider2D>();

        if (circle != null)
        {
            ballRadius = circle.bounds.extents.y;
        }
    }

    private void FixedUpdate()
    {
        if (topWall == null || bottomWall == null || rb == null)
            return;

        float topY = topWall.position.y;
        float bottomY = bottomWall.position.y;

        Vector2 position = rb.position;
        Vector2 velocity = rb.linearVelocity;

        // 上の壁を完全に通過した場合
        if (position.y - ballRadius > topY)
        {
            position.y = bottomY + ballRadius + warpOffset;
        }
        // 下の壁を完全に通過した場合
        else if (position.y + ballRadius < bottomY)
        {
            position.y = topY - ballRadius - warpOffset;
        }
        else
        {
            return;
        }

        rb.position = position;

        // ワープ前の速度と進行方向を維持
        rb.linearVelocity = velocity;
    }
}
