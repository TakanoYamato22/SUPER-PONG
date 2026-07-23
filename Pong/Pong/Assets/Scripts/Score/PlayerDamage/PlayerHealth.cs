using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private float maxHP = 150f;

    [Header("UI")]
    [SerializeField] private Image healthImage;
    [SerializeField] private GameObject gameOverText;

    private float currentHP;
    private bool isDead;
    private bool hasShield;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public bool IsDead => isDead;

    private void Awake()
    {
        InitializeHealth();
    }

    private void InitializeHealth()
    {
        currentHP = maxHP;
        isDead = false;
        hasShield = false;

        UpdateHealthBar();

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }
    }

    // キャラ選択結果から最大HPを設定
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
            currentHP = Mathf.Clamp(currentHP, 0f, maxHP);
        }

        UpdateHealthBar();

        Debug.Log(
            gameObject.name +
            " の最大HPを " +
            maxHP +
            " に設定"
        );
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
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
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        UpdateHealthBar();

        if (currentHP <= 0f)
        {
            GameOver();
        }
    }

    public void Heal(float amount)
    {
        if (isDead)
            return;

        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        UpdateHealthBar();
    }

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

    private void UpdateHealthBar()
    {
        if (healthImage == null)
            return;

        healthImage.fillAmount = currentHP / maxHP;
    }

    private void GameOver()
    {
        isDead = true;

        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}