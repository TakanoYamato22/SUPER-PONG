
using UnityEngine;

public class EruptionBurnHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        BallBurn ballBurn =
            other.GetComponentInParent<BallBurn>();

        if (ballBurn == null)
            return;

        ballBurn.ApplyBurn();
    }
}
