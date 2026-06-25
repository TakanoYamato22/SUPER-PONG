using UnityEngine;
using System.Collections;

public class BossDamageManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float hitFlashTime = 0.3f;
    [SerializeField] private float hitInvincibleTime = 1.5f;

    // ★追加した場所1：インスペクターにエフェクトを登録する枠
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private ParticleSystem smashEffect;

    private bool isInvincible;

    public bool IsInvincible => isInvincible;

    public void PlayHitEffect(MonoBehaviour owner)
    {
        owner.StartCoroutine(HitFlash());
        owner.StartCoroutine(InvincibleCoroutine());

        // ★追加した場所2：ダメージを受けた瞬間にエフェクトを呼び出す
        PlayParticleEffect();
    }

    public IEnumerator HitFlash()
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(hitFlashTime);

        spriteRenderer.color = Color.white;
    }

    // 被弾後無敵
    public IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(hitInvincibleTime);

        isInvincible = false;
    }

    // 開幕無敵用
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

    // ★追加した場所3：一番最後のカッコの手前にこれを追加しました！
    private void PlayParticleEffect()
    {
        Vector3 spawnPosition = transform.position;

        bool isSmash = Input.GetKey(KeyCode.Space);

        if (isSmash)
        {
            if (smashEffect != null)
            {
                // ★追加：エフェクトの親をボス自身にして、動きを追従させる
                smashEffect.transform.SetParent(this.transform);

                smashEffect.transform.position = spawnPosition;
                if (smashEffect.isPlaying) smashEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                smashEffect.Play();
                Debug.Log("<color=red><b>[ボス] スマッシュダメージ！</b></color>");
            }
        }
        else
        {
            if (hitEffect != null)
            {
                // ★追加：エフェクトの親をボス自身にして、動きを追従させる
                hitEffect.transform.SetParent(this.transform);

                hitEffect.transform.position = spawnPosition;
                if (hitEffect.isPlaying) hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                hitEffect.Play();
            }
        }
    }
}