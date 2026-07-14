using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float areaX = 8f;
    [SerializeField] private float areaY = 8f;
    [SerializeField] private float changeTargetDistance = 0.2f;
    [SerializeField] private AudioClip itemSound;
    [SerializeField] private GameObject hitEffectPrefab;

    private Vector2 targetPos;

    private void Start()
    {
        SetRandomTarget();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPos) <= changeTargetDistance)
        {
            SetRandomTarget();
        }
    }

    private void SetRandomTarget()
    {
        targetPos = new Vector2(
            Random.Range(-areaX, areaX),
            Random.Range(-areaY, areaY)
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        BallSmashManager smash =
            collision.gameObject.GetComponent<BallSmashManager>();

        // スマッシュ中なら、この衝突だけ無視して貫通させる
        if (smash != null && smash.IsSmashed)
        {
            Collider2D droneCol = GetComponent<Collider2D>();
            Collider2D ballCol = collision.collider;

            if (droneCol != null && ballCol != null)
            {
                Physics2D.IgnoreCollision(ballCol, droneCol, true);
            }
        }

        BreakDrone();
    }

    public void BreakDrone()
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        }

        if (itemSound != null)
        {
            AudioSource.PlayClipAtPoint(itemSound, transform.position);
        }

        Destroy(gameObject);
    }
}