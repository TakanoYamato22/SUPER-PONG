using UnityEngine;

public class CenterHitShrink : MonoBehaviour
{
    public Paddle target;
    public int requiredHits = 3;
    public float shrinkDuration = 3f;

    private void OnEnable()
    {
        PaddleEvents.OnCenterHit += HandleCenterHit;
    }

    private void OnDisable()
    {
        PaddleEvents.OnCenterHit -= HandleCenterHit;
    }

    private void HandleCenterHit(Paddle paddle, int count)
    {
        if (paddle == target && count >= requiredHits)
        {
            target.Shrink(shrinkDuration);
        }
    }
}
