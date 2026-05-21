using UnityEngine;

public class BallSmashManager : MonoBehaviour
{
    private Ball ball;

    public float smashBoost = 5f;
    private bool isSmashed = false;

    // スマッシュ前の速度を保存
    private float beforeSmashSpeed;

    private void Awake()
    {
        ball = GetComponent<Ball>();
    }

    /// <summary>
    /// スマッシュ発動（Paddle に当たった瞬間に呼ぶ）
    /// </summary>
    public void ApplySmash()
    {
        if (isSmashed) return;

        isSmashed = true;

        // スマッシュ前の速度を保存
        beforeSmashSpeed = ball.currentSpeed;

        // 最大速度制限を無視
        ball.ignoreMaxSpeed = true;

        // スマッシュ分だけ速度を上げる
        ball.IncreaseSpeed(smashBoost);
    }

    /// <summary>
    /// スマッシュ終了（一定時間後 or 次のヒット時）
    /// </summary>
    public void EndSmash()
    {
        if (!isSmashed) return;

        isSmashed = false;

        // 最大速度制限を戻す
        ball.ignoreMaxSpeed = false;

        // スマッシュ前の速度に戻す
        ball.SetSpeed(beforeSmashSpeed);
    }

    /// <summary>
    /// ラウンドリセット時に強制終了
    /// </summary>
    public void ResetSmash()
    {
        isSmashed = false;
        ball.ignoreMaxSpeed = false;
    }
}
