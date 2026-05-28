using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("ScriptableObject 設定を受け取る")]
    public BossData data;

    [Header("背景UI（Canvas の Image）")]
    public Image backgroundUI;

    // ▼ 派生クラスでも使うので protected に変更
    protected float hp;
    protected float moveSpeed;
    protected float moveRangeX;
    protected float moveRangeY;

    protected Vector3 targetPos;
    protected float attackTimer;

    // ▼ override される可能性があるので virtual に
    protected virtual void Start()
    {
        hp = data.maxHP;
        moveSpeed = data.moveSpeed;
        moveRangeX = data.moveRangeX;
        moveRangeY = data.moveRangeY;

        if (backgroundUI != null && data.backgroundImage != null)
            backgroundUI.sprite = data.backgroundImage;

        SetNewTarget();
    }

    void Update()
    {
        Move();
        HandleAttack();
    }

    protected void SetNewTarget()
    {
        targetPos = new Vector3(
            Random.Range(-moveRangeX, moveRangeX),
            Random.Range(-moveRangeY, moveRangeY),
            0
        );
    }

    // ▼ GiusController が override するので virtual に
    protected virtual void Move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            SetNewTarget();
        }
    }

    void HandleAttack()
    {
        if (data.attackPattern == null) return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= data.attackPattern.attackInterval)
        {
            data.attackPattern.ExecuteAttack(this);
            attackTimer = 0f;
        }
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        Debug.Log($"{data.bossName} HP: {hp}");

        if (hp <= 0)
            Die();
    }
    protected virtual void Die()
    {
        Debug.Log($"{data.bossName} Defeated!");
        Destroy(gameObject);
    }

}