using UnityEngine;

public class GiusController : BossController
{
    protected override void Move()
    {
        float t = Time.time * moveSpeed;

        float x = Mathf.PerlinNoise(t, 0f) * 2f - 1f;
        float y = Mathf.PerlinNoise(0f, t) * 2f - 1f;

        x += (Mathf.PerlinNoise(t * 0.5f, t * 1.3f) - 0.5f) * 0.8f;
        y += (Mathf.PerlinNoise(t * 1.7f, t * 0.4f) - 0.5f) * 0.8f;

        x = Mathf.Clamp(x * moveRangeX, -moveRangeX, moveRangeX);
        y = Mathf.Clamp(y * moveRangeY, -moveRangeY, moveRangeY);

        transform.position = new Vector3(x, y, 0);
    }

    protected override void Die()
    {
        Debug.Log("Gius 撃破！");
        base.Die();
    }
}
