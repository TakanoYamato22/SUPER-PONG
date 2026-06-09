using UnityEngine;

[CreateAssetMenu(menuName = "BossBallGimmick/Hp70SpawnGimmickBalls")]
public class Hp70SpawnGimmickBalls : BossBallGimmick
{
    public GameObject gimmickBallPrefab;
    public int spawnCount = 2;
    public float spreadAngle = 45f;
    public float spawnSpeed = 6f;

    private bool triggered = false;

    private void OnEnable()
    {
        triggered = false;
    }

    public override void OnBossHpChanged(Ball ball, int currentHp, int maxHp)
    {
        if (triggered) return;

        float ratio = (float)currentHp / maxHp;

        if (ratio <= 0.7f)
        {
            triggered = true;
            SpawnGimmickBalls(ball);
        }
    }

    private void SpawnGimmickBalls(Ball baseBall)
    {
        Debug.Log("GimmickBall 追加生成！");

        Vector2 baseDir = Vector2.right;

        Rigidbody2D baseRb = baseBall.GetComponent<Rigidbody2D>();
        if (baseRb != null && baseRb.linearVelocity.sqrMagnitude > 0.01f)
        {
            baseDir = baseRb.linearVelocity.normalized;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newBall = Instantiate(
                gimmickBallPrefab,
                baseBall.transform.position,
                Quaternion.identity
            );

            Rigidbody2D newRb = newBall.GetComponent<Rigidbody2D>();

            float t = spawnCount == 1 ? 0.5f : (float)i / (spawnCount - 1);
            float angleOffset = spreadAngle * (t - 0.5f);

            Vector2 dir = Quaternion.Euler(0, 0, angleOffset) * baseDir;

            if (newRb != null)
            {
                newRb.bodyType = RigidbodyType2D.Dynamic;
                newRb.gravityScale = 0f;
                newRb.linearVelocity = Vector2.zero;
                newRb.AddForce(dir * spawnSpeed, ForceMode2D.Impulse);
            }
        }
    }

    public override void OnHitBoss(Ball ball) { }
    public override void OnHitWall(Ball ball) { }
    public override void OnUpdate(Ball ball) { }
}