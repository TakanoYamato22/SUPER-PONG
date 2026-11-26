
using UnityEngine;

public class ForestGustManager : MonoBehaviour
{
    [Header("参照")]
    [SerializeField]
    private Ball ball;

    [SerializeField]
    private ForestSpirit windSpirit;

    [SerializeField]
    private ForestBossController forestBoss;

    [Header("突風設定")]
    [SerializeField, Min(0.1f)]
    private float gustDuration = 3f;

    [SerializeField, Min(0.1f)]
    private float cooldownDuration = 12f;

    [SerializeField, Min(0f)]
    private float speedChange = 2f;

    [Header("突風エフェクト")]
    [SerializeField]
    private ParticleSystem rightGustEffect;

    [SerializeField]
    private ParticleSystem leftGustEffect;

    private float timer;

    private bool gimmickEnabled;
    private bool gustActive;
    private bool gustRight;

    private int lastBallDirection;
    private int lastGustDirection;

    private void Awake()
    {
        StopEffect(rightGustEffect);
        StopEffect(leftGustEffect);
    }

    private void Update()
    {
        if (windSpirit == null || forestBoss == null)
        {
            return;
        }

        // 戦闘開始前・風の精霊が未出現の場合
        if (!forestBoss.BattleStarted ||
            !windSpirit.gameObject.activeInHierarchy ||
            windSpirit.IsDead ||
            forestBoss.IsDead)
        {
            if (gimmickEnabled)
            {
                StopGust();
            }

            return;
        }

        // 風の精霊が出現したら開始
        if (!gimmickEnabled)
        {
            StartGust();
        }

        if (ball == null)
        {
            return;
        }

        timer += Time.deltaTime;

        if (gustActive)
        {
            if (timer >= gustDuration)
            {
                StartCooldown();
                return;
            }

            CheckBallDirection();
        }
        else
        {
            if (timer >= cooldownDuration)
            {
                StartRandomGust();
            }
        }
    }

    // ========================================
    // 突風ギミック開始
    // ========================================

    public void StartGust()
    {
        if (gimmickEnabled || ball == null)
        {
            return;
        }

        gimmickEnabled = true;

        StartRandomGust();

        Debug.Log("風の精霊：突風ギミック開始！");
    }

    // ========================================
    // ランダム方向の突風
    // ========================================

    private void StartRandomGust()
    {
        gustRight = Random.value < 0.5f;

        gustActive = true;
        timer = 0f;

        ResetDirectionCheck();

        if (gustRight)
        {
            StopEffect(leftGustEffect);
            PlayEffect(rightGustEffect);

            Debug.Log("右向きの突風！");
        }
        else
        {
            StopEffect(rightGustEffect);
            PlayEffect(leftGustEffect);

            Debug.Log("左向きの突風！");
        }

        CheckBallDirection();
    }

    // ========================================
    // Ballの速度変更
    // ========================================

    private void CheckBallDirection()
    {
        Vector2 velocity = ball.velocity;

        if (velocity.sqrMagnitude < 0.001f)
        {
            lastBallDirection = 0;
            return;
        }

        if (Mathf.Abs(velocity.x) < 0.1f)
        {
            return;
        }

        int ballDirection = velocity.x > 0f ? 1 : -1;
        int gustDirection = gustRight ? 1 : -1;

        // 同じ方向の組み合わせなら変更しない
        if (ballDirection == lastBallDirection &&
            gustDirection == lastGustDirection)
        {
            return;
        }

        lastBallDirection = ballDirection;
        lastGustDirection = gustDirection;

        if (ballDirection == gustDirection)
        {
            // 追い風なら加速
            ball.SetSpeed(ball.currentSpeed + speedChange);

            Debug.Log(
                $"追い風！Ball速度：{ball.currentSpeed}"
            );
        }
        else
        {
            // 向かい風なら減速
            ball.SetSpeed(ball.currentSpeed - speedChange);

            Debug.Log(
                $"向かい風！Ball速度：{ball.currentSpeed}"
            );
        }
    }

    // ========================================
    // クールタイム
    // ========================================

    private void StartCooldown()
    {
        gustActive = false;
        timer = 0f;

        ResetDirectionCheck();

        StopEffect(rightGustEffect);
        StopEffect(leftGustEffect);

        Debug.Log("突風終了！クールタイム開始！");
    }

    // ========================================
    // ギミック停止
    // ========================================

    public void StopGust()
    {
        gimmickEnabled = false;
        gustActive = false;
        timer = 0f;

        ResetDirectionCheck();

        StopEffect(rightGustEffect);
        StopEffect(leftGustEffect);

        Debug.Log("風の精霊：突風ギミック停止！");
    }

    // ========================================
    // 判定リセット
    // ========================================

    private void ResetDirectionCheck()
    {
        lastBallDirection = 0;
        lastGustDirection = 0;
    }

    // ========================================
    // エフェクト再生
    // ========================================

    private void PlayEffect(ParticleSystem effect)
    {
        if (effect == null)
        {
            return;
        }

        if (!effect.gameObject.activeSelf)
        {
            effect.gameObject.SetActive(true);
        }

        if (!effect.gameObject.activeInHierarchy)
        {
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
        {
            return;
        }

        effect.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );
    }
}
