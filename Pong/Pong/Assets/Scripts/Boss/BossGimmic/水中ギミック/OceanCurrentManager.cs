
using UnityEngine;

public class OceanCurrentManager : MonoBehaviour
{
    [Header("Ball")]
    [SerializeField] private Ball ball;

    [Header("海流設定")]
    [SerializeField, Min(0.1f)]
    private float currentDuration = 10f;

    [SerializeField, Min(0.1f)]
    private float cooldownDuration = 10f;

    [SerializeField]
    private float speedChange = 5f;

    [Header("初期方向")]
    [SerializeField] private bool startRight = true;

    [Header("濁流エフェクト")]
    [SerializeField] private ParticleSystem rightCurrentEffect;
    [SerializeField] private ParticleSystem leftCurrentEffect;

    private float timer;

    private bool currentRight;
    private bool gimmickEnabled;
    private bool currentActive;

    // 最後に適用した方向
    private int lastBallDirection;
    private int lastCurrentDirection;

    private void Awake()
    {
        StopEffect(rightCurrentEffect);
        StopEffect(leftCurrentEffect);
    }

    private void Update()
    {
        if (!gimmickEnabled || ball == null)
            return;

        timer += Time.deltaTime;

        if (currentActive)
        {
            // 海流が発生している時間
            if (timer >= currentDuration)
            {
                StartCooldown();
                return;
            }

            CheckBallDirection();
        }
        else
        {
            // クールタイム中
            if (timer >= cooldownDuration)
            {
                // 次は逆方向の海流
                currentRight = !currentRight;

                StartCurrent();
            }
        }
    }

    // ========================================
    // Ballの速度変更
    // ========================================

    private void CheckBallDirection()
    {
        Vector2 velocity = ball.velocity;

        // Ballが停止している場合
        if (velocity.sqrMagnitude < 0.001f)
        {
            lastBallDirection = 0;
            return;
        }

        // 蛇行による細かい方向変化を無視
        if (Mathf.Abs(velocity.x) < 0.1f)
            return;

        int ballDirection =
            velocity.x > 0f ? 1 : -1;

        int currentDirection =
            currentRight ? 1 : -1;

        // 前回と同じ組み合わせなら変更しない
        if (ballDirection == lastBallDirection &&
            currentDirection == lastCurrentDirection)
        {
            return;
        }

        lastBallDirection = ballDirection;
        lastCurrentDirection = currentDirection;

        // 順流なら加速
        if (ballDirection != currentDirection)
        {
            ball.SetSpeed(
                ball.currentSpeed + speedChange
            );

            Debug.Log(
                $"順流！Ball速度：{ball.currentSpeed}"
            );
        }
        else
        {
            // 逆流なら減速
            ball.SetSpeed(
                ball.currentSpeed - speedChange
            );

            Debug.Log(
                $"逆流！Ball速度：{ball.currentSpeed}"
            );
        }
    }

    // ========================================
    // 第2形態開始時に呼ぶ
    // ========================================

    public void StartOceanCurrent()
    {
        if (ball == null)
        {
            Debug.LogWarning(
                "OceanCurrentManager：Ballが未設定！",
                this
            );
            return;
        }

        if (gimmickEnabled)
            return;

        gimmickEnabled = true;
        currentRight = startRight;

        StartCurrent();

        Debug.Log("海流ギミック開始！");
    }

    // ========================================
    // 海流開始
    // ========================================

    private void StartCurrent()
    {
        timer = 0f;
        currentActive = true;

        ResetDirectionCheck();

        if (currentRight)
        {
            StopEffect(leftCurrentEffect);
            PlayEffect(rightCurrentEffect);

            Debug.Log("右向きの海流開始！");
        }
        else
        {
            StopEffect(rightCurrentEffect);
            PlayEffect(leftCurrentEffect);

            Debug.Log("左向きの海流開始！");
        }
    }

    // ========================================
    // クールタイム開始
    // ========================================

    private void StartCooldown()
    {
        timer = 0f;
        currentActive = false;

        ResetDirectionCheck();

        // 両方の濁流を停止
        StopEffect(rightCurrentEffect);
        StopEffect(leftCurrentEffect);

        Debug.Log("海流終了！クールタイム開始！");
    }

    // ========================================
    // 海流ギミック停止
    // ========================================

    public void StopOceanCurrent()
    {
        gimmickEnabled = false;
        currentActive = false;

        timer = 0f;

        ResetDirectionCheck();

        StopEffect(rightCurrentEffect);
        StopEffect(leftCurrentEffect);

        Debug.Log("海流ギミック停止！");
    }

    // ========================================
    // 判定リセット
    // ========================================

    private void ResetDirectionCheck()
    {
        lastBallDirection = 0;
        lastCurrentDirection = 0;
    }

    // ========================================
    // エフェクト再生
    // ========================================

    private void PlayEffect(ParticleSystem effect)
    {
        if (effect == null)
            return;

        if (!effect.gameObject.activeSelf)
        {
            effect.gameObject.SetActive(true);
        }

        if (!effect.gameObject.activeInHierarchy)
        {
            Debug.LogWarning(
                "海流エフェクトの親が非アクティブ！",
                effect
            );
            return;
        }

        effect.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        effect.Play(true);
    }

    // ========================================
    // エフェクト停止
    // ========================================

    private void StopEffect(ParticleSystem effect)
    {
        if (effect == null)
            return;

        effect.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );
    }
}
