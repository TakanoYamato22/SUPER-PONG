using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BouncySurface : MonoBehaviour
{
    public enum ForceType
    {
        Additive,
        Multiplicative
    }

    [Header("通常反射")]
    public ForceType forceType = ForceType.Additive;
    public float bounceStrength = 0f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private ParticleSystem smashEffect;

    private SmashController smashController;
    private CharacterRuntimeStats characterStats;

    private void Awake()
    {
        smashController = GetComponent<SmashController>();

        characterStats =
            GetComponent<CharacterRuntimeStats>();
    }

    private void OnCollisionEnter2D(
        Collision2D collision
    )
    {
        if (
            !collision.gameObject.TryGetComponent(
                out Ball ball
            )
        )
        {
            return;
        }

        BallSmashManager smashManager =
            ball.GetComponent<BallSmashManager>();

        Vector3 hitPoint =
            collision.contacts.Length > 0
            ? collision.contacts[0].point
            : transform.position;

        // ==============================================
        // スマッシュ成功
        // ==============================================

        if (
            smashController != null &&
            smashController.CanSmashNow()
        )
        {
            if (
                smashManager != null &&
                smashManager.IsSmashed
            )
            {
                smashManager.SmashReturn();
            }
            else if (smashManager != null)
            {
                smashManager.ApplySmash();
            }

            ApplyCharacterSmashMultiplier(ball);

            smashController.SuccessSmash();

            PlayEffect(
                smashEffect,
                hitPoint
            );

            return;
        }

        // ==============================================
        // 相手のスマッシュを通常反射
        // ==============================================

        if (
            smashManager != null &&
            smashManager.IsSmashed
        )
        {
            smashManager.EndSmash();

            PlayEffect(
                hitEffect,
                hitPoint
            );

            return;
        }

        // ==============================================
        // 通常反射
        // ==============================================

        switch (forceType)
        {
            case ForceType.Additive:

                ball.IncreaseSpeed(
                    bounceStrength
                );

                break;

            case ForceType.Multiplicative:

                float multiplier =
                    bounceStrength;

                float addAmount =
                    ball.currentSpeed *
                    (multiplier - 1f);

                ball.IncreaseSpeed(
                    addAmount
                );

                break;
        }

        PlayEffect(
            hitEffect,
            hitPoint
        );
    }

    private void ApplyCharacterSmashMultiplier(
        Ball ball
    )
    {
        if (
            ball == null ||
            characterStats == null
        )
        {
            return;
        }

        float multiplier =
            characterStats
                .FinalSmashSpeedMultiplier;

        bool allowBelowBaseSpeed =
            multiplier < 1f;

        ball.ApplyCharacterSpeedMultiplier(
            multiplier,
            allowBelowBaseSpeed
        );

        Debug.Log(
            gameObject.name +
            " Smash倍率: " +
            multiplier
        );
    }

    private void PlayEffect(
        ParticleSystem effect,
        Vector3 position
    )
    {
        if (effect == null)
            return;

        effect.transform.position = position;

        if (effect.isPlaying)
        {
            effect.Stop(
                true,
                ParticleSystemStopBehavior
                    .StopEmittingAndClear
            );
        }

        effect.Play();
    }
}