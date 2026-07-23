using UnityEngine;

public class Drone : MonoBehaviour
{
    [Header("移動")]
    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float areaX = 8f;

    [SerializeField]
    private float areaY = 8f;

    [SerializeField]
    private float changeTargetDistance = 0.2f;

    [Header("破壊演出")]
    [SerializeField]
    private AudioClip itemSound;

    [SerializeField]
    private GameObject hitEffectPrefab;

    private Vector2 targetPosition;
    private bool isBroken;

    private void Start()
    {
        SetRandomTarget();
    }

    private void Update()
    {
        if (isBroken)
        {
            return;
        }

        transform.position =
            Vector2.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

        float distance =
            Vector2.Distance(
                transform.position,
                targetPosition
            );

        if (distance <= changeTargetDistance)
        {
            SetRandomTarget();
        }
    }

    private void SetRandomTarget()
    {
        targetPosition =
            new Vector2(
                Random.Range(-areaX, areaX),
                Random.Range(-areaY, areaY)
            );
    }

    /*
     * 通常Ballとの物理衝突。
     * Ballは反射し、Droneは壊れる。
     */
    private void OnCollisionEnter2D(
        Collision2D collision
    )
    {
        Ball ball =
            collision.gameObject
                .GetComponentInParent<Ball>();

        if (ball == null)
        {
            return;
        }

        BreakDrone();
    }

    /*
     * 通常衝突とスマッシュTriggerの両方から呼ばれる。
     */
    public void BreakDrone()
    {
        if (isBroken)
        {
            return;
        }

        isBroken = true;

        Collider2D[] colliders =
            GetComponentsInChildren<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        if (hitEffectPrefab != null)
        {
            Instantiate(
                hitEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        if (itemSound != null)
        {
            AudioSource.PlayClipAtPoint(
                itemSound,
                transform.position
            );
        }

        Destroy(gameObject);
    }
}