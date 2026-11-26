
using System.Collections;
using UnityEngine;
using TMPro;

public class SpaceBossController : BossController
{
    [Header("瞬間移動設定")]
    [SerializeField] private float teleportInterval = 3f;

    [Header("瞬間移動範囲")]
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;

    [Header("スタン設定")]
    [SerializeField] private bool isStunned = false;

    [Header("第1形態・空間反転ギミック")]
    [SerializeField] private SpaceScreenRotator screenRotator;

    [SerializeField, Range(0f, 1f)]
    private float reverseHpRatio = 0.3f;

    [Header("第2形態・最終決戦")]
    [SerializeField, Min(1f)]
    private float secondPhaseTimeLimit = 60f;

    [Header("プレイヤー")]
    [SerializeField] private PlayerHealth[] playerHealths;

    [Header("カウントダウンUI")]
    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private Color normalTimerColor = Color.white;
    [SerializeField] private Color warningTimerColor = Color.red;

    [SerializeField, Min(0f)]
    private float warningTime = 10f;

    [Header("時間切れ・爆発演出")]
    [SerializeField] private ParticleSystem explosionParticle;

    [SerializeField, Min(0.1f)]
    private float explosionDuration = 1.5f;

    [SerializeField] private Vector2 explosionCenter = Vector2.zero;

    // ========================================
    // 内部変数
    // ========================================

    private float stunTimer;
    private float teleportTimer;

    private bool reverseTriggered;

    private float remainingTime;

    private bool countdownStarted;
    private bool countdownFinished;
    private bool timeUpTriggered;

    private bool isExploding;

    private Coroutine explosionCoroutine;

    private float previousTimeScale = 1f;

    public float RemainingTime => remainingTime;

    public bool IsCountdownActive =>
        countdownStarted && !countdownFinished;

    // ========================================
    // 初期化
    // ========================================

    protected override void Start()
    {
        base.Start();

        remainingTime = secondPhaseTimeLimit;

        countdownStarted = false;
        countdownFinished = false;
        timeUpTriggered = false;

        isExploding = false;

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        if (explosionParticle != null)
        {
            explosionParticle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        Debug.Log("宇宙ボス Start 実行！");
        Debug.Log("BattleStarted = " + BattleStarted);
    }

    // ========================================
    // 毎フレーム処理
    // ========================================

    protected override void Update()
    {
        base.Update();

        // 爆発演出中は通常処理を停止
        if (isExploding || timeUpTriggered)
            return;

        // ボス撃破時
        if (IsDead)
        {
            FinishCountdown();
            return;
        }

        // 戦闘前・形態変化中は停止
        if (!BattleStarted || IsPhaseChanging)
            return;

        // 第2形態開始時にタイマー起動
        if (IsSecondPhase &&
            !countdownStarted &&
            !countdownFinished)
        {
            StartCountdown();
        }

        if (IsCountdownActive)
        {
            UpdateCountdown();
        }
    }

    // ========================================
    // 最終決戦開始
    // ========================================

    private void StartCountdown()
    {
        countdownStarted = true;
        countdownFinished = false;
        timeUpTriggered = false;

        remainingTime = secondPhaseTimeLimit;

        // プレイヤーを無敵化＆全回復
        SetPlayersInvincible(true);

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        UpdateCountdownText();

        Debug.Log(
            $"宇宙ボス：最終決戦開始！制限時間{secondPhaseTimeLimit}秒"
        );
    }

    // ========================================
    // カウントダウン更新
    // ========================================

    private void UpdateCountdown()
    {
        if (IsDead)
        {
            FinishCountdown();
            return;
        }

        if (AreAllPlayersDead())
        {
            StopCountdown();
            return;
        }

        // ボールの再発射待機中も時間は進む
        remainingTime -= Time.deltaTime;

        remainingTime = Mathf.Max(
            remainingTime,
            0f
        );

        UpdateCountdownText();

        if (remainingTime <= 0f)
        {
            TimeUp();
        }
    }

    // ========================================
    // タイマー表示
    // ========================================

    private void UpdateCountdownText()
    {
        if (countdownText == null)
            return;

        int seconds = Mathf.CeilToInt(remainingTime);

        countdownText.text = seconds.ToString();

        countdownText.color =
            remainingTime <= warningTime
                ? warningTimerColor
                : normalTimerColor;
    }

    // ========================================
    // 時間切れ
    // ========================================

    private void TimeUp()
    {
        if (countdownFinished ||
            timeUpTriggered ||
            IsDead)
        {
            return;
        }

        timeUpTriggered = true;
        countdownFinished = true;
        countdownStarted = false;

        Debug.Log("宇宙ボス：時間切れ！爆発演出開始！");

        explosionCoroutine = StartCoroutine(
            TimeUpExplosionRoutine()
        );
    }

    // ========================================
    // 時間切れ・爆発演出
    // ========================================

    private IEnumerator TimeUpExplosionRoutine()
    {
        isExploding = true;

        // スタンを解除
        isStunned = false;
        stunTimer = 0f;

        // 瞬間移動タイマーを停止
        teleportTimer = 0f;

        // ========================================
        // 1. ボスを中央へテレポート
        // ========================================

        transform.position = new Vector3(
            explosionCenter.x,
            explosionCenter.y,
            transform.position.z
        );

        Debug.Log("宇宙ボス：中央へ瞬間移動！");

        // 位置変更を画面に反映
        yield return null;

        // ========================================
        // 2. ゲーム内の時間を停止
        // ========================================

        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;


        // ========================================
        // 3. 爆発エフェクト再生
        // ========================================

        if (explosionParticle != null)
        {
            // 爆発をボスの中央に配置
            explosionParticle.transform.position =
                transform.position;

            // 時間停止中もParticleを動かす
            var main = explosionParticle.main;
            main.useUnscaledTime = true;

            // Particleを有効化
            if (!explosionParticle.gameObject.activeSelf)
            {
                explosionParticle.gameObject.SetActive(true);
            }

            // Particleを初期化
            explosionParticle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );

            explosionParticle.Clear(true);

            // 爆発を再生
            explosionParticle.Play(true);

            Debug.Log(
                "爆発Particle再生状態：" +
                explosionParticle.isPlaying
            );
        }
        else
        {
            Debug.LogWarning(
                "SpaceExplosionParticleが設定されていません！"
            );
        }


        // ========================================
        // 4. 爆発が終わるまで待機
        // ========================================

        yield return new WaitForSecondsRealtime(
            explosionDuration
        );

        // ========================================
        // 5. ゲームオーバー
        // ========================================

        Time.timeScale = previousTimeScale;

        SetPlayersInvincible(false);

        HideCountdown();

        Debug.Log("宇宙ボス：爆発による強制敗北！");

        if (playerHealths != null)
        {
            foreach (PlayerHealth player in playerHealths)
            {
                if (player != null)
                {
                    player.ForceGameOver();
                }
            }
        }

        isExploding = false;
        explosionCoroutine = null;
    }

