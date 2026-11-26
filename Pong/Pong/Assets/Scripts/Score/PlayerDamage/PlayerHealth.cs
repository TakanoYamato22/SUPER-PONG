
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private float maxHP = 150f;

    [Header("UI")]
    [SerializeField] private Image healthImage;
    [SerializeField] private GameObject gameOverText;

    [Header("無敵時のHPバー色")]
    [SerializeField]
    private Color invincibleHealthColor =
        new Color(0.3f, 0.9f, 1f, 1f);

    private float currentHP;

    private bool isDead;
    private bool hasShield;
    private bool damageDisabled;

    // 元のHPバーの色を保存
    private Color normalHealthColor;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public bool IsDead => isDead;
    public bool IsDamageDisabled => damageDisabled;

    // ========================================
    // 初期化
    // ========================================

    private void Awake()
    {
        // Inspectorで設定した元の色を保存
        if (healthImage != null)
        {
            normalHealthColor = healthImage.color;
        }

        InitializeHealth();
    }

    private void InitializeHealth()
    {
        currentHP = maxHP;

        isDead = false;
        hasShield = false;
        damageDisabled = false;

        UpdateHealthBar();
        UpdateHealthBarColor();

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }
    }

    // ========================================
    // 最大HPの設定
    // ========================================

    public void SetMaxHP(float newMaxHP, bool fullyHeal = true)
    {
        maxHP = Mathf.Max(1f, newMaxHP);

        if (fullyHeal)
        {
            currentHP = maxHP;
            isDead = false;
        }
        else
        {
            currentHP = Mathf.Clamp(
                currentHP,
                0f,
                maxHP
            );
        }

        UpdateHealthBar();

        Debug.Log(
            gameObject.name +
            " の最大HPを " +
            maxHP +
            " に設定"
        );
    }

    // ========================================
    // ダメージ処理
    // ========================================

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        // 無敵中はHPもシールドも消費しない
        if (damageDisabled)
            return;

        if (hasShield)
        {
            hasShield = false;

            Debug.Log(
                gameObject.name +
                " のシールドでダメージ無効！"
            );

            return;
        }

        currentHP -= damage;

        currentHP = Mathf.Clamp(
            currentHP,
            0f,
            maxHP
        );

        UpdateHealthBar();

        if (currentHP <= 0f)
        {
            GameOver();
        }
    }

    // ========================================
    // 回復
    // ========================================

    public void Heal(float amount)
    {
        if (isDead)
            return;

        currentHP += amount;

        currentHP = Mathf.Clamp(
            currentHP,
            0f,
            maxHP
        );

        UpdateHealthBar();
    }

    // ========================================
    // シールド
    // ========================================

    public void GiveShield()
    {
        if (isDead)
            return;

        hasShield = true;

        Debug.Log(
            gameObject.name +
            " にシールド付与！"
        );
    }

    // ========================================
    // ダメージ無効化
    // ========================================

    public void SetDamageDisabled(bool disabled)
    {
        // 無敵状態に切り替える
        damageDisabled = disabled;

        if (disabled && !isDead)
        {
            // 無敵になった瞬間にHPを全回復
            currentHP = maxHP;

            // HPバーを満タンに更新
            UpdateHealthBar();
        }

        // 無敵なら水色、通常なら元の色
        UpdateHealthBarColor();

        Debug.Log(
            gameObject.name +
            (disabled
                ? "：HP全回復＆無敵開始！"
                : "：ダメージ無効化終了！")
        );
    }

    // ========================================
    // 時間切れによる強制敗北
    // ========================================

    public void ForceGameOver()
    {
        if (isDead)
            return;

        damageDisabled = false;

        currentHP = 0f;

        UpdateHealthBar();
        UpdateHealthBarColor();

        Debug.Log(
            gameObject.name +
            "：時間切れによる強制敗北！"
        );

        GameOver();
    }

    // ========================================
    // HPバーの更新
    // ========================================

    private void UpdateHealthBar()
    {
        if (healthImage == null)
            return;

        healthImage.fillAmount = currentHP / maxHP;
    }

    // ========================================
    // HPバーの色変更
    // ========================================

    private void UpdateHealthBarColor()
    {
        if (healthImage == null)
            return;

        healthImage.color = damageDisabled
            ? invincibleHealthColor
            : normalHealthColor;
    }

    // ========================================
    // ゲームオーバー
    // ========================================

    private void GameOver()
    {
        if (isDead)
            return;

        isDead = true;

        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}
