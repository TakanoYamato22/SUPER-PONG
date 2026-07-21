using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BallDroneBreaker : MonoBehaviour
{
    private BallSmashManager smashManager;

    private void Awake()
    {
        smashManager =
            GetComponentInParent<BallSmashManager>();

        Collider2D triggerCollider =
            GetComponent<Collider2D>();

        if (!triggerCollider.isTrigger)
        {
            Debug.LogWarning(
                $"{name} のCollider2DをIs Trigger ONにしてください。",
                this
            );
        }

        if (smashManager == null)
        {
            Debug.LogError(
                "親オブジェクトにBallSmashManagerがありません。",
                this
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryBreakDrone(other);
    }

    /*
     * スマッシュ開始時点ですでにDroneと重なっていた場合にも
     * 壊せるようにStayも入れている。
     */
    private void OnTriggerStay2D(Collider2D other)
    {
        TryBreakDrone(other);
    }

    private void TryBreakDrone(Collider2D other)
    {
        if (smashManager == null)
        {
            return;
        }

        if (!smashManager.IsSmashed)
        {
            return;
        }

        Drone drone =
            other.GetComponent<Drone>();

        if (drone == null)
        {
            drone =
                other.GetComponentInParent<Drone>();
        }

        if (drone == null)
        {
            return;
        }

        drone.BreakDrone();
    }
}