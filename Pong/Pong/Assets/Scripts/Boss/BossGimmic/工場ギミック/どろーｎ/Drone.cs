using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float areaX = 8f;
    [SerializeField] private float areaY = 8f;
    [SerializeField] private float changeTargetDistance = 0.2f;

    // ★追加: 発生させたいエフェクトのプレハブをインスペクターから登録できるようにします
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
        float x = Random.Range(-areaX, areaX);
        float y = Random.Range(-areaY, areaY);

        targetPos = new Vector2(x, y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // ★追加: ぶつかった瞬間、エフェクトをドローンと同じ位置・同じ回転で生成する
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, transform.rotation);
            }

            // ドローンを破壊
            Destroy(gameObject);
        }
    }
}