
using UnityEngine;

public class ForestBossController : BossController
{
    [Header("森ステージ設定")]
    [SerializeField]
    private ForestSpirit[] spirits = new ForestSpirit[4];

    private bool initialized;

    protected override void Start()
    {
        base.Start();

        if (data == null)
        {
            return;
        }

        if (spirits == null || spirits.Length != 4)
        {
            Debug.LogError(
                "精霊を4体登録してください。",
                this
            );

            return;
        }

        float totalMaxHP = 0f;

        foreach (ForestSpirit spirit in spirits)
        {
            if (spirit == null)
            {
                Debug.LogError(
                    "精霊が登録されていません。",
                    this
                );

                return;
            }

            totalMaxHP += spirit.MaxHP;
        }

        if (!Mathf.Approximately(totalMaxHP, MaxHP))
        {
            Debug.LogError(
                $"BossDataの最大HP({MaxHP})と" +
                $"精霊のHP合計({totalMaxHP})が一致しません。",
                this
            );

            return;
        }

        initialized = true;

        SyncSpiritHP();

        Debug.Log(
            $"森ステージ初期化完了！HP：{CurrentHP} / {MaxHP}",
            this
        );
    }

    // ========================================
    // 古代樹への直接攻撃は無効
    // ========================================

    public override void TakeDamage(float damage)
    {
        // 何もしない
    }

    // ========================================
    // 精霊からのダメージ通知
    // ========================================

    public void OnSpiritDamaged(
        ForestSpirit spirit,
        float damage
    )
    {
        if (!initialized ||
            !BattleStarted ||
            IsDead ||
            IsPhaseChanging)
        {
            return;
        }

        bool registered = false;

        foreach (ForestSpirit member in spirits)
        {
            if (member == spirit)
            {
                registered = true;
                break;
            }
        }

        if (!registered)
        {
            return;
        }

        // 精霊の残りHPを合計
        SyncSpiritHP();

        Debug.Log(
            $"古代樹 HP：{CurrentHP} / {MaxHP}",
            this
        );
    }

    // ========================================
    // 精霊4体のHPを合計
    // ========================================

    private void SyncSpiritHP()
    {
        float totalHP = 0f;

        foreach (ForestSpirit spirit in spirits)
        {
            if (spirit == null)
            {
                continue;
            }

            totalHP += spirit.CurrentHP;
        }

        // 古代樹HPを精霊の合計HPに同期
        SetBossHP(totalHP);
    }
}
