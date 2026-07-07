using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BouncySurface : MonoBehaviour
{
    public enum ForceType
    {
        Additive,
        Multiplicative,
    }

    public ForceType forceType = ForceType.Additive;
    public float bounceStrength = 0f;

    [SerializeField] private PlayerSmashController playerSmashController;

    // 🌟 GitHubの既存コードを壊さないよう、足りない変数だけをここに追記
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private ParticleSystem smashEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {


            Vector2 hitPoint = collision.contacts[0].point;

            // ★追加：スペースキーが押されているかチェック（テスト用）
            bool isTestSmashPressed = Input.GetKey(KeyCode.Space);

            // スペースキーが押されているか、元々の bounceStrength が 2 以上ならスマッシュと判定
            if (isTestSmashPressed || bounceStrength >= 2f)
            {
                // スマッシュ時の加速処理（本来のスマッシュ用の強さ、例えば 5f などで強制加速）
                ball.IncreaseSpeed(5f);

                // スマッシュエフェクトを再生
                if (smashEffect != null)
                {
                    smashEffect.transform.position = hitPoint;
                    if (smashEffect.isPlaying) smashEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    smashEffect.Play();
                    Debug.Log("<color=red><b>[テスト] 強制スマッシュエフェクト再生！</b></color>");
                }
            }
            else
            {
                // 通常時の加速処理（インスペクターで設定された元の数値）
                switch (forceType)
                {
                    case ForceType.Additive:
                        ball.IncreaseSpeed(bounceStrength);
                        break;
                    case ForceType.Multiplicative:
                        float multiplier = bounceStrength;
                        float addAmount = ball.currentSpeed * (multiplier - 1f);
                        ball.IncreaseSpeed(addAmount);
                        break;
                }

                // 通常ヒットエフェクトを再生
                if (hitEffect != null)
                {
                    hitEffect.transform.position = hitPoint;
                    if (hitEffect.isPlaying) hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    hitEffect.Play();
                }
            }

        }

        BallSmashManager smashManager = ball.GetComponent<BallSmashManager>();

        Vector3 hitPoint = collision.contacts.Length > 0
            ? collision.contacts[0].point
            : transform.position;

        if (smashController != null && smashController.CanSmashNow())
        {
            if (smashManager != null && smashManager.IsSmashed)

            if (playerSmashController != null && playerSmashController.CanSmashNow())
 parent of 50accef (Merge branch 'main' into micchi-)
            {
                playerSmashController.DoSmash();
                return;
            }

            // 🌟 元の switch 文をそのまま残すため、ここで一旦 `case ForceType.Additive` を処理
            switch (forceType)
            {
                case ForceType.Additive:
                    ball.IncreaseSpeed(bounceStrength);
                    break; // ※元は return でしたが、下の追加コードを実行するために break に調整するか、このまま残します
            }

            // 🌟 追加部分で使われている「衝突位置」をここで取得
            Vector3 hitPoint = collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position;

            // ★追加：スペースキーが押されているかチェック（テスト用）
            bool isTestSmashPressed = Input.GetKey(KeyCode.Space);

            // スペースキーが押されているか、元々の bounceStrength が 2 以上ならスマッシュと判定
            if (isTestSmashPressed || bounceStrength >= 2f)
            {
                // スマッシュ時の加速処理（本来のスマッシュ用の強さ、例えば 5f などで強制加速）
                ball.IncreaseSpeed(5f);

                // スマッシュエフェクトを再生
                if (smashEffect != null)
                {
                    smashEffect.transform.position = hitPoint;
                    if (smashEffect.isPlaying) smashEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    smashEffect.Play();
                    Debug.Log("<color=red><b>[テスト] 強制スマッシュエフェクト再生！</b></color>");
                }
            }
            else
            {
                // 通常時の加速処理（インスペクターで設定された元の数値）
                switch (forceType)
                {
                    case ForceType.Additive:
                        ball.IncreaseSpeed(bounceStrength);
                        break;
                    case ForceType.Multiplicative:
                        float multiplier = bounceStrength;
                        float addAmount = ball.currentSpeed * (multiplier - 1f);
                        ball.IncreaseSpeed(addAmount);
                        break;
                }

                // 通常ヒットエフェクトを再生
                if (hitEffect != null)
                {
                    hitEffect.transform.position = hitPoint;
                    if (hitEffect.isPlaying) hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    hitEffect.Play();
                }
            }
        }
    }
}