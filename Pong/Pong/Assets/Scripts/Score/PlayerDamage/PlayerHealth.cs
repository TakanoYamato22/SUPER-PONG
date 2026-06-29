using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 150f;
    [SerializeField] private Image healthImage;
    [SerializeField] private GameObject gameOverText;

    private float currentHP;
    private bool isDead;
    private bool hasShield;

    private void Start()
    {
        currentHP = maxHP;
        isDead = false;
        hasShield = false;

        if (healthImage != null)
            healthImage.fillAmount = 1f;

        if (gameOverText != null)
            gameOverText.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if (hasShield)
        {
            hasShield = false;
            Debug.Log(gameObject.name + " のシールドでダメージ無効！");
            return;
        }

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        UpdateHealthBar();

        if (currentHP <= 0)
        {
            isDead = true;

            if (gameOverText != null)
                gameOverText.SetActive(true);

            Time.timeScale = 0f;
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        UpdateHealthBar();
    }

    public void GiveShield()
    {
        if (isDead) return;

        hasShield = true;
        Debug.Log(gameObject.name + " にシールド付与！");
    }

    private void UpdateHealthBar()
    {
        if (healthImage != null)
            healthImage.fillAmount = currentHP / maxHP;
    }
}