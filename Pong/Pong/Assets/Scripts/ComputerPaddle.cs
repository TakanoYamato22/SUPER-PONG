using UnityEngine;

// CPU用パドル（プレイヤーの対戦相手）
public class ComputerPaddle : Paddle
{
    [SerializeField]
    private Ball ball; // 追いかける対象（ボール）

    [Header("CPU Difficulty Settings")]

    // 反応速度（0〜1）
    // 1.0 = 完全追従（鬼）
    // 0.3 = 少し遅れる（おすすめ）
    // 0.1 = かなり鈍い
    public float reactionSpeed = 0.01f;

    // 未来予測の強さ（0〜1）
    // 1.0 = 完全に未来位置を読む（プロ）
    // 0.5 = 半分だけ読む（自然）
    // 0.0 = 現在位置だけ見る（初心者CPU）
    public float predictionStrength = 0.0f;

    [Header("Human-like Behavior")]

    // ミスする確率（0〜1）
    // 0.0 = ミスしない
    // 0.1 = 10%でミス（おすすめ）
    public float mistakeChance = 1.0f;

    private void FixedUpdate()
    {
      
        // 🎲 ランダムでミスする（人間っぽさ）
        if (Random.value < mistakeChance)
        {
            // このフレームは何もしない（反応遅れ）
            return;
        }


        // 🟢 ボールがCPU側に向かっているとき

        if (ball.velocity.x > 0f)
        {
            // 現在のボール位置を基準にする
            float predictedY = ball.transform.position.y;


            // 🔮 未来位置予測（難易度に影響）

            if (predictionStrength > 0f)
            {
                // ボールがパドル位置に来るまでの時間を計算
                float timeToReach =
                    (transform.position.x - ball.transform.position.x) / ball.velocity.x;

                // 未来のY座標を予測
                float futureY =
                    ball.transform.position.y + ball.velocity.y * timeToReach;

                // 現在位置と未来位置を補間（predictionStrengthで調整）
                predictedY = Mathf.Lerp(
                    ball.transform.position.y,
                    futureY,
                    predictionStrength
                );
            }


            // 🧠 反応速度を反映

            float targetY = Mathf.Lerp(
                transform.position.y,
                predictedY,
                reactionSpeed
            );

            // その位置に向かって移動
            MoveTowards(targetY);
        }
        else
        {
  
            // 🔵 ボールが離れているとき

            // 中央に戻る（待機）
            MoveTowards(0f);
        }
    }

    // ===============================
    // 🏃 パドル移動処理
    // ===============================
    private void MoveTowards(float targetY)
    {
        // 現在位置 → 目標位置へ少しずつ移動
        float newY = Mathf.MoveTowards(
            transform.position.y,
            targetY,
            speed * Time.fixedDeltaTime
        );

        // 実際に位置を更新
        transform.position = new Vector2(transform.position.x, newY);
    }
}