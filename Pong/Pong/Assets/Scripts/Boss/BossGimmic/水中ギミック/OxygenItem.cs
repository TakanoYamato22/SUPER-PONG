
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OxygenItem : MonoBehaviour
{
    [Header("‰ñ•œ—Ê")]
    [SerializeField, Range(0f, 1f)]
    private float recoverRatio = 0.2f;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BallOxygenManager oxygen =
            other.GetComponentInParent<BallOxygenManager>();

        if (oxygen == null)
            return;

        oxygen.RecoverOxygen(recoverRatio);

        Destroy(gameObject);
    }
}
