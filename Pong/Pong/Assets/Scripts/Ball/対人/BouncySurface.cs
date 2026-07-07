using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BouncySurface : MonoBehaviour
{
    public enum ForceType
    {
        Additive,
        Multiplicative,
    }

    public ForceType forceType = ForceType.Additive;
    public float bounceStrength = 0f;

    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private ParticleSystem smashEffect;

    private SmashController smashController;

    private void Awake()
    {
        smashController = GetComponent<SmashController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Ball ball))
        {
            return;
        }

        BallSmashManager smashManager = ball.GetComponent<BallSmashManager>();

        Vector3 hitPoint = collision.contacts.Length > 0
            ? collision.contacts[0].point
            : transform.position;

        if (smashController != null && smashController.CanSmashNow())
        {
            if (smashManager != null && smashManager.IsSmashed)
            {
                smashManager.SmashReturn();
            }
            else if (smashManager != null)
            {
                smashManager.ApplySmash();
            }

            smashController.SuccessSmash();
            PlayEffect(smashEffect, hitPoint);
            return;
        }

        if (smashManager != null && smashManager.IsSmashed)
        {
            smashManager.EndSmash();
            PlayEffect(hitEffect, hitPoint);
            return;
        }

        switch (forceType)
        {
            case ForceType.Additive:
                ball.IncreaseSpeed(bounceStrength);
                break;

            case ForceType.Multiplicative:
                float multiplier = bounceStrength;
                float addAmount = ball.currentSpeed * (multiplier - 1f);
                ball.IncreaseSpeed(addAmount);
                break;
        }

        PlayEffect(hitEffect, hitPoint);
    }

    private void PlayEffect(ParticleSystem effect, Vector3 position)
    {
        if (effect == null) return;

        effect.transform.position = position;

        if (effect.isPlaying)
        {
            effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        effect.Play();
    }
}