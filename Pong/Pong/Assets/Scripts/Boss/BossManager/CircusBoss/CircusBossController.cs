
using UnityEngine;

public class CircusBossController : BossController
{
    // ========================================
    // Ballダメージ設定
    // ========================================

    [Header("Ballダメージ設定")]
    [Tooltip("Ballの現在速度に掛ける倍率")]
    [SerializeField]
    private float ballDamageMultiplier = 1f;

    // ========================================
    // ジャックポット設定
    // ========================================

    [Header("ジャックポット設定")]
    [SerializeField]
    private float jackpotMultiplier = 2f;

    [SerializeField]
    private SpriteRenderer ballRenderer;

    [SerializeField]
    private Color jackpotColor = Color.yellow;

    private bool jackpotActive;

    private Color originalBallColor = Color.white;

    // ========================================
    // 初期化
    // ========================================

    protected override void Start()
    {
        base.Start();

        if (ballRenderer != null)
        {
            originalBallColor = ballRenderer.color;
        }
    }

    // ========================================
    // ジャックポット付与
    // ========================================

    public void ActivateJackpot()
    {
        if (IsDead)
        {
            return;
        }

        jackpotActive = true;

        if (ballRenderer != null)
        {
            ballRenderer.color = jackpotColor;
        }

        Debug.Log(
            "ジャックポット発動！次の有効な一撃が2倍！"
        );
    }

    // ========================================
    // Ball衝突判定
    // ========================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();

        if (ball == null)
        {
            ball = other.GetComponentInParent<Ball>();
        }

        if (ball == null)
        {
            return;
        }

        // Ballの現在速度を取得
        float damage = ball.currentSpeed;

        // ステージ側の倍率を適用
        damage *= ballDamageMultiplier;

        // 既存のBallパワーアップを適用
        if (ball.hasPowerUp)
        {
            damage *= ball.powerMultiplier;
        }

        ApplyBallDamage(damage);
    }

    // ========================================
    // ダメージ処理
    // ========================================

    private void ApplyBallDamage(float damage)
    {
        if (!BattleStarted ||
            IsDead ||
            IsPhaseChanging)
        {
            return;
        }

        if (damage <= 0f)
        {
            return;
        }

        float finalDamage = damage;

        // ジャックポット中はダメージ2倍
        if (jackpotActive)
        {
            finalDamage *= jackpotMultiplier;
        }

        // ダメージ前のHPを保存
        float hpBefore = CurrentHP;

        // BossControllerのダメージ処理を使用
        base.TakeDamage(finalDamage);

        // 実際にHPが減った場合だけ
        // ジャックポットを消費する
        if (jackpotActive && CurrentHP < hpBefore)
        {
            DeactivateJackpot();
        }

        Debug.Log(
            $"【Circus】Ballダメージ：{finalDamage} " +
            $"残りHP：{CurrentHP}"
        );
    }

    // ========================================
    // ジャックポット解除
    // ========================================

    private void DeactivateJackpot()
    {
        jackpotActive = false;

        if (ballRenderer != null)
        {
            ballRenderer.color = originalBallColor;
        }

        Debug.Log(
            "ジャックポット効果終了！"
        );
    }

    // ========================================
    // 状態取得
    // ========================================

    public bool IsJackpotActive()
    {
        return jackpotActive;
    }
}
