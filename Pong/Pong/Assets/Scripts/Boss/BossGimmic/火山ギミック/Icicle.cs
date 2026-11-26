
using UnityEngine;

public class Icicle : MonoBehaviour
{
    [Header("—‰ºİ’è")]
    [SerializeField] private float fallSpeed = 4f;
    [SerializeField] private float destroyY = -6f;

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y <= destroyY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball ball = collision.GetComponent<Ball>();

        if (ball == null)
            return;


        Vector2 normal =
            (ball.transform.position - transform.position).normalized;

        // ”½Ë•ûŒü‚ªc‚É•Î‚è‚·‚¬‚È‚¢‚æ‚¤‚É’²®
        float horizontalStrength = 0.6f;

        // Ball‚ª•X’Œ‚Ì¶‰E‚Ç‚¿‚ç‚É‚¢‚é‚©
        float horizontalDirection =
            ball.transform.position.x >= transform.position.x
                ? 1f
                : -1f;

        // ‰¡•ûŒü‚Ì¬•ª‚ğ’Ç‰Á
        normal.x += horizontalDirection * horizontalStrength;
        normal.Normalize();

        ball.ReflectByIcicle(normal);


        // •ûŒü‚ğ”½Ë
        ball.ReflectByIcicle(normal);

        // ‘¬“x‚ğ5Œ¸­iÅ’á15j
        ball.ReduceSpeedByIcicle();

        // •X’Œ‚ğ”j‰ó
        Destroy(gameObject);
    }
}
