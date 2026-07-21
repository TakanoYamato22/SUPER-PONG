using UnityEngine;

public class Comet : MonoBehaviour
{
    [Header("移動方向")]
    [Tooltip("左下なら X=-1、Y=-1")]
    [SerializeField]
    private Vector2 direction =
        new Vector2(-1f, -1f);

    [Header("移動速度")]
    [SerializeField] private float speed = 8f;

    [Header("削除するまでの移動距離")]
    [SerializeField] private float destroyDistance = 20f;

    private Vector3 startPosition;
    private Vector2 moveDirection;

    private void Start()
    {
        startPosition = transform.position;

        if (direction.sqrMagnitude <= 0.001f)
        {
            Debug.LogWarning(
                $"{gameObject.name}のDirectionが0です。左下方向に設定します。"
            );

            direction = new Vector2(
                -1f,
                -1f
            );
        }

        moveDirection = direction.normalized;
    }

    private void Update()
    {
        transform.position +=
            (Vector3)(
                moveDirection *
                speed *
                Time.deltaTime
            );

        float movedDistance =
            Vector3.Distance(
                startPosition,
                transform.position
            );

        if (movedDistance >= destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}