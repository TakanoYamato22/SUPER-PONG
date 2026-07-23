using UnityEngine;

[RequireComponent(typeof(Paddle))]
public class PaddleCollisionHandler : MonoBehaviour
{
    [Header("通常反射時の基本加速量")]
    [SerializeField] private float baseSpeedIncrease = 1.5f;

    [Header("反射角度")]
    [SerializeField] private float maxBounceAngle = 45f;

    private CharacterRuntimeStats characterStats;

    private void Awake()
    {
        characterStats = GetComponent<CharacterRuntimeStats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ballタグ以外は無視
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (ball == null)
            return;

        Collider2D paddleCollider = collision.otherCollider;

        // パドルのどの高さに当たったかを -1 ～ 1 で取得
        float contactY =
            (ball.transform.position.y - paddleCollider.bounds.center.y)
            / (paddleCollider.bounds.size.y / 2f);

        contactY = Mathf.Clamp(contactY, -1f, 1f);

        // 接触位置に応じて反射角度を決める
        float bounceAngle = contactY * maxBounceAngle;
        float radians = bounceAngle * Mathf.Deg2Rad;

        // 左右どちらへ跳ね返すか
        float horizontalDirection;

        if (transform.position.x < ball.transform.position.x)
        {
            horizontalDirection = 1f;
        }
        else
        {
            horizontalDirection = -1f;
        }

        Vector2 newDirection = new Vector2(
            horizontalDirection * Mathf.Cos(radians),
            Mathf.Sin(radians)
        ).normalized;

        // まず反射方向を変更
        ball.velocity = newDirection * ball.currentSpeed;

        // 全キャラ共通の通常加速
        ball.IncreaseSpeed(baseSpeedIncrease);

        // Speedsterなどのキャラ固有倍率を反映
        float characterMultiplier = 1f;

        if (characterStats != null)
        {
            characterMultiplier =
                characterStats.NormalHitSpeedMultiplier;
        }

        if (!Mathf.Approximately(characterMultiplier, 1f))
        {
            ball.ApplyCharacterSpeedMultiplier(
                characterMultiplier,
                false
            );
        }

        // ボールがパドル内部に残らないように少し外へ出す
        float pushDistance = 0.1f;

        ball.transform.position = new Vector2(
            paddleCollider.bounds.center.x
            + horizontalDirection
            * (paddleCollider.bounds.extents.x + pushDistance),

            ball.transform.position.y
        );
    }
}