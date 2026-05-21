using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float moveRangeX = 7f;
    public float moveRangeY = 4f;
    public float hp = 100f;

    private Vector3 targetPos;

    void Start()
    {
        SetNewTarget();
    }

    void Update()
    {
        Move();
    }

    void SetNewTarget()
    {
        targetPos = new Vector3(
            Random.Range(-moveRangeX, moveRangeX),
            Random.Range(-moveRangeY, moveRangeY),
            0
        );
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            SetNewTarget();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            float damage = rb.linearVelocity.magnitude;

            hp -= damage;

            Debug.Log("Boss HP: " + hp);

            if (hp <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("Boss Defeated!");
        Destroy(gameObject);
    }
}
