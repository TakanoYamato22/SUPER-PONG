
using System.Collections;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [Header("引力")]
    [SerializeField] private float gravityRadius = 3f;
    [SerializeField] private float gravityStrength = 20f;

    [Header("捕獲")]
    [SerializeField] private float captureRadius = 0.5f;

    [Header("未捕獲時の消滅")]
    [SerializeField] private float noCaptureLifeTime = 15f;

    [Header("捕獲後の演出")]
    [SerializeField] private float captureWaitTime = 2f;
    [SerializeField] private float pulseDuration = 2f;
    [SerializeField] private float pulseSpeed = 15f;
    [SerializeField] private float pulseAmount = 0.3f;

    [Header("射出")]
    [SerializeField] private float launchSpeed = 25f;

    private Ball capturedBall;
    private Vector3 originalScale;

    private bool isDisappearing;
    private bool isPulsing;

    private Coroutine lifeCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;

        // 捕獲できなかった場合の15秒タイマー
        lifeCoroutine = StartCoroutine(NoCaptureRoutine());
    }

    private void FixedUpdate()
    {
        if (isDisappearing)
            return;

        if (capturedBall != null)
        {
            capturedBall.velocity = Vector2.zero;
            capturedBall.transform.position = transform.position;
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            gravityRadius
        );

        foreach (Collider2D hit in hits)
        {
            Ball ball = hit.GetComponent<Ball>();

            if (ball == null)
                continue;

            Vector2 direction =
                (Vector2)(transform.position - ball.transform.position);

            float distance = direction.magnitude;

            if (distance <= captureRadius)
            {
                CaptureBall(ball);
                break;
            }

            if (distance < gravityRadius)
            {
                Vector2 gravity =
                    direction.normalized * gravityStrength;

                ball.velocity += gravity * Time.fixedDeltaTime;
            }
        }
    }

    private void CaptureBall(Ball ball)
    {
        capturedBall = ball;
        capturedBall.velocity = Vector2.zero;
        capturedBall.transform.position = transform.position;

        // 15秒タイマーを停止
        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
            lifeCoroutine = null;
        }

        // 捕獲後専用の処理を開始
        StartCoroutine(CaptureRoutine());

        Debug.Log("ブラックホールがBallを捕獲！");
    }

    // 捕獲できなかった場合
    private IEnumerator NoCaptureRoutine()
    {
        yield return new WaitForSeconds(noCaptureLifeTime);

        if (capturedBall == null)
        {
            isDisappearing = true;

            Debug.Log("ブラックホール消滅（未捕獲）");

            Destroy(gameObject);
        }
    }

    // 捕獲できた場合
    private IEnumerator CaptureRoutine()
    {
        // 捕獲してから2秒待機
        yield return new WaitForSeconds(captureWaitTime);

        isPulsing = true;

        float timer = 0f;

        // グワングワン演出
        while (timer < pulseDuration)
        {
            timer += Time.deltaTime;

            float pulse =
                1f + Mathf.Sin(timer * pulseSpeed)
                * pulseAmount;

            transform.localScale = originalScale * pulse;

            yield return null;
        }

        isPulsing = false;
        transform.localScale = originalScale;

        Disappear();
    }

    private void Disappear()
    {
        isDisappearing = true;

        if (capturedBall != null)
        {
            Vector2 direction = Random.insideUnitCircle;

            if (direction.sqrMagnitude < 0.001f)
                direction = Vector2.right;

            direction.Normalize();

            // Ballの最大速度を超えないように設定
            float speed = Mathf.Min(
                launchSpeed,
                capturedBall.maxSpeed
            );

            capturedBall.SetSpeed(speed);
            capturedBall.velocity = direction * speed;

            Debug.Log("ブラックホール消滅！Ball高速射出！");
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            transform.position,
            gravityRadius
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            captureRadius
        );
    }
}