    // ========================================
    // ボス撃破
    // ========================================

    private void FinishCountdown()
    {
        if (!countdownStarted || countdownFinished)
            return;

        countdownFinished = true;
        countdownStarted = false;

        SetPlayersInvincible(false);
        HideCountdown();

        Debug.Log("宇宙ボス：制限時間以内に撃破成功！");
    }

    // ========================================
    // タイマー停止
    // ========================================

    private void StopCountdown()
    {
        countdownFinished = true;
        countdownStarted = false;

        SetPlayersInvincible(false);
        HideCountdown();
    }

    // ========================================
    // プレイヤー無敵切り替え
    // ========================================

    private void SetPlayersInvincible(bool invincible)
    {
        if (playerHealths == null)
            return;

        foreach (PlayerHealth player in playerHealths)
        {
            if (player != null)
            {
                player.SetDamageDisabled(invincible);
            }
        }
    }

    // ========================================
    // プレイヤーの敗北確認
    // ========================================

    private bool AreAllPlayersDead()
    {
        if (playerHealths == null ||
            playerHealths.Length == 0)
        {
            return false;
        }

        bool foundPlayer = false;

        foreach (PlayerHealth player in playerHealths)
        {
            if (player == null)
                continue;

            foundPlayer = true;

            if (!player.IsDead)
                return false;
        }

        return foundPlayer;
    }

    // ========================================
    // UIを非表示
    // ========================================

    private void HideCountdown()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    // ========================================
    // スタン処理
    // ========================================

    private void FixedUpdate()
    {
        if (!isStunned || isExploding)
            return;

        stunTimer -= Time.fixedDeltaTime;

        if (stunTimer <= 0f)
        {
            isStunned = false;
            stunTimer = 0f;

            Debug.Log("宇宙ボスのスタン解除！");
        }
    }

    public void Stun(float duration)
    {
        // 爆発中はスタンを受け付けない
        if (isExploding || timeUpTriggered)
            return;

        isStunned = true;
        stunTimer = duration;

        Debug.Log(
            $"宇宙ボスが{duration}秒スタン！"
        );
    }

    // ========================================
    // ダメージ・第1形態限定の画面反転
    // ========================================

    public override void TakeDamage(float damage)
    {
        // 爆発中はボスにダメージを与えない
        if (isExploding || timeUpTriggered)
            return;

        // 既存のダメージ・形態変化処理
        base.TakeDamage(damage);

        // 第2形態では画面回転しない
        if (IsSecondPhase || IsPhaseChanging)
            return;

        if (reverseTriggered ||
            IsDead ||
            !BattleStarted)
        {
            return;
        }

        if (MaxHP <= 0f)
            return;

        // 第1形態のHPが30%以下で画面反転
        if (CurrentHP / MaxHP <= reverseHpRatio)
        {
            reverseTriggered = true;

            if (screenRotator != null)
            {
                screenRotator.StartRotation();
            }
            else
            {
                Debug.LogWarning(
                    "SpaceScreenRotatorが設定されていません！"
                );
            }
        }
    }

    // ========================================
    // 瞬間移動（両形態で有効）
    // ========================================

    protected override void Move()
    {
        // 爆発中・時間切れ後は移動しない
        if (isExploding || timeUpTriggered)
            return;

        // スタン中は移動しない
        if (isStunned)
            return;

        teleportTimer += Time.deltaTime;

        if (teleportTimer < teleportInterval)
            return;

        teleportTimer = 0f;

        Teleport();
    }

    // ========================================
    // ランダム瞬間移動
    // ========================================

    private void Teleport()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        transform.position = new Vector3(
            randomX,
            randomY,
            transform.position.z
        );

        Debug.Log("宇宙ボス瞬間移動！");
    }

    // ========================================
    // 無効化時の後片付け
    // ========================================

    private void OnDisable()
    {
        if (explosionCoroutine != null)
        {
            StopCoroutine(explosionCoroutine);
            explosionCoroutine = null;
        }

        if (isExploding)
        {
            Time.timeScale = previousTimeScale;
            isExploding = false;
        }

        if (countdownStarted && !countdownFinished)
        {
            StopCountdown();
        }
    }
}
