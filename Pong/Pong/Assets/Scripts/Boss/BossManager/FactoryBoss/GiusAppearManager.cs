using UnityEngine;
using System.Collections;

public class GiusAppearManager : MonoBehaviour
{
    [SerializeField] private Transform boss;
    [SerializeField] private BossController bossController;
    [SerializeField] private GiusController giusController;

    [Header("ìoèÍà íu")]
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 targetPosition;

    [Header("ìoèÍéûä‘")]
    [SerializeField] private float appearTime = 1.5f;

    [Header("å¯â âπ")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip appearSE;

    private void Start()
    {
        StartCoroutine(AppearCoroutine());
    }

    private IEnumerator AppearCoroutine()
    {
        Time.timeScale = 0f;

        if (audioSource != null && appearSE != null)
            audioSource.PlayOneShot(appearSE);

        boss.position = startPosition;

        if (bossController != null)
            bossController.SetBattleStarted(false);

        float timer = 0f;

        while (timer < appearTime)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / appearTime;
            boss.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        boss.position = targetPosition;

        Time.timeScale = 1f;

        if (giusController != null)
        {
            giusController.StartMove();
        }

        if (bossController != null)
            bossController.SetBattleStarted(true);
    }
}