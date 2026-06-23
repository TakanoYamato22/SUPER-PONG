using UnityEngine;

[CreateAssetMenu(menuName = "BossBallGimmick/Hp70SpawnGimmickBalls")]
public class Hp70SpawnGimmickBalls : BossBallGimmick
{
    public GameObject gimmickBallPrefab;
    public float spawnSpeed = 6f;
    public float spawnOffset = 0.5f;

    [Header("Repeat")]
    public float repeatInterval = 10f;

    private bool activated = false;
    private float timer = 0f;

    private void OnEnable()
    {
        activated = false;
        timer = 0f;
    }

    public override void OnBossHpChanged(Ball ball, int currentHp, int maxHp)
    {
        if (activated) return;

        float ratio = (float)currentHp / maxHp;

        if (ratio <= 0.7f)
        {
            activated = true;
            timer = 0f;

            SpawnGimmickBalls(ball);
        }
    }

    public override void OnUpdate(Ball ball)
    {
        if (!activated) return;

        timer += Time.deltaTime;

        if (timer >= repeatInterval)
        {
            timer = 0f;
            SpawnGimmickBalls(ball);
        }
    }

    private void SpawnGimmickBalls(Ball baseBall)
    {
        Debug.Log("SplitBall 定期発動！");

        Rigidbody2D baseRb = baseBall.GetComponent<Rigidbody2D>();
        Collider2D baseCol = baseBall.GetComponent<Collider2D>();

        Vector2 forwardDir = Vector2.right;

        if (baseRb != null && baseRb.linearVelocity.sqrMagnitude > 0.01f)
        {
            forwardDir = baseRb.linearVelocity.normalized;
        }
        else
        {
            forwardDir = Random.value < 0.5f ? Vector2.right : Vector2.left;
        }

        Vector2 backDir = -forwardDir;

        SpawnOneBall(baseBall, baseCol, forwardDir);
        SpawnOneBall(baseBall, baseCol, backDir);
    }

    private void SpawnOneBall(Ball baseBall, Collider2D baseCol, Vector2 dir)
    {
        Vector2 spawnPos =
            (Vector2)baseBall.transform.position + dir * spawnOffset;

        GameObject newBall = Instantiate(
            gimmickBallPrefab,
            spawnPos,
            Quaternion.identity
        );

        Rigidbody2D newRb = newBall.GetComponent<Rigidbody2D>();
        Collider2D newCol = newBall.GetComponent<Collider2D>();

        if (baseCol != null && newCol != null)
        {
            Physics2D.IgnoreCollision(baseCol, newCol);
        }

        if (newRb != null)
        {
            newRb.bodyType = RigidbodyType2D.Dynamic;
            newRb.gravityScale = 0f;
            newRb.linearVelocity = dir.normalized * spawnSpeed;
        }
    }

    public override void OnHitBoss(Ball ball) { }
    public override void OnHitWall(Ball ball) { }
}