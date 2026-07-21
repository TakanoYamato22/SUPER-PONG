using UnityEngine;

// 選択したキャラクター能力をパドルに反映する
public class CharacterApplier : MonoBehaviour
{
    [Header("Player Number")]
    [SerializeField] private int playerNumber = 1;
    // 1 = Player Paddle
    // 2 = Computer Paddle / Player2 Paddle

    [Header("Database")]
    [SerializeField] private CharacterDatabase characterDatabase;

    private void Start()
    {
        if (characterDatabase == null)
        {
            characterDatabase = FindFirstObjectByType<CharacterDatabase>();
        }

        ApplyCharacter();
    }

    private void ApplyCharacter()
    {
        int characterIndex;

        if (playerNumber == 1)
        {
            characterIndex = GameSettings.player1CharacterIndex;
        }
        else
        {
            characterIndex = GameSettings.player2CharacterIndex;
        }

        CharacterData data = characterDatabase.GetCharacter(characterIndex);

        if (data == null)
            return;

        Paddle paddle = GetComponent<Paddle>();

        if (paddle != null)
        {
            paddle.speed = data.moveSpeed;
            paddle.smashPower = data.smashPower;
            //paddle.smashDashDistance = data.smashDashDistance;
        }

        // パドルの縦サイズ変更
        Vector3 scale = transform.localScale;
        scale.y = data.paddleHeight;
        transform.localScale = scale;

        // 色変更
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.color = data.paddleColor;
        }

        Debug.Log(name + " にキャラ反映: " + data.characterName);
    }
}