using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("第1形態データ")]
    [Tooltip("通常形態で使用するBossData")]
    public BossData data;

    [Header("第2形態設定")]
    [Tooltip("第2形態で使用するBossData。形態変化しないBossは空欄")]
    [SerializeField]
    private BossData secondPhaseData;

    [Tooltip("形態変化演出。形態変化しないBossは空欄")]
    [SerializeField]
    private BossPhaseChangerBase phaseChanger;

    [Header("Sprite HPバー")]
    [SerializeField]
    private Transform hpFill;

    [Header("Bossギミック")]
    [SerializeField]
    private BallGimmickRunner gimmickRunner;

    [Header("ダメージ管理")]
    [SerializeField]
    private BossDamageManager damageManager;

    [Header("クリア表示")]
    [SerializeField]
    private GameObject clearText;

    [Header("効果音")]
    [SerializeField]
    private AudioClip hitSE;

    [SerializeField]
    private AudioClip deadSE;

    [Header("撃破演出")]
    [Min(0f)]
    [SerializeField]
    private float deadWaitTime = 0.8f;

    protected float hp;
    protected float moveSpeed;
    protected float moveRangeX;
    protected float moveRangeY;

    private BossData currentData;

    private bool battleStarted;
    private bool isDead;
    private bool secondPhaseStarted;
    private bool phaseChanging;

    private Vector3 hpFillStartScale;
    private Vector3 hpFillStartPosition;

    public float CurrentHP =>
        hp;

    public float MaxHP =>
        currentData != null
            ? currentData.maxHP
            : 0f;

    public bool BattleStarted =>
        battleStarted;

    public bool IsDead =>
        isDead;

    public bool IsSecondPhase =>
        secondPhaseStarted;

    public bool IsPhaseChanging =>
        phaseChanging;

    protected virtual void Start()
    {
        if (clearText != null)
        {
            clearText.SetActive(false);
        }

        if (data == null)
        {
            Debug.LogError(
                "第1形態のBossDataが設定されていません。",
                this
            );

            enabled = false;
            return;
        }

        /*
         * HPバーの初期状態を先に保存する。
         * ApplyBossDataより前でも問題ないが、
         * UpdateHpBarより前に必ず実行する。
         */
        if (hpFill != null)
        {
            hpFillStartScale =
                hpFill.localScale;

            hpFillStartPosition =
                hpFill.localPosition;
        }

        currentData = data;

        ApplyBossData(
            currentData,
            true
        );

        FindPhaseChanger();

        UpdateHpBar();
        NotifyHpChanged();

        if (damageManager != null)
        {
            StartCoroutine(
                damageManager.StartInvincible(
                    0f
                )
            );
        }
    }

    protected virtual void Update()
    {
        if (!battleStarted ||
            isDead ||
            phaseChanging)
        {
            return;
        }

        Move();
    }

    /// <summary>
    /// 各Boss固有の移動。
    /// 必要なBossController派生クラスで上書きする。
    /// </summary>
    protected virtual void Move()
    {
    }

    /// <summary>
    /// 戦闘の開始・停止を設定する。
    /// </summary>
    public void SetBattleStarted(
        bool started
    )
    {
        if (isDead)
        {
            return;
        }

        if (phaseChanging && started)
        {
            return;
        }

        battleStarted = started;

        Debug.Log(
            $"BattleStarted = {battleStarted}",
            this
        );
    }

    /// <summary>
    /// Bossへダメージを与える。
    /// </summary>
    public virtual void TakeDamage(
        float damage
    )
    {
        if (!battleStarted)
        {
            return;
        }

        if (isDead ||
            phaseChanging)
        {
            return;
        }

        if (damage <= 0f)
        {
            return;
        }

        if (damageManager != null &&
            damageManager.IsInvincible)
        {
            return;
        }

        hp -= damage;
        hp = Mathf.Max(hp, 0f);

        UpdateHpBar();
        NotifyHpChanged();

        if (hp <= 0f)
        {
            HandleHpZero();
            return;
        }

        if (damageManager != null)
        {
            damageManager.PlayHitEffect(
                this
            );
        }
    }

    /// <summary>
    /// HPが0になったときの処理。
    /// 第1形態なら形態変化、第2形態なら死亡。
    /// </summary>
    private void HandleHpZero()
    {
        if (CanStartSecondPhase())
        {
            StartSecondPhase();
            return;
        }

        Die();
    }

    /// <summary>
    /// 第2形態へ移行可能か。
    /// </summary>
    private bool CanStartSecondPhase()
    {
        if (secondPhaseStarted)
        {
            return false;
        }

        if (phaseChanging)
        {
            return false;
        }

        if (secondPhaseData == null)
        {
            return false;
        }

        if (phaseChanger == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 第2形態への移行演出を開始する。
    /// </summary>
    private void StartSecondPhase()
    {
        secondPhaseStarted = true;
        phaseChanging = true;

        // 演出中はダメージや通常処理を止める
        battleStarted = false;

        Debug.Log(
            "第1形態のHPが0：形態変化開始",
            this
        );

        phaseChanger.StartPhaseChange(
            CompleteSecondPhase
        );
    }

    /// <summary>
    /// 形態変化演出終了後に呼ばれる。
    /// 第2形態のBossDataを適用する。
    /// </summary>
    private void CompleteSecondPhase()
    {
        if (isDead)
        {
            return;
        }

        if (secondPhaseData == null)
        {
            Debug.LogError(
                "第2形態のBossDataが設定されていません。",
                this
            );

            phaseChanging = false;
            Die();
            return;
        }

        currentData =
            secondPhaseData;

        /*
         * 第2形態データを適用し、
         * HPを第2形態の最大HPまで回復する。
         */
        ApplyBossData(
            currentData,
            true
        );

        UpdateHpBar();
        NotifyHpChanged();

        phaseChanging = false;
        battleStarted = true;

        /*
         * 明転直後にBallが重なっていて
         * 即ダメージを受けるのを防ぐ。
         */
        if (damageManager != null)
        {
            StartCoroutine(
                damageManager.StartInvincible(
                    1f
                )
            );
        }

        Debug.Log(
            $"第2形態開始：HP {hp} / {MaxHP}",
            this
        );
    }

    /// <summary>
    /// BossDataのステータスを適用する。
    /// </summary>
    private void ApplyBossData(
        BossData bossData,
        bool recoverHp
    )
    {
        if (bossData == null)
        {
            return;
        }

        if (recoverHp)
        {
            hp = Mathf.Max(
                bossData.maxHP,
                0f
            );
        }

        moveSpeed =
            bossData.moveSpeed;

        moveRangeX =
            bossData.moveRangeX;

        moveRangeY =
            bossData.moveRangeY;
    }

    /// <summary>
    /// HPバーを更新する。
    /// </summary>
    private void UpdateHpBar()
    {
        if (hpFill == null)
        {
            return;
        }

        if (currentData == null ||
            currentData.maxHP <= 0f)
        {
            return;
        }

        float hpPercent =
            Mathf.Clamp01(
                hp / currentData.maxHP
            );

        hpFill.localScale =
            new Vector3(
                hpFillStartScale.x *
                hpPercent,
                hpFillStartScale.y,
                hpFillStartScale.z
            );

        /*
         * 左端を固定して、
         * 右側からHPバーが減るように調整。
         */
        hpFill.localPosition =
            new Vector3(
                hpFillStartPosition.x -
                hpFillStartScale.x *
                (1f - hpPercent) *
                0.5f,
                hpFillStartPosition.y,
                hpFillStartPosition.z
            );
    }

    /// <summary>
    /// BossギミックへHP変化を通知する。
    /// </summary>
    private void NotifyHpChanged()
    {
        if (gimmickRunner == null)
        {
            return;
        }

        if (currentData == null)
        {
            return;
        }

        gimmickRunner.OnBossHpChanged(
            hp,
            currentData.maxHP
        );
    }

    /// <summary>
    /// 同じGameObjectまたは子・親から
    /// 形態変化スクリプトを探す。
    /// </summary>
    private void FindPhaseChanger()
    {
        if (phaseChanger != null)
        {
            return;
        }

        phaseChanger =
            GetComponent<BossPhaseChangerBase>();

        if (phaseChanger == null)
        {
            phaseChanger =
                GetComponentInChildren
                    <BossPhaseChangerBase>(
                        true
                    );
        }

        if (phaseChanger == null)
        {
            phaseChanger =
                GetComponentInParent
                    <BossPhaseChangerBase>();
        }

        /*
         * 形態変化しないBossならnullで正常。
         * secondPhaseDataだけ設定されている場合は警告。
         */
        if (secondPhaseData != null &&
            phaseChanger == null)
        {
            Debug.LogWarning(
                "第2形態のBossDataは設定されていますが、" +
                "BossPhaseChangerBaseが見つかりません。",
                this
            );
        }
    }

    protected virtual void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        battleStarted = false;
        phaseChanging = false;

        StartCoroutine(
            DieCoroutine()
        );
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSecondsRealtime(
            deadWaitTime
        );

        if (clearText != null)
        {
            clearText.SetActive(true);
        }

        Time.timeScale = 0f;

        gameObject.SetActive(false);
    }
}