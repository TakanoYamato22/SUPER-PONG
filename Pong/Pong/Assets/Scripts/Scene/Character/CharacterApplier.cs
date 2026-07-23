using UnityEngine;

// 選択したキャラクター能力を
// ゲーム中のパドルへ反映する
public class CharacterApplier : MonoBehaviour
{
    [Header("Player Number")]
    [SerializeField] private int playerNumber = 1;
    // 1 = Player Paddle
    // 2 = Computer Paddle / Player2 Paddle

    [Header("Database")]
    [SerializeField] private CharacterDatabase characterDatabase;

    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    private void Start()
    {
        if (characterDatabase == null)
        {
            characterDatabase =
                FindFirstObjectByType<CharacterDatabase>();
        }

        ApplyCharacter();
    }

    private void ApplyCharacter()
    {
        if (characterDatabase == null)
        {
            Debug.LogError(
                gameObject.name +
                " にCharacterDatabaseが設定されていません"
            );

            return;
        }

        int characterIndex = GetCharacterIndex();

        CharacterData data =
            characterDatabase.GetCharacter(characterIndex);

        if (data == null)
        {
            Debug.LogError(
                gameObject.name +
                " のCharacterDataを取得できませんでした"
            );

            return;
        }

        ApplyPaddleStats(data);
        ApplyPaddleSize(data);
        ApplyColor(data);
        ApplyHealth(data);
        ApplySmashSettings(data);
        ApplyRuntimeStats(data);

        Debug.Log(
            gameObject.name +
            " にキャラ反映: " +
            data.characterName
        );

        Debug.Log(
            gameObject.name +
            " / Player Number = " +
            playerNumber +
            " / Character Index = " +
            characterIndex
        );
    }

    // ==================================================
    // 使用するキャラ番号を取得
    // ==================================================

    private int GetCharacterIndex()
    {
        if (playerNumber == 1)
        {
            return GameSettings.player1CharacterIndex;
        }

        // 1Pモード時のCPUはBalanced固定
        if (GameSettings.playerCount == 1)
        {
            return 0;
        }

        // 2Pモード時は2Pが選んだキャラ
        return GameSettings.player2CharacterIndex;
    }

    // ==================================================
    // 移動速度を反映
    // ==================================================

    private void ApplyPaddleStats(CharacterData data)
    {
        // Computer Paddleには
        // ComputerPaddleとPlayer2Paddleの両方が付いているため
        // すべてのPaddle系コンポーネントに反映する
        Paddle[] paddles = GetComponents<Paddle>();

        if (paddles.Length == 0)
        {
            Debug.LogWarning(
                gameObject.name +
                " にPaddle系コンポーネントがありません"
            );

            return;
        }

        foreach (Paddle paddle in paddles)
        {
            paddle.speed = data.moveSpeed;

            Debug.Log(
                gameObject.name +
                " / " +
                paddle.GetType().Name +
                " のSpeedを " +
                data.moveSpeed +
                " に設定"
            );
        }
    }

    // ==================================================
    // パドルサイズを反映
    // ==================================================

    private void ApplyPaddleSize(CharacterData data)
    {
        Vector3 newScale = initialScale;

        newScale.y =
            initialScale.y *
            data.paddleHeightMultiplier;

        transform.localScale = newScale;
    }

    // ==================================================
    // 色を反映
    // ==================================================

    private void ApplyColor(CharacterData data)
    {
        SpriteRenderer[] spriteRenderers =
            GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = data.paddleColor;
        }
    }

    // ==================================================
    // HPを反映
    // ==================================================

    private void ApplyHealth(CharacterData data)
    {
        PlayerHealth health =
            GetComponent<PlayerHealth>();

        if (health == null)
            return;

        health.SetMaxHP(
            data.maxHP,
            true
        );
    }

    // ==================================================
    // スマッシュ設定を反映
    // ==================================================

    private void ApplySmashSettings(CharacterData data)
    {
        SmashController smashController =
            GetComponent<SmashController>();

        if (smashController == null)
            return;

        smashController.ApplyCharacterSettings(
            data.smashCooldown,
            data.smashMoveDistance,
            data.smashMoveSpeed
        );
    }

    // ==================================================
    // 通常反射・スマッシュ倍率を反映
    // ==================================================

    private void ApplyRuntimeStats(CharacterData data)
    {
        CharacterRuntimeStats stats =
            GetComponent<CharacterRuntimeStats>();

        if (stats == null)
        {
            stats =
                gameObject.AddComponent<CharacterRuntimeStats>();
        }

        stats.ApplyCharacterData(data);
    }
}