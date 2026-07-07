using UnityEngine;

// 選択したキャラクターの能力をパドルに反映
public class CharacterApplier : MonoBehaviour
{
    [SerializeField] private CharacterDatabase database;
    [SerializeField] private Paddle targetPaddle;

    private void Start()
    {
        CharacterData data = database.GetCharacter(GameSettings.player1CharacterIndex);

        if (data == null)
            return;

        // 移動速度を反映
        targetPaddle.speed = data.moveSpeed;

        // パドルの縦サイズを反映
        Vector3 scale = targetPaddle.transform.localScale;
        scale.y = data.paddleHeight;
        targetPaddle.transform.localScale = scale;

        // 将来的にスマッシュ性能も反映可能
        // targetPaddle.smashPower = data.smashPower;
        // targetPaddle.smashDashDistance = data.smashDashDistance;

        Debug.Log("キャラ能力適用: " + data.characterName);
    }
}