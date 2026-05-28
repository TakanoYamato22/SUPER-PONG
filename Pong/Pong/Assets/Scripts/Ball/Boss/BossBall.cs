using UnityEngine;

public class BossBall : Ball
{
    public float speedReduceRate = 0.8f;

    protected override void Start()
    {
        base.Start();

        // Ball の AddStartingForce を使う
        AddStartingForce();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BossController boss))
        {
            // ダメージは Ball の currentSpeed を使う
            float damage = currentSpeed;
            boss.TakeDamage(damage);

            // 貫通後に減速（Ball の SetSpeed を使う）
            SetSpeed(currentSpeed * speedReduceRate);
        }
    }
}
