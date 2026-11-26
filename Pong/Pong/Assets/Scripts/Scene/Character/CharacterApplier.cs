using UnityEngine;

// �I�������L�����N�^�[�\�͂�
// �Q�[�����̃p�h���֔��f����
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
                " ��CharacterDatabase���ݒ肳��Ă��܂���"
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
                " ��CharacterData���擾�ł��܂���ł���"
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
            " �ɃL�������f: " +
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
    // �g�p����L�����ԍ����擾
    // ==================================================

    private int GetCharacterIndex()
    {
        // 1P
        if (playerNumber == 1)
        {
            Debug.Log(
                gameObject.name +
                "：1Pキャラクター番号 = " +
                GameSettings.player1CharacterIndex
            );

            return GameSettings.player1CharacterIndex;
        }

        // 2P
        if (playerNumber == 2)
        {
            // 2人プレイのとき
            if (GameSettings.playerCount >= 2)
            {
                Debug.Log(
                    gameObject.name +
                    "：2Pキャラクター番号 = " +
                    GameSettings.player2CharacterIndex
                );

                return GameSettings.player2CharacterIndex;
            }

            // 1人プレイのCPU
            Debug.Log(
                gameObject.name +
                "：CPUなのでキャラクター番号0を使用"
            );

            return 0;
        }

        Debug.LogWarning(
            gameObject.name +
            "：playerNumberが1か2ではありません。現在値：" +
            playerNumber
        );

        return 0;
    }

    // ==================================================
    // �ړ����x�𔽉f
    // ==================================================

    private void ApplyPaddleStats(CharacterData data)
    {
        // Computer Paddle�ɂ�
        // ComputerPaddle��Player2Paddle�̗������t���Ă��邽��
        // ���ׂĂ�Paddle�n�R���|�[�l���g�ɔ��f����
        Paddle[] paddles = GetComponents<Paddle>();

        if (paddles.Length == 0)
        {
            Debug.LogWarning(
                gameObject.name +
                " ��Paddle�n�R���|�[�l���g������܂���"
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
                " ��Speed�� " +
                data.moveSpeed +
                " �ɐݒ�"
            );
        }
    }

    // ==================================================
    // �p�h���T�C�Y�𔽉f
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
    // �F�𔽉f
    // ==================================================
    private SpriteRenderer targetSpriteRenderer;
    private Color appliedColor;

    private void ApplyColor(CharacterData data)
    {
        appliedColor = data.paddleColor;

        targetSpriteRenderer =
            GetComponent<SpriteRenderer>();

        if (targetSpriteRenderer == null)
        {
            Debug.LogError(
                gameObject.name +
                "：SpriteRendererがありません"
            );

            return;
        }

        targetSpriteRenderer.color = appliedColor;

        Debug.Log(
            gameObject.name +
            "：色を設定しました " +
            targetSpriteRenderer.color
        );
    }

    private void LateUpdate()
    {
        if (targetSpriteRenderer == null)
            return;

        if (targetSpriteRenderer.color != appliedColor)
        {
            Debug.LogWarning(
                gameObject.name +
                "：色が別処理で変更されました。現在色 = " +
                targetSpriteRenderer.color
            );
        }

        targetSpriteRenderer.color = appliedColor;
    }
    // ==================================================
    // HP�𔽉f
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
    // �X�}�b�V���ݒ�𔽉f
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
    // �ʏ픽�ˁE�X�}�b�V���{���𔽉f
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