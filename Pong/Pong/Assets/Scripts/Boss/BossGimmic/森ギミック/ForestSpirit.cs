
using UnityEngine;

public class ForestSpirit : MonoBehaviour
{
    public enum SpiritType
    {
        Wind,
        Water,
        Fire,
        Light
    }

    // ========================================
    // 精霊の設定
    // ========================================

    [Header("精霊の種類")]
    [SerializeField]
    private SpiritType spiritType = SpiritType.Wind;

    [Header("精霊データ")]
    [Tooltip("この精霊が使用するBossData")]
    public BossData data;

    [Header("古代樹")]
    [SerializeField]
    private ForestBossController forestBoss;

    [Header("精霊HPバー")]
    [SerializeField]
    private Transform hpFill;

    // ========================================
    // ダメージ設定
    // ========================================

    [Header("精霊ダメージ管理")]
    [SerializeField]
    private SpiritDamageManager damageManager;

    [Header("Ballとの衝突")]
    [SerializeField, Min(0f)]
    private float ballDamage = 10f;

    [Header("動作確認")]
    [SerializeField]
    private bool enableDamageTest = false;

    // ========================================
    // 効果音
    // ========================================

    [Header("効果音")]
    [SerializeField]
    private AudioClip hitSE;

    [SerializeField]
    private AudioClip deadSE;

    // ========================================
    // 内部変数
    // ========================================

    private float hp;
    private bool isDead;

    private Vector3 hpFillStartScale;
    private Vector3 hpFillStartPosition;

    public float MaxHP =>
        data != null ? data.maxHP : 0f;

    public float CurrentHP => hp;

    public bool IsDead => isDead;

    // ========================================
    // 初期化
    // ========================================

    private void Awake()
    {
        if (data == null)
        {
            Debug.LogError(
                $"{name}：BossDataが設定されていません！",
                this
            );
        }

        hp = MaxHP;
        isDead = false;

        if (hpFill != null)
        {
            hpFillStartScale = hpFill.localScale;
            hpFillStartPosition = hpFill.localPosition;
        }

        if (damageManager == null)
        {
            damageManager =
                GetComponent<SpiritDamageManager>();
        }

        UpdateHpBar();
    }

    // ========================================
    // 動作確認
    // ========================================

    private void Update()
    {
        if (enableDamageTest &&
            Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10f);
        }
    }

    // ========================================
    // Ballとの衝突
    // ========================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();

        if (ball == null ||
            forestBoss == null ||
            !forestBoss.BattleStarted ||
            forestBoss.IsDead ||
            forestBoss.IsPhaseChanging ||
            isDead)
        {
            return;
        }

        BallSmashManager smashManager =
            ball.GetComponent<BallSmashManager>();

        switch (spiritType)
        {
            case SpiritType.Wind:

                // 通常Ball・Smash Ballでダメージ
                TakeDamage(ballDamage);
                break;

            case SpiritType.Water:

                // Smash Ballのみダメージ
                if (smashManager != null &&
                    smashManager.IsSmashed)
                {
                    TakeDamage(ballDamage);
                }

                break;

            case SpiritType.Fire:

                // 炎の精霊はBallではダメージを受けない
                // 反射した炎の弾でのみダメージ
                break;

            case SpiritType.Light:

                // 通常Ball・Smash Ballでダメージ
                TakeDamage(ballDamage);
                break;
        }
    }

    // ========================================
    // ダメージ処理
    // ========================================

    public void TakeDamage(float damage)
    {
        if (data == null ||
            forestBoss == null ||
            !forestBoss.BattleStarted ||
            forestBoss.IsDead ||
            forestBoss.IsPhaseChanging ||
            isDead ||
            damage <= 0f)
        {
            return;
        }

        // 精霊自身の無敵時間
        if (damageManager != null &&
            damageManager.IsInvincible)
        {
            return;
        }

        float actualDamage = Mathf.Min(
            hp,
            damage
        );

        hp = Mathf.Max(
            hp - actualDamage,
            0f
        );

        // HPバー更新
        UpdateHpBar();

        // 古代樹のHPを同期
        forestBoss.OnSpiritDamaged(
            this,
            actualDamage
        );

        Debug.Log(
            $"{name} HP：{hp} / {MaxHP}",
            this
        );

        // 精霊撃破
        if (hp <= 0f)
        {
            Die();
            return;
        }

        // 赤点滅・無敵時間・Effect
        if (damageManager != null)
        {
            damageManager.PlayHitEffect();
        }
    }

    // ========================================
    // HPバー更新
    // ========================================

    private void UpdateHpBar()
    {
        if (hpFill == null ||
            MaxHP <= 0f)
        {
            return;
        }

        float hpPercent = Mathf.Clamp01(
            hp / MaxHP
        );

        hpFill.localScale = new Vector3(
            hpFillStartScale.x * hpPercent,
            hpFillStartScale.y,
            hpFillStartScale.z
        );

        hpFill.localPosition = new Vector3(
            hpFillStartPosition.x -
            hpFillStartScale.x *
            (1f - hpPercent) * 0.5f,
            hpFillStartPosition.y,
            hpFillStartPosition.z
        );
    }

    // ========================================
    // 精霊撃破
    // ========================================

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log(
            $"{name} 撃破！",
            this
        );

        gameObject.SetActive(false);
    }
}
