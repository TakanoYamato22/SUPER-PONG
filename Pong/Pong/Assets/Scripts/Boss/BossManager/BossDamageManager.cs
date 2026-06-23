using UnityEngine;
using System.Collections;

public class BossDamageManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float hitFlashTime = 0.3f;
    [SerializeField] private float hitInvincibleTime = 1.5f;

    private bool isInvincible;

    public bool IsInvincible => isInvincible;

    public void PlayHitEffect(MonoBehaviour owner)
    {
        owner.StartCoroutine(HitFlash());
        owner.StartCoroutine(InvincibleCoroutine());
    }

    public IEnumerator HitFlash()
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(hitFlashTime);

        spriteRenderer.color = Color.white;
    }

    // ”í’eŒã–³“G
    public IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(hitInvincibleTime);

        isInvincible = false;
    }

    // ŠJ–‹–³“G—p
    public IEnumerator StartInvincible(float time)
    {
        isInvincible = true;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.yellow;

        yield return new WaitForSeconds(time);

        isInvincible = false;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }
}