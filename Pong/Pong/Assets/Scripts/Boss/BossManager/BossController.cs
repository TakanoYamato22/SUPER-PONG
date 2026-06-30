using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("ScriptableObject 設定")]
    public BossData data;

    [Header("UI")]
    public Image backgroundUI;
    public Slider hpSlider;

    [Header("Sprite HPバー")]
    [SerializeField] private Transform hpFill;

    [SerializeField] private BossBallGimmickRunner gimmickRunner;
    [SerializeField] private BossDamageManager damageManager;
    [SerializeField] private GameObject clearText;

    [Header("効果音")]
    
    [SerializeField] private AudioClip hitSE;
    [SerializeField] private AudioClip deadSE;

    [Header("撃破演出")]
    [SerializeField] private float deadWaitTime = 0.8f;

    protected float hp;
    protected float moveSpeed;
    protected float moveRangeX;
    protected float moveRangeY;

    private bool battleStarted = false;
    private bool isDead = false;

    private Vector3 hpFillStartScale;
    private Vector3 hpFillStartPosition;

    protected virtual void Start()
    {
        if (clearText != null)
            clearText.SetActive(false);

        hp = data.maxHP;
        moveSpeed = data.moveSpeed;
        moveRangeX = data.moveRangeX;
        moveRangeY = data.moveRangeY;

        if (hpSlider != null)
        {
            hpSlider.maxValue = hp;
            hpSlider.value = hp;
        }

        if (hpFill != null)
        {
            hpFillStartScale = hpFill.localScale;
            hpFillStartPosition = hpFill.localPosition;
        }

        if (gimmickRunner != null && data.gimmick != null)
            gimmickRunner.SetGimmick(data.gimmick);

        if (damageManager != null)
        {
            StartCoroutine(damageManager.StartInvincible(0f));
        }
    }

    protected virtual void Update()
    {
        if (!battleStarted)
            return;

        Move();
    }

    protected virtual void Move()
    {
        // 各ボスでオーバーライド
    }

    public void SetBattleStarted(bool started)
    {
        battleStarted = started;
        Debug.Log("BattleStarted = " + battleStarted);
    }

    public virtual void TakeDamage(float damage)
    {
        if (!battleStarted)
            return;

        if (isDead)
            return;

        if (damageManager != null && damageManager.IsInvincible)
            return;

        hp -= damage;
        hp = Mathf.Max(hp, 0);


        if (hpSlider != null)
            hpSlider.value = hp;

        if (hpFill != null)
        {
            float hpPercent = hp / data.maxHP;

            hpFill.localScale = new Vector3(
                hpFillStartScale.x * hpPercent,
                hpFillStartScale.y,
                hpFillStartScale.z
            );

            hpFill.localPosition = new Vector3(
                hpFillStartPosition.x - hpFillStartScale.x * (1f - hpPercent) * 0.5f,
                hpFillStartPosition.y,
                hpFillStartPosition.z
            );
        }

        if (gimmickRunner != null)
            gimmickRunner.OnBossHpChanged(hp, data.maxHP);

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
        if (isDead)
            return;

        isDead = true;
        battleStarted = false;

        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {

        yield return new WaitForSecondsRealtime(deadWaitTime);

        if (clearText != null)
            clearText.SetActive(true);

        Time.timeScale = 0f;
        gameObject.SetActive(false);
    }
}