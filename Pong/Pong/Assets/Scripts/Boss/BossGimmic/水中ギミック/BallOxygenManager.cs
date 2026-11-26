
using UnityEngine;

[RequireComponent(typeof(Ball))]
public class BallOxygenManager : MonoBehaviour
{
    [Header("酸素ゲージ")]
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float oxygenIncreasePerSecond = 2f;

    [Header("酸素不足ダメージ")]
    [SerializeField] private float damageInterval = 5f;
    [SerializeField] private int oxygenDamage = 10;

    [Header("ダメージ対象")]
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;

    private float currentOxygen;
    private float damageTimer;
    private bool oxygenEnabled;

    public float CurrentOxygen => currentOxygen;
    public float MaxOxygen => maxOxygen;

    public float OxygenRatio =>
        maxOxygen > 0f
            ? currentOxygen / maxOxygen
            : 0f;

    public bool IsOxygenMax =>
        currentOxygen >= maxOxygen;

    public bool OxygenEnabled => oxygenEnabled;

    private void Update()
    {
        if (!oxygenEnabled)
            return;

        // 時間経過でゲージ蓄積
        currentOxygen = Mathf.Min(
            maxOxygen,
            currentOxygen +
            oxygenIncreasePerSecond * Time.deltaTime
        );

        // MAXになったら継続ダメージ
        if (IsOxygenMax)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;
                ApplyOxygenDamage();
            }
        }
        else
        {
            damageTimer = 0f;
        }
    }

    private void ApplyOxygenDamage()
    {
        // 1P
        if (player1Health != null)
        {
            player1Health.TakeDamage(oxygenDamage);
        }

        // 2P
        if (player2Health != null &&
            player2Health != player1Health)
        {
            player2Health.TakeDamage(oxygenDamage);
        }

        Debug.Log(
            $"酸素不足！各プレイヤーに{oxygenDamage}ダメージ"
        );
    }

    // 第2形態開始時
    public void StartOxygenGimmick()
    {
        currentOxygen = 0f;
        damageTimer = 0f;
        oxygenEnabled = true;

        Debug.Log("酸素ギミック開始！");
    }

    // 戦闘終了時
    public void StopOxygenGimmick()
    {
        oxygenEnabled = false;
        damageTimer = 0f;
    }

    // 酸素アイテム取得
    public void RecoverOxygen(float ratio)
    {
        if (!oxygenEnabled)
            return;

        currentOxygen = Mathf.Max(
            0f,
            currentOxygen -
            maxOxygen * Mathf.Clamp01(ratio)
        );

        damageTimer = 0f;

        Debug.Log(
            $"酸素回復！現在：{currentOxygen}/{maxOxygen}"
        );
    }

    private void OnDisable()
    {
        StopOxygenGimmick();
    }
}
