using UnityEngine;
using System.Collections;

public class BossDamageManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float hitFlashTime = 0.3f;
    [SerializeField] private float invincibleTime = 0.4f;

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

    public IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
    }
}