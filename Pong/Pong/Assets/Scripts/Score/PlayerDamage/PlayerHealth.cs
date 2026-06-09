using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private Image healthImage;
    [SerializeField] private GameObject gameOverText;

    private float currentHP;
    private bool isDead;

    private void Start()
    {
        currentHP = maxHP;

        if (healthImage != null)
            healthImage.fillAmount = 1f;

        if (gameOverText != null)
            gameOverText.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        if (healthImage != null)
            healthImage.fillAmount = currentHP / maxHP;

        if (currentHP <= 0)
        {
            isDead = true;

            if (gameOverText != null)
                gameOverText.SetActive(true);

            Time.timeScale = 0f;
        }
    }
}