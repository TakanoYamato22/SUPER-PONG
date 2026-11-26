
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Creature : MonoBehaviour
{
    [Header("捕獲設定")]
    [SerializeField, Min(0.1f)]
    private float captureDuration = 2f;

    private bool hasCaptured;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryCapture(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryCapture(other);
    }

    private void TryCapture(Collider2D other)
    {
        if (hasCaptured)
            return;

        Ball ball = other.GetComponent<Ball>();

        if (ball == null)
            ball = other.GetComponentInParent<Ball>();

        if (ball == null)
            return;

        CreatureCaptureState captureState =
            ball.GetComponent<CreatureCaptureState>();

        if (captureState == null)
            captureState =
                ball.gameObject.AddComponent<CreatureCaptureState>();

        if (captureState.IsCaptured)
            return;

        hasCaptured = true;

        StartCoroutine(
            CaptureRoutine(ball, captureState)
        );
    }

    private IEnumerator CaptureRoutine(
        Ball ball,
        CreatureCaptureState captureState)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        Vector2 savedVelocity = ball.velocity;
        Vector2 savedPosition = rb.position;

        bool wasSimulated = rb.simulated;

        captureState.SetCaptured(true);

        // Ballを捕獲
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        Debug.Log("クリーチャーがBallを捕獲！");

        // 捕獲中はBallをクリーチャーの位置に固定
        float elapsed = 0f;

        while (elapsed < captureDuration)
        {
            if (ball == null)
            {
                Destroy(gameObject);
                yield break;
            }

            ball.transform.position = transform.position;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 元の進行方向と現在の設定速度で解放
        if (ball != null)
        {
            rb.position = transform.position;
            rb.simulated = wasSimulated;

            if (savedVelocity.sqrMagnitude > 0.001f)
            {
                rb.linearVelocity =
                    savedVelocity.normalized * ball.currentSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }

            captureState.SetCaptured(false);
        }

        Debug.Log("Ballを解放！");

        Destroy(gameObject);
    }
}
