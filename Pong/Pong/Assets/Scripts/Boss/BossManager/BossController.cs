using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("ScriptableObject 設定")]
    public BossData data;

[Header("UI")]
    public Image backgroundUI;
    public Slider hpSlider;

    [SerializeField] private BossBallGimmickRunner gimmickRunner;
    [SerializeField] private BossDamageManager damageManager;

    protected float hp;
    protected float moveSpeed;
    protected float moveRangeX;
    protected float moveRangeY;

    protected virtual void Start()
    {
        hp = data.maxHP;
        moveSpeed = data.moveSpeed;
        moveRangeX = data.moveRangeX;
        moveRangeY = data.moveRangeY;

        if (hpSlider != null)
        {
            hpSlider.maxValue = hp;
            hpSlider.value = hp;
        }

        if (backgroundUI != null && data.backgroundImage != null)
            backgroundUI.sprite = data.backgroundImage;

        if (gimmickRunner != null && data.gimmick != null)
            gimmickRunner.SetGimmick(data.gimmick);
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        // デフォルトの移動
    }

    public virtual void TakeDamage(float damage)
    {
        if (damageManager != null && damageManager.IsInvincible)
            return;

        hp -= damage;
        hp = Mathf.Max(hp, 0);

        if (hpSlider != null)
            hpSlider.value = hp;

        gimmickRunner?.OnBossHpChanged(hp, data.maxHP);

        if (hp <= 0)
        {
            Die();
            return;
        }

        if (damageManager != null)
        {
            damageManager.PlayHitEffect(this);
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{data.bossName} Defeated!");
        Destroy(gameObject);
    }

}
