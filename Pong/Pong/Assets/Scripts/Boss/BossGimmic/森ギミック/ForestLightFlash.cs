
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ForestLightFlash : MonoBehaviour
{
    private ParticleSystem flashParticle;

    private void Awake()
    {
        flashParticle = GetComponent<ParticleSystem>();
    }

    public void Initialize(float duration, float alpha)
    {
        var main = flashParticle.main;

        main.startLifetime = duration;

        Color color = main.startColor.color;
        color.a = Mathf.Clamp01(alpha);
        main.startColor = color;

        flashParticle.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        flashParticle.Play();

        StartCoroutine(DestroyAfter(duration + 0.2f));
    }

    private IEnumerator DestroyAfter(float duration)
    {
        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }
}
