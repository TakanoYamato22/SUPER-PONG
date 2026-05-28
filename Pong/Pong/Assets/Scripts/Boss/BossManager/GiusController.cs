using UnityEngine;

public class GiusController : BossController
{
    protected override void Start()
    {
        base.Start();
        // Giusê—p‚Ì‰Šú‰»‚ª‚ ‚ê‚Î‚±‚±‚É‘‚­
    }

    protected override void Move()
    {
        float x = Mathf.Sin(Time.time * moveSpeed * 2f) * moveRangeX;
        float y = Mathf.Cos(Time.time * moveSpeed * 0.5f) * moveRangeY;
        transform.position = new Vector3(x, y, 0);
    }

    protected override void Die()
    {
        Debug.Log("Gius Œ‚”jI");
        base.Die();
    }
}
